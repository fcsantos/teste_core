using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    Location = table.Column<string>(type: "varchar(200)", nullable: false),
                    Number = table.Column<string>(type: "varchar(50)", nullable: false),
                    Complement = table.Column<string>(type: "varchar(250)", nullable: true),
                    PostalCode = table.Column<string>(type: "char(8)", nullable: false),
                    District = table.Column<string>(type: "varchar(100)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    State = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    CategoriaId = table.Column<Guid>(nullable: true),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Controllers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controllers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(100)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Cell = table.Column<string>(type: "varchar(20)", nullable: false),
                    DocumentCard = table.Column<string>(type: "varchar(20)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyChannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false),
                    Cell = table.Column<string>(type: "varchar(20)", nullable: false),
                    sortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inquiries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    Title = table.Column<string>(type: "varchar(150)", nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SendToAllUsers = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pathologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ParentPathologyId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pathologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pathologies_Pathologies_ParentPathologyId",
                        column: x => x.ParentPathologyId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ParentSpecialtyId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_Specialties_ParentSpecialtyId",
                        column: x => x.ParentSpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Documento = table.Column<string>(type: "char(14)", nullable: false),
                    TipoFornecedor = table.Column<int>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedores_Adresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Adresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(type: "varchar(100)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Document = table.Column<string>(type: "varchar(20)", nullable: false),
                    DocumentCard = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Cell = table.Column<string>(type: "varchar(20)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    Gender = table.Column<int>(nullable: false),
                    IsMailSender = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Adresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Adresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "varchar(100)", nullable: true),
                    ControllerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Controllers_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Controllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ServiceName = table.Column<string>(type: "varchar(100)", nullable: true),
                    DoctorId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    InquiryId = table.Column<Guid>(nullable: false),
                    InquiryTitle = table.Column<string>(type: "varchar(100)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    InquiryScheduleId = table.Column<Guid>(nullable: false),
                    QuestionTitle = table.Column<string>(type: "varchar(100)", nullable: true),
                    AnswerOptionId = table.Column<Guid>(nullable: false),
                    AnswerText = table.Column<string>(type: "varchar(100)", nullable: true),
                    AnswerValue = table.Column<decimal>(type: "decimal(5, 2)", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAnswers_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 250, nullable: false),
                    Placeholder = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLine = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    SortOrder = table.Column<byte>(nullable: false, defaultValueSql: "((99))"),
                    TypeOfAnswer = table.Column<string>(type: "varchar(20)", nullable: false),
                    InquiryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClinicalSummaryFacilitators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PathologyId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    TypeClinicalSummary = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalSummaryFacilitators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicalSummaryFacilitators_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClinicalSummaryFacilitators_Pathologies_PathologyId",
                        column: x => x.PathologyId,
                        principalTable: "Pathologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialties",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(nullable: false),
                    SpecialtyId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(100)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialties", x => new { x.DoctorId, x.SpecialtyId });
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    FornecedorId = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Imagem = table.Column<string>(type: "varchar(100)", nullable: false),
                    Valor = table.Column<decimal>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allergies_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allergies_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarePlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", nullable: true),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarePlans_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarePlans_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consultations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InquiriesSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    InquiryId = table.Column<Guid>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Answered = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    IsMailSender = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InquiriesSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InquiriesSchedule_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InquiriesSchedule_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InquiriesSchedule_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    ReplyMessageId = table.Column<Guid>(nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(type: "varchar(5000)", nullable: false),
                    StatusMessage = table.Column<string>(type: "varchar(20)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1"),
                    IsReply = table.Column<bool>(type: "bit", nullable: false),
                    IsMailSender = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Messages_ReplyMessageId",
                        column: x => x.ReplyMessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NoticeUsers",
                columns: table => new
                {
                    NoticeId = table.Column<Guid>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeUsers", x => new { x.NoticeId, x.PatientId });
                    table.ForeignKey(
                        name: "FK_NoticeUsers_Notices_NoticeId",
                        column: x => x.NoticeId,
                        principalTable: "Notices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoticeUsers_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    DoctorId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(type: "varchar(5000)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Observations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDoctors",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDoctors", x => new { x.DoctorId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_ServiceDoctors_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceDoctors_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(max)", nullable: true),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Option = table.Column<string>(type: "varchar(250)", nullable: false),
                    SortOrder = table.Column<byte>(nullable: true, defaultValueSql: "((99))"),
                    AnswerValue = table.Column<decimal>(type: "decimal(5, 2)", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_ControllerId",
                table: "Actions",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_DoctorId",
                table: "Allergies",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_PatientId",
                table: "Allergies",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlans_DoctorId",
                table: "CarePlans",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarePlans_PatientId",
                table: "CarePlans",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CategoriaId",
                table: "Categorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalSummaryFacilitators_DoctorId",
                table: "ClinicalSummaryFacilitators",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalSummaryFacilitators_PathologyId",
                table: "ClinicalSummaryFacilitators",
                column: "PathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_DoctorId",
                table: "Consultations",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_PatientId",
                table: "Consultations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_DoctorId",
                table: "Diagnostics",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_PatientId",
                table: "Diagnostics",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                table: "DoctorSpecialties",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_AddressId",
                table: "Fornecedores",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InquiriesSchedule_DoctorId",
                table: "InquiriesSchedule",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiriesSchedule_InquiryId",
                table: "InquiriesSchedule",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_InquiriesSchedule_PatientId",
                table: "InquiriesSchedule",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DoctorId",
                table: "Messages",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PatientId",
                table: "Messages",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReplyMessageId",
                table: "Messages",
                column: "ReplyMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeUsers_PatientId",
                table: "NoticeUsers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_DoctorId",
                table: "Observations",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_PatientId",
                table: "Observations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Pathologies_ParentPathologyId",
                table: "Pathologies",
                column: "ParentPathologyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAnswers_InquiryId",
                table: "PatientAnswers",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AddressId",
                table: "Patients",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_FornecedorId",
                table: "Produtos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_InquiryId",
                table: "Questions",
                column: "InquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDoctors_ServiceId",
                table: "ServiceDoctors",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DoctorId",
                table: "Services",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_ParentSpecialtyId",
                table: "Specialties",
                column: "ParentSpecialtyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "CarePlans");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "ClinicalSummaryFacilitators");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "DoctorSpecialties");

            migrationBuilder.DropTable(
                name: "EmergencyChannel");

            migrationBuilder.DropTable(
                name: "InquiriesSchedule");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NoticeUsers");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "PatientAnswers");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "ServiceDoctors");

            migrationBuilder.DropTable(
                name: "Controllers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Pathologies");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropTable(
                name: "Notices");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Inquiries");

            migrationBuilder.DropTable(
                name: "Adresses");

            migrationBuilder.DropTable(
                name: "Doctors");
        }
    }
}
