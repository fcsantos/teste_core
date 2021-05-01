using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Core.Api.Extensions;
using Core.Api.ViewModels;
using Core.Business.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<AppSettings> appSettings,                              
                              ILogger<AuthController> logger, 
                              IEmailSender emailSender, 
                              IMapper mapper, 
                              RoleManager<IdentityRole> roleManager,
                              IPatientRepository patientRepository,
                              IDoctorRepository doctorRepository,
                              INotifier notifier, IUser user) : base(notifier, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
            _roleManager = roleManager;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        [ClaimsAuthorize("Account", "Register")]
        [HttpPost("new-account")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var resultRole = await _userManager.AddToRoleAsync(user, registerUser.Role);
                if (resultRole.Succeeded)
                {
                    await _emailSender.SendEmailAsync(
                        string.IsNullOrEmpty(registerUser.Name) ? registerUser.Email : string.Format("{0},{1}", registerUser.Name, registerUser.Email),
                        "Utilizador cadastrado",
                        $"Favor aceda com o link: { HtmlEncoder.Default.Encode(_appSettings.UrlLogin)} " + " com a password " + registerUser.Password);

                    return CustomResponse(await GenerateJwt(registerUser.Email));
                }

                foreach (var error in resultRole.Errors)
                {
                    NotifyError(error.Description);
                }
            }
            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [ClaimsAuthorize("Account", "RegisterUser")]
        [HttpPost("new-account-user")]
        public async Task<ActionResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                var resultRole = await _userManager.AddToRoleAsync(user, registerUser.Role);
                if (resultRole.Succeeded) {

                    var claims = registerUser.Role == _appSettings.RolePatient ? _appSettings.ClaimsListPatient : registerUser.Role == _appSettings.RoleDoctor ? _appSettings.ClaimsListDoctor : new Dictionary<string, string[]>();
                    foreach (var claim in claims)
                    {
                        foreach (var item in claim.Value)
                        {
                            await _userManager.AddClaimAsync(user, new Claim(claim.Key, item));
                        }
                    }
                    return CustomResponse(await GenerateJwt(registerUser.Email)); 
                }

                foreach (var error in resultRole.Errors)
                {
                    NotifyError(error.Description);
                }
            }
            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUser);
        }

        [HttpPost("forgot-my-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel input)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                NotifyError("Se você possui uma conta conosco, enviamos um e-mail com as instruções para redefinir sua password.");
                return CustomResponse(input);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = string.Format("{0}?code={1}", _appSettings.UrlResetPassword,code);

            await _emailSender.SendEmailAsync(
                input.Email,
                "Redefinir senha",
                $"Redefina sua senha clicando no link: { HtmlEncoder.Default.Encode(callbackUrl)}");

            return CustomResponse(input);
        }
        
        [HttpPost("reset-password")]
        public async Task<ActionResult<ResetPasswordViewModel>> ResetPassword(ResetPasswordViewModel input)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                NotifyError("Se você possui uma conta conosco, enviamos um e-mail com as instruções para redefinir sua password.");
                return CustomResponse(input);
            }

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code)), input.Password);
            if (!result.Succeeded)
            {
                NotifyError(result.Errors.FirstOrDefault().Description);
                return CustomResponse(input);
            }

            return CustomResponse("Senha alterada com sucesso.");
        }

        [ClaimsAuthorize("User", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) { return NotFound(); }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                NotifyError(result.Errors.FirstOrDefault().Description);                
                return CustomResponse(user);
            }

            return CustomResponse(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (_patientRepository.Search(p => !p.IsActive.Value && p.Email == loginUser.Email).Result.Any())
            {
                NotifyError("Usuário está inativo");
                return CustomResponse(loginUser);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Usuário " + loginUser.Email + " logado com sucesso");
                return CustomResponse(await GenerateJwt(loginUser.Email));
            }
            if (result.IsLockedOut)
            {
                NotifyError("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotifyError("Usuário ou Senha incorretos");
            return CustomResponse(loginUser);
        }

        [ClaimsAuthorize("Account", "Register")]
        [HttpGet("roles/getAll")]
        public IEnumerable<RoleViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(_roleManager.Roles);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<ResetPasswordViewModel>> ChangePassword(ChangePasswordViewModel input)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                NotifyError("Se você possui uma conta conosco, enviamos um e-mail com as instruções para redefinir sua password.");
                return CustomResponse(input);
            }

            var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            if (!result.Succeeded)
            {
                NotifyError(result.Errors.FirstOrDefault().Description);
                return CustomResponse(input);
            }

            return CustomResponse("Senha alterada com sucesso.");
        }

        private async Task<LoginResponseViewModel> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var patient = await _patientRepository.GetPatientByUserId(user.Id);
            var doctor = await _doctorRepository.GetDoctorByUserId(user.Id);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));         
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(ClaimTypes.Name, patient != null ? string.Format("{0} {1}", patient.FirstName, patient.LastName) : doctor != null ? doctor.Name : email));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }            

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidOn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}