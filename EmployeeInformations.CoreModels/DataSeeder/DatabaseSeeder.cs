using EmployeeInformations.CoreModels.Configuration;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmployeeInformations.CoreModels.DataSeeder
{
    public class DatabaseSeeder
    {
        private readonly IConfiguration _configuration;
        private readonly EmployeesDbContext _context;

        public DatabaseSeeder(IConfiguration configuration, EmployeesDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public void Seed()
        {
            // Ensure database is created
            _context.Database.EnsureCreated();

            // Seed in order (respecting foreign key constraints)
            if (!_context.CountryEntities.Any())
            {
                SeedCountry();
            }

            if (!_context.state.Any())
            {
                SeedStates();
            }

            if (!_context.cities.Any())
            {
                SeedCities();
            }

            if (!_context.Company.Any())
            {
                SeedCompany();
            }

            // Get company ID for company-specific data
            var company = _context.Company.FirstOrDefault();
            if (company != null)
            {
                if (!_context.Departments.Any())
                {
                    SeedDepartments(company.CompanyId);
                }

                if (!_context.Designations.Any())
                {
                    SeedDesignations(company.CompanyId);
                }

                if (!_context.RoleEntities.Any())
                {
                    SeedRoles(company.CompanyId);
                }

                if (!_context.RelievingReasonEntity.Any())
                {
                    SeedReleaveTypes(company.CompanyId);
                }                

                if (!_context.documentTypesEntities.Any())
                {
                    SeedDocumentTypes(company.CompanyId);
                }

                if (!_context.SkillSets.Any())
                {
                    SeedSkillSets(company.CompanyId);
                }

                if (!_context.BloodGroup.Any())
                {
                    SeedBloodGroups();
                }

                if (!_context.Relationship.Any())
                {
                    SeedRelationshipTypes();
                }

                if (!_context.Employees.Any())
                {
                    SeedSuperAdmin(company.CompanyId);
                }
            }

            _context.SaveChanges();
        }

        private void SeedCountry()
        {
            _context.CountryEntities.Add(new CountryEntity
            {
                CCA2 = "IN",
                NumericCode = 356,
                Name = "India",
                IsDeleted = false
            });
            var result = _context.SaveChanges() > 0 ? 1 : 0;
        }

        private void SeedStates()
        {
            var india = _context.CountryEntities.FirstOrDefault(c => c.Name == "India");
            if (india == null) return;

            var statesConfig = _configuration.GetSection("MasterData:States").Get<List<StateData>>();
            if (statesConfig == null) return;

            foreach (var stateConfig in statesConfig)
            {
                _context.state.Add(new StateEntity
                {
                    StateName = stateConfig.Name,
                    CountryId = india.CountryId,
                    IsDeleted = false
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedCities()
        {
            var statesConfig = _configuration.GetSection("MasterData:States").Get<List<StateData>>();
            if (statesConfig == null) return;

            foreach (var stateConfig in statesConfig)
            {
                var state = _context.state.FirstOrDefault(s => s.StateName == stateConfig.Name);
                if (state == null) continue;

                foreach (var cityName in stateConfig.Cities)
                {
                    _context.cities.Add(new CityEntity
                    {
                        CityName = cityName,
                        StateId = state.StateId,
                        IsDeleted = false
                    });
                    var result = _context.SaveChanges() > 0 ? 1 : 0;
                }
            }
        }

        private void SeedCompany()
        {
            // Get company settings from config
            var companyName = _configuration["CompanySettings:CompanyName"] ?? "Your Company Name";
            var companyEmail = _configuration["CompanySettings:CompanyEmail"] ?? "admin@company.com";
            var companyPhone = _configuration["CompanySettings:CompanyPhoneNumber"] ?? "+919876543210";
            var industry = _configuration["CompanySettings:Industry"] ?? "Information Technology";
            var contactFirstName = _configuration["CompanySettings:ContactPersonFirstName"] ?? "Super";
            var contactLastName = _configuration["CompanySettings:ContactPersonLastName"] ?? "Admin";
            var contactEmail = _configuration["CompanySettings:ContactPersonEmail"] ?? "superadmin@company.com";
            var contactPhone = _configuration["CompanySettings:ContactPersonPhoneNumber"] ?? "+919876543210";
            var address1 = _configuration["CompanySettings:PhysicalAddress1"] ?? "Main Street";
            var address2 = _configuration["CompanySettings:PhysicalAddress2"] ?? "Downtown";
            var cityName = _configuration["CompanySettings:City"] ?? "Chennai";
            var stateName = _configuration["CompanySettings:State"] ?? "Tamil Nadu";
            var zipCodeStr = _configuration["CompanySettings:ZipCode"] ?? "600001";
            var countryCode = _configuration["CompanySettings:CompanyCountryCode"] ?? "+91";

            // Get company setting values from config
            var timeZone = _configuration["CompanySettings:TimeZone"] ?? "India Standard Time";
            var currency = _configuration["CompanySettings:Currency"] ?? "INR";
            var language = _configuration["CompanySettings:Language"] ?? "en-US";
            var gstNumber = _configuration["CompanySettings:GSTNumber"] ?? "";
            var companyCode = _configuration["CompanySettings:CompanyCode"] ?? "COMP";
            var isTimeLockEnable = bool.Parse(_configuration["CompanySettings:IsTimeLockEnable"] ?? "false");
            var modeId = int.Parse(_configuration["CompanySettings:ModeId"] ?? "1");

            // Get location IDs
            var india = _context.CountryEntities.FirstOrDefault(c => c.Name == "India");
            var state = _context.state.FirstOrDefault(s => s.StateName == stateName);
            var city = _context.cities.FirstOrDefault(c => c.CityName == cityName);

            if (india == null || state == null || city == null)
            {
                throw new Exception("Required location data not found for company creation");
            }

            int zipCode;
            if (!int.TryParse(zipCodeStr, out zipCode))
            {
                zipCode = 638056;
            }

            // Create Company Entity
            var company = new CompanyEntity
            {
                CompanyName = companyName,
                CompanyEmail = companyEmail,
                CompanyPhoneNumber = companyPhone,
                Industry = industry,
                ContactPersonFirstName = contactFirstName,
                ContactPersonLastName = contactLastName,
                ContactPersonGender = 1, // Male
                ContactPersonEmail = contactEmail,
                ContactPersonPhoneNumber = contactPhone,
                PhysicalAddress1 = address1,
                PhysicalAddress2 = address2,
                PhysicalAddressCity = city.CityId,
                PhysicalAddressState = state.StateId,
                PhysicalAddressZipCode = zipCode,
                PhysicalCountryId = india.CountryId,
                CompanyCountryCode = countryCode,
                IsDeleted = false,
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = 0 // System
            };

            _context.Company.Add(company);
            _context.SaveChanges(); // Save to get CompanyId

            // Now create CompanySettingEntity using the generated CompanyId
            var companySetting = new CompanySettingEntity
            {
                CompanyId = company.CompanyId, // Use the ID from the saved company
                ModeId = modeId,
                TimeZone = timeZone,
                Currency = currency,
                Language = language,
                GSTNumber = gstNumber,
                IsDeleted = false,
                IsTimeLockEnable = isTimeLockEnable,
                CompanyCode = companyCode
            };

            _context.CompanySetting.Add(companySetting);
            _context.SaveChanges();
        }

        private void SeedDepartments(int companyId)
        {
            var departments = _configuration.GetSection("MasterData:Departments").Get<List<string>>();
            if (departments == null) return;

            foreach (var deptName in departments)
            {
                _context.Departments.Add(new DepartmentEntity
                {
                    DepartmentName = deptName,
                    IsDeleted = false,
                    IsActive = true,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedDesignations(int companyId)
        {
            var designations = _configuration.GetSection("MasterData:Designations").Get<List<string>>();
            if (designations == null) return;

            foreach (var designationName in designations)
            {
                _context.Designations.Add(new DesignationEntity
                {
                    DesignationName = designationName,
                    IsDeleted = false,
                    IsActive = true,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedRoles(int companyId)
        {
            var roles = _configuration.GetSection("MasterData:Roles").Get<List<string>>();
            if (roles == null) return;

            foreach (var roleName in roles)
            {
                _context.RoleEntities.Add(new RoleEntity
                {
                    RoleName = roleName,
                    IsActive = true,
                    IsDeleted = false,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedReleaveTypes(int companyId)
        {
            var relievingReasons = new List<string>
            {
                "Terminated",
                "Personal Reason",
                "Medical Issue",
                "Suspended",
                "Absconded",
                "Resigned",
                "Retired",
                "Contract Ended"
            };

            if (relievingReasons == null) return;

            foreach (var roleName in relievingReasons)
            {
                _context.RelievingReasonEntity.Add(new RelievingReasonEntity
                {
                    RelievingReasonName = roleName,
                    IsActive = true,
                    IsDeleted = false,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedDocumentTypes(int companyId)
        {
            var documentTypes = _configuration.GetSection("MasterData:DocumentTypes").Get<List<string>>();
            if (documentTypes == null) return;

            foreach (var documentTypeName in documentTypes)
            {
                _context.documentTypesEntities.Add(new DocumentTypesEntity
                {
                    DocumentName = documentTypeName,
                    IsActive = true,
                    IsDeleted = false,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedSkillSets(int companyId)
        {
            var skills = new List<string>
            {
                "Patient Care","Clinical Assessment","Vital Signs Monitoring","Medication Administration","Phlebotomy","Wound Care","IV Cannulation","Emergency Care",
                "Basic Life Support (BLS)","Advanced Cardiac Life Support (ACLS)","First Aid","Infection Control","Medical Documentation","Electronic Health Records (EHR)",
                "Diagnostic Testing","Laboratory Procedures","Radiology Assistance","Surgical Assistance","Preoperative Care","Postoperative Care","Pain Management",
                "Patient Counseling","Health Education","Clinical Research","Medical Coding","Medical Billing","Telemedicine Support","Critical Care",
                "Intensive Care Unit (ICU) Care",
                "Neonatal Care",
                "Geriatric Care",
                "Pediatric Care",
                "Mental Health Support",
                "Rehabilitation Therapy",
                "Physiotherapy Assistance",
                "Occupational Therapy Support",
                "Nutrition Assessment",
                "Pharmacology Knowledge",
                "Anesthesia Support",
                "Sterilization Techniques",
                "Blood Transfusion Procedures"
            };

            foreach (var skillName in skills)
            {
                _context.SkillSets.Add(new SkillSetEntity
                {
                    SkillName = skillName,
                    IsActive = true,
                    IsDeleted = false,
                    CompanyId = companyId
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedBloodGroups()
        {
            var bloodGroups = new List<string> { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            foreach (var bloodGroup in bloodGroups)
            {
                _context.BloodGroup.Add(new BloodGroupEntity
                {
                    BloodGroupName = bloodGroup
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedRelationshipTypes()
        {
            var relationships = new List<string>
            {
                "Father", "Mother", "Spouse", "Son", "Daughter",
                "Brother", "Sister", "Other"
            };

            foreach (var relationship in relationships)
            {
                _context.Relationship.Add(new RelationshipTypeEntity
                {
                    RelationshipName = relationship
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
        }

        private void SeedSuperAdmin(int companyId)
        {
            try
            {
                var superAdminSettings = _configuration.GetSection("SuperAdmin").Get<SuperAdminSettings>();
                if (superAdminSettings == null) return;

                // Get required references
                var adminDepartment = _context.Departments
                    .FirstOrDefault(d => d.DepartmentName == "Administration");
                var superAdminDesignation = _context.Designations
                    .FirstOrDefault(d => d.DesignationName == "Super Admin");
                var superAdminRole = _context.RoleEntities
                    .FirstOrDefault(r => r.RoleName == "Admin");

                if (adminDepartment == null || superAdminDesignation == null || superAdminRole == null)
                {
                    throw new Exception("Required master data not found for creating super admin");
                }

                _context.Employees.Add(new EmployeesEntity
                {
                    UserName = superAdminSettings.Username,
                    CompanyId = companyId,
                    Password = sha256_hash(superAdminSettings.DefaultPassword),
                    FirstName = superAdminSettings.FirstName,
                    LastName = superAdminSettings.LastName,
                    OfficeEmail = superAdminSettings.Email,
                    DesignationId = superAdminDesignation.DesignationId,
                    DepartmentId = adminDepartment.DepartmentId,
                    IsActive = true,
                    IsProbationary = false,
                    CreatedBy = 0, // System
                    RoleId = superAdminRole.RoleId,
                    IsDeleted = false,
                    IsVerified = true,
                    IsOnboarding = true,
                    CreatedDate = DateTime.Now                    
                });
                var result = _context.SaveChanges() > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {

            }

        }

        public static string sha256_hash(string value)
        {
            StringBuilder sb = new StringBuilder();
            using SHA256 hash = SHA256.Create();
            {
                Encoding encoder = Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoder.GetBytes(value));
                foreach (byte b in result)
                    sb.Append(b.ToString("X2"));
            }
            return Convert.ToString(sb);
        }
    }
}