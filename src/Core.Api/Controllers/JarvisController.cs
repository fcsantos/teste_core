using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Api.Extensions;
using Core.Business.Intefaces;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/jarvis")]
    public class JarvisController : MainController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageService _messageService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly AppSettings _appSettings;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientService _patientService;
        private readonly IInquiryScheduleRepository _inquiryScheduleRepository;
        private readonly IInquiryScheduleService _inquiryScheduleService;

        public JarvisController(IMessageRepository messageRepository,
                                IMessageService messageService,
                                UserManager<IdentityUser> userManager,
                                IEmailSender emailSender,
                                IOptions<AppSettings> appSettings,
                                IDoctorRepository doctorRepository,
                                IPatientRepository patientRepository,
                                IPatientService patientService,
                                IInquiryScheduleRepository inquiryScheduleRepository,
                                IInquiryScheduleService inquiryScheduleService,
                                INotifier notifier, IUser user) : base(notifier, user)
        {
            _messageRepository = messageRepository;
            _messageService = messageService;
            _userManager = userManager;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _patientService = patientService;
            _inquiryScheduleRepository = inquiryScheduleRepository;
            _inquiryScheduleService = inquiryScheduleService;
        }

        [HttpGet("send-access-new-patient")]
        public async Task<ActionResult> SendAccessToNewPatient()
        {
            var password = PasswordGenerator.GenerateRandomPassword();

            var patients = await _patientRepository.GetPatientIsMailNotSender();

            foreach (var patient in patients)
            {
                var user = await _userManager.FindByEmailAsync(patient.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return CustomResponse("Email " + patient.Email + " não encontrado");
                }

                var result = await _userManager.ChangePasswordAsync(user, _appSettings.DefaultPassword, password);
                if (!result.Succeeded)
                {
                    return CustomResponse(result.Errors.FirstOrDefault());
                }

                await _emailSender.SendEmailAsync(
                    string.Format("{0},{1}", patient.FirstName + " " + patient.LastName, patient.Email),
                    "Utilizador cadastrado",
                    $"Favor aceda com o link: { HtmlEncoder.Default.Encode(_appSettings.UrlLogin)} " + " com a password " + password);
                
                await _patientService.UpdateEmailSender(patient);
            }

            return CustomResponse("Emails enviados com sucesso");
        }

        [HttpGet("notification-new-message-patient")]
        public async Task<ActionResult> NotificationOfNewMessageToPatient()
        {
            var newMessages = await _messageRepository.GetAllNewMessagesToPatientIsMailNotSender();

            foreach (var message in newMessages)
            {
                await _emailSender.SendEmailAsync(
                    string.Format("{0},{1}", message.Patient.FirstName + " " + message.Patient.LastName, message.Patient.Email),
                    "Notificação de nova mensagem enviada pelo médico",
                    $"Você recebeu uma nova mensagem de {message.Doctor.Name}. Favor aceda com o link: { HtmlEncoder.Default.Encode(_appSettings.Url)}");
                
                await _messageService.UpdateEmailSender(message);
            }

            return CustomResponse("Emails enviados com sucesso");
        }

        [HttpGet("notification-inquiry-schedule-patient")]
        public async Task<ActionResult> NotificationOfInquiryScheduleToPatient()
        {
            var schedules = await _inquiryScheduleRepository.GetAllInquiryScheduleIsMailNotSender();

            foreach (var schedule in schedules)
            {
                await _emailSender.SendEmailAsync(
                    string.Format("{0},{1}", schedule.Patient.FirstName + " " + schedule.Patient.LastName, schedule.Patient.Email),
                    "Notificação de um inquérito enviado pelo médico",
                    $"Você recebeu um inquérito de {schedule.Doctor.Name}. Favor aceda com o link: { HtmlEncoder.Default.Encode(_appSettings.Url)}");

                await _inquiryScheduleService.UpdateEmailSender(schedule);
            }

            return CustomResponse("Emails enviados com sucesso");
        }

        public static class PasswordGenerator
        {
            /// <summary>
            /// Generates a Random Password
            /// respecting the given strength requirements.
            /// </summary>
            /// <param name="opts">A valid PasswordOptions object
            /// containing the password strength requirements.</param>
            /// <returns>A random password</returns>
            public static string GenerateRandomPassword(PasswordOptions opts = null)
            {
                if (opts == null) opts = new PasswordOptions()
                {
                    RequiredLength = 8,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };

                string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                "0123456789",                   // digits
                "!@$?_-"                        // non-alphanumeric
        };
                CryptoRandom rand = new CryptoRandom();
                List<char> chars = new List<char>();

                if (opts.RequireUppercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[0][rand.Next(0, randomChars[0].Length)]);

                if (opts.RequireLowercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[1][rand.Next(0, randomChars[1].Length)]);

                if (opts.RequireDigit)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[2][rand.Next(0, randomChars[2].Length)]);

                if (opts.RequireNonAlphanumeric)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[3][rand.Next(0, randomChars[3].Length)]);

                for (int i = chars.Count; i < opts.RequiredLength
                    || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
                {
                    string rcs = randomChars[rand.Next(0, randomChars.Length)];
                    chars.Insert(rand.Next(0, chars.Count),
                        rcs[rand.Next(0, rcs.Length)]);
                }

                return new string(chars.ToArray());
            }
        }
    }
}
