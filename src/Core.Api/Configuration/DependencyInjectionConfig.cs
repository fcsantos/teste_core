using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Core.Api.Extensions;
using Core.Business.Intefaces;
using Core.Business.Notifications;
using Core.Business.Services;
using Core.Data.Context;
using Core.Data.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<INoticeUserRepository, NoticeUserRepository>();
            services.AddScoped<IDoctorSpecialtyRepository, DoctorSpecialtyRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
            services.AddScoped<IPathologyRepository, PathologyRepository>();
            services.AddScoped<IAppControllerRepository, AppControllerRepository>();
            services.AddScoped<IAppActionRepository, AppActionRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IEmergencyChannelRepository, EmergencyChannelRepository>();
            services.AddScoped<IClinicalSummaryFacilitatorRepository, ClinicalSummaryFacilitatorRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceDoctorRepository, ServiceDoctorRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IAllergyRepository, AllergyRepository>();
            services.AddScoped<IDiagnosticRepository, DiagnosticRepository>();
            services.AddScoped<ICarePlanRepository, CarePlanRepository>();
            services.AddScoped<IObservationRepository, ObservationRepository>();
            services.AddScoped<IInquiryRepository, InquiryRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IInquiryScheduleRepository, InquiryScheduleRepository>();
            services.AddScoped<IPatientAnswersRepository, PatientAnswersRepository>();

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<INoticeService, NoticeService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IPathologyService, PathologyService>();
            services.AddScoped<IAppControllerService, AppControllerService>();
            services.AddScoped<IAppActionService, AppActionService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IEmergencyChannelService, EmergencyChannelService>();
            services.AddScoped<IClinicalSummaryFacilitatorService, ClinicalSummaryFacilitatorService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<IAllergyService, AllergyService>();
            services.AddScoped<IDiagnosticService, DiagnosticService>();
            services.AddScoped<ICarePlanService, CarePlanService>();
            services.AddScoped<IObservationService, ObservationService>();
            services.AddScoped<IInquiryService, InquiryService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IInquiryScheduleService, InquiryScheduleService>();
            services.AddScoped<IPatientAnswersService, PatientAnswersService>();

            services.AddScoped<IDapperDbRepository, DapperDbRepository>();            

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}
