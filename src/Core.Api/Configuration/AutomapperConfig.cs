using AutoMapper;
using Core.Api.ViewModels;
using Core.Business.Models;
using Core.Business.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Core.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();

            CreateMap<NoticeViewModel, Notice>()
                .ForMember(dest => dest.NoticeUsers, opt => opt.MapFrom(src => src.NoticeUsers.Select(id => new NoticeUser { PatientId = id })));

            CreateMap<Notice, NoticeViewModel>()
                .ForMember(dest => dest.NoticeUsers, opt => opt.MapFrom(src => src.NoticeUsers.Select(d => d.PatientId)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.StartDateFormat, opt => opt.MapFrom(src => src.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDateFormat, opt => opt.MapFrom(src => src.EndDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.PatientsName, opt => opt.MapFrom(src => src.SendToAllUsers ? "Enviado para todos os Pacientes" : string.Join(", ", src.NoticeUsers.Select(d => string.Format("{0} {1}", d.Patient.FirstName, d.Patient.LastName)))));

            CreateMap<PatientViewModel, Patient>();
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));

            CreateMap<DoctorViewModel, Doctor>()
                .ForMember(dest => dest.DoctorSpecialties,
                    opt => opt.MapFrom(src =>
                       src.SpecialtyGuids.Select(id => new DoctorSpecialty { SpecialtyId = Guid.Parse(id) }))
                );

            CreateMap<Doctor, DoctorViewModel>()
                .ForMember(dest => dest.SpecialtiesNames,
                    opt => opt.MapFrom(src =>
                        string.Join(", ", src.DoctorSpecialties.Select(d => d.Specialty.Name)))
                 )
                .ForMember(dest => dest.SpecialtyGuids,
                    opt => opt.MapFrom(src => src.DoctorSpecialties.Select(de => de.SpecialtyId))
                )
                .ForMember(dest => dest.SystemSpecialties,
                    opt => opt.MapFrom(src => src.DoctorSpecialties.Select(de => de.Specialty))
                )
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<EmergencyChannel, EmergencyChannelViewModel>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<SpecialtyViewModel, Specialty>();
            CreateMap<Specialty, SpecialtyViewModel>()
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.ParentSpecialty.Name));

            CreateMap<PathologyViewModel, Pathology>();
            CreateMap<Pathology, PathologyViewModel>()
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.ParentPathology.Name));

            CreateMap<AppAction, AppActionViewModel>().ReverseMap();
            CreateMap<AppController, AppControllerViewModel>().ReverseMap();

            CreateMap<AllUsersDto, AllUsersViewModel>().ReverseMap();

            CreateMap<UserClaimsDto, UserClaimsViewModel>().ReverseMap();

            CreateMap<ServiceViewModel, Service>()
                .ForMember(dest => dest.ServiceDoctors,
                    opt => opt.MapFrom(src =>
                       src.ServiceDoctors.Select(id => new ServiceDoctor { DoctorId = id })
                       ));

            CreateMap<Service, ServiceViewModel>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.ServiceDoctors,
                    opt => opt.MapFrom(src => src.ServiceDoctors.Select(d => d.DoctorId)))
                    .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<ClinicalSummaryFacilitatorViewModel, ClinicalSummaryFacilitator>();
            CreateMap<ClinicalSummaryFacilitator, ClinicalSummaryFacilitatorViewModel>()
                .ForMember(dest => dest.PathologyName, opt => opt.MapFrom(src => src.Pathology.Name))
                .ForMember(dest => dest.TypeClinicalSummaryFormat, opt => opt.MapFrom(src => src.TypeClinicalSummary == TypeClinicalSummary.Allergy ? "Alergia" :
                                                                                             src.TypeClinicalSummary == TypeClinicalSummary.Diagnostic ? "Diagnóstico" : "Observação"));

            CreateMap<MessageViewModel, Message>();
            CreateMap<Message, MessageViewModel>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.StatusMessageFormat, opt => opt.MapFrom(src => src.StatusMessage == StatusMessage.Answered ? "Respondido" :
                                                                                       src.StatusMessage == StatusMessage.AwaitingResponse ? "Aguardando Resposta" :
                                                                                       src.StatusMessage == StatusMessage.Generated ? "Gerado" :
                                                                                       src.StatusMessage == StatusMessage.Read ? "Lido" : "Enviado"));

            CreateMap<ConsultationViewModel, Consultation>();
            CreateMap<Consultation, ConsultationViewModel>()
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.Date.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name));

            CreateMap<AllergyViewModel, Allergy>();
            CreateMap<Allergy, AllergyViewModel>()
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<DiagnosticViewModel, Diagnostic>();
            CreateMap<Diagnostic, DiagnosticViewModel>()
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<CarePlanViewModel, CarePlan>();
            CreateMap<CarePlan, CarePlanViewModel>()
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<ObservationViewModel, Observation>();
            CreateMap<Observation, ObservationViewModel>()
                .ForMember(dest => dest.DateFormat, opt => opt.MapFrom(src => src.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));

            CreateMap<InquiryViewModel, Inquiry>();
            CreateMap<Inquiry, InquiryViewModel>()
             .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive.Value ? "Ativo" : "Inativo"));


            CreateMap<Question, QuestionViewModel>().ReverseMap();
            CreateMap<AnswerOptionsViewModel, AnswerOption>().ReverseMap();

            CreateMap<ComboDto, ComboViewModel>().ReverseMap();

            CreateMap<InquirySchedule, InquiryScheduleViewModel>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.InquiryTitle, opt => opt.MapFrom(src => src.Inquiry.Title));

            CreateMap<InquiryScheduleViewModel, InquirySchedule>();

            CreateMap<PatientAnswers, PatientAnswersViewModel>().ReverseMap();

        }
    }
}
