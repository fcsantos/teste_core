using Core.Business.Intefaces;
using Core.Business.Models;
using Core.Business.Models.DTO;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data.Repository
{
    public class DapperDbRepository : IDapperDbRepository
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<DapperDbRepository> _logger;
        private readonly IUser _user;

        public DapperDbRepository(IConfiguration configuration, ILogger<DapperDbRepository> logger, IUser user)
        {
            _configuration = configuration;
            _logger = logger;
            _user = user;
        }

        #region Claims
        public async Task<IEnumerable<UserClaimsDto>> GetAllUserClaimsByUserId(string userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = $"SELECT * FROM(SELECT u.Id, u.UserId, ClaimType, ClaimValue, d.Name FROM AspNetUserClaims u " +
                              $"INNER JOIN Doctors d on u.UserId = d.UserId " +
                              $"UNION " +
                              $"SELECT u.Id, u.UserId, ClaimType, ClaimValue, CONCAT(p.FirstName, ' ', p.LastName) AS Name FROM AspNetUserClaims u " +
                              $"INNER JOIN Patients p on u.UserId = p.UserId " +
                              $"UNION " +
                              $"SELECT u.Id, u.UserId, ClaimType, ClaimValue,'Utilizador' AS Name FROM AspNetUserClaims u " +
                              $"WHERE u.UserId NOT IN(SELECT D.UserId FROM Doctors D) AND " +
                              $"u.UserId NOT IN(SELECT P.UserId FROM Patients P)) T " +
                              $"WHERE T.UserId = '{userId}'";
                    return await conn.QueryAsync<UserClaimsDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllUserClaimsByUserId: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserClaimsDto> GetUserClaimsById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    return await conn.QueryFirstAsync<UserClaimsDto>($"SELECT Id, UserId, ClaimType, ClaimValue FROM AspNetUserClaims WHERE Id = {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetUserClaimsById: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<AllUsersDto>> GetAllUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = $"SELECT U.Id, U.Email, CONCAT(P.FirstName, ' ',P.LastName) AS Name, 'Paciente' as UserType FROM Patients P " +
                              $"INNER JOIN AspNetUsers U ON P.UserId = U.Id " +
                              $"UNION " +
                              $"SELECT U.Id, U.Email, D.Name, 'Médico' as UserType FROM Doctors D " +
                              $"INNER JOIN AspNetUsers U ON D.UserId = U.Id " +
                              $"UNION " +
                              $"SELECT U.Id, U.Email, U.Email AS Name, 'Utilizador' as UserType FROM AspNetUsers U " +
                              $"WHERE Id NOT IN(SELECT D.UserId FROM Doctors D) AND " +
                              $"Id NOT IN(SELECT P.UserId FROM Patients P) " +
                              $"ORDER BY UserType ASC";
                    return await conn.QueryAsync<AllUsersDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllUsers: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Combos
        public async Task<IEnumerable<ComboDto>> GetAllClinicalSummaryFacilitator(string type)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM ClinicalSummaryFacilitators WHERE DoctorId IN (SELECT Id FROM Doctors WHERE UserId = @UserId) AND TypeClinicalSummary = @TypeClinicalSummary";
                    return await conn.QueryAsync<ComboDto>(sql, new { UserId = _user.GetUserId(), TypeClinicalSummary = type });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllClinicalSummaryFacilitator: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllPathologies()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM Pathologies";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllPathologies: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllInquiries()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Title AS Name FROM Inquiries WHERE isActive=1 ORDER BY Name";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllInquiries: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllParentPathologies()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM Pathologies WHERE ParentPathologyId IS NULL";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllParentPathologies: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllParentSpecialties()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM Specialties WHERE ParentSpecialtyId IS NULL";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllParentSpecialties: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllSpecialties()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM Specialties";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllSpecialties: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllPatients()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, CONCAT(Document, ' - ', FirstName, ' ', LastName) AS Name FROM Patients";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllPatients: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboDto>> GetAllDoctors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT Id, Name FROM Doctors";
                    return await conn.QueryAsync<ComboDto>(sql);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllDoctors: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<PagedResult<Patient>> GetAllPatientsPaged(int pageSize, int pageIndex, string query = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var sql = @$"SELECT * FROM Patients 
                             WHERE (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%' OR @LastName IS NULL OR LastName LIKE '%' + @LastName + '%') 
                             ORDER BY FirstName 
                             OFFSET {pageSize * (pageIndex - 1)} ROWS 
                             FETCH NEXT {pageSize} ROWS ONLY 
                             SELECT COUNT(Id) FROM Patients 
                             WHERE (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%' OR @LastName IS NULL OR LastName LIKE '%' + @LastName + '%')";

                    var multi = await conn.QueryMultipleAsync(sql, new { FirstName = query, LastName = query });

                    var patients = multi.Read<Patient>();
                    var total = multi.Read<int>().FirstOrDefault();

                    return new PagedResult<Patient>()
                    {
                        List = patients,
                        TotalResults = total,
                        PageIndex = pageIndex,
                        PageSize = pageSize,
                        Query = query
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAllPatientsPaged: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
