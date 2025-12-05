using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmployeeInformations.CoreModels.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "AllLeaveDetails",
                schema: "public",
                columns: table => new
                {
                    AllLeaveDetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveYear = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    CasualLeaveCount = table.Column<decimal>(type: "numeric", nullable: false),
                    SickLeaveCount = table.Column<decimal>(type: "numeric", nullable: false),
                    EarnedLeaveCount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaternityLeaveCount = table.Column<decimal>(type: "numeric", nullable: false),
                    CompensatoryOffCount = table.Column<decimal>(type: "numeric", nullable: false),
                    CasualLeaveTaken = table.Column<decimal>(type: "numeric", nullable: false),
                    SickLeaveTaken = table.Column<decimal>(type: "numeric", nullable: false),
                    EarnedLeaveTaken = table.Column<decimal>(type: "numeric", nullable: false),
                    MaternityLeaveTaken = table.Column<decimal>(type: "numeric", nullable: false),
                    CompensatoryOffTaken = table.Column<decimal>(type: "numeric", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllLeaveDetails", x => x.AllLeaveDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                schema: "public",
                columns: table => new
                {
                    AnnouncementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnouncementName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AnnouncementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AnnouncementEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    EmpId = table.Column<string>(type: "text", nullable: true),
                    DesignationId = table.Column<string>(type: "text", nullable: true),
                    DepartmentId = table.Column<string>(type: "text", nullable: true),
                    AnnouncementAssignee = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.AnnouncementId);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementAttachments",
                schema: "public",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnouncementId = table.Column<int>(type: "integer", nullable: false),
                    AttachmentName = table.Column<string>(type: "text", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementAttachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "announcementFilter",
                schema: "public",
                columns: table => new
                {
                    AnnouncementId = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementAssignee = table.Column<int>(type: "integer", nullable: true),
                    AnnouncementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AnnouncementEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    AttachmentName = table.Column<string>(type: "text", nullable: true),
                    Assignee = table.Column<string>(type: "text", nullable: true),
                    ActiveStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "announcementFilterCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_announcementFilterCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLog",
                schema: "public",
                columns: table => new
                {
                    ApplicationLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Host = table.Column<string>(type: "text", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true),
                    ExecptionMessage = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLog", x => x.ApplicationLogId);
                });

            migrationBuilder.CreateTable(
                name: "AssetBrandType",
                schema: "public",
                columns: table => new
                {
                    BrandTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BrandType = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetBrandType", x => x.BrandTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AssetCategory",
                schema: "public",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CategoryCode = table.Column<string>(type: "text", nullable: false),
                    AssetCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "AssetCounts",
                schema: "public",
                columns: table => new
                {
                    AssetCountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCounts", x => x.AssetCountId);
                });

            migrationBuilder.CreateTable(
                name: "AssetLog",
                schema: "public",
                columns: table => new
                {
                    AssetLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    AssetNo = table.Column<string>(type: "text", nullable: false),
                    AssetType = table.Column<int>(type: "integer", nullable: false),
                    AssetName = table.Column<string>(type: "text", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLog", x => x.AssetLogId);
                });

            migrationBuilder.CreateTable(
                name: "AssetLogReportDataModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetCode = table.Column<string>(type: "text", nullable: false),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: true),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetLogReportDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetModels",
                schema: "public",
                columns: table => new
                {
                    AllAssetsId = table.Column<int>(type: "integer", nullable: false),
                    AssetName = table.Column<string>(type: "text", nullable: true),
                    AssetCode = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    CategoryName = table.Column<string>(type: "text", nullable: true),
                    BrandType = table.Column<string>(type: "text", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    BranchLocationName = table.Column<string>(type: "text", nullable: true),
                    StatusName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "public",
                columns: table => new
                {
                    AllAssetsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    BrandId = table.Column<int>(type: "integer", nullable: false),
                    AssetName = table.Column<string>(type: "text", nullable: false),
                    AssetCode = table.Column<string>(type: "text", nullable: false),
                    ProductNumber = table.Column<string>(type: "text", nullable: true),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    PurchaseNumber = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WarrantyStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    WarrantyEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AssetStatusId = table.Column<int>(type: "integer", nullable: true),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PurchaseOrder = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    VendorName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AllAssetsId);
                });

            migrationBuilder.CreateTable(
                name: "AssetsLogModels",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    AssetCode = table.Column<string>(type: "text", nullable: true),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AuthorName = table.Column<string>(type: "text", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetsLogModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetStatus",
                schema: "public",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceListDataModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    TotalHours = table.Column<string>(type: "text", nullable: false),
                    InsideOffice = table.Column<string>(type: "text", nullable: false),
                    BreakHours = table.Column<string>(type: "text", nullable: false),
                    BurningHours = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<string>(type: "text", nullable: false),
                    EndDate = table.Column<string>(type: "text", nullable: false),
                    EntryTime = table.Column<string>(type: "text", nullable: false),
                    ExitTime = table.Column<string>(type: "text", nullable: false),
                    TotalSecounds = table.Column<long>(type: "bigint", nullable: false),
                    OfficeEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceListDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceLogs",
                schema: "public",
                columns: table => new
                {
                    Sno = table.Column<int>(name: "S.no", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeCode = table.Column<string>(type: "text", nullable: true),
                    LogDateTime = table.Column<string>(type: "text", nullable: true),
                    LogDate = table.Column<string>(type: "text", nullable: true),
                    LogTime = table.Column<string>(type: "text", nullable: true),
                    Direction = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLogs", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceReportDateModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<string>(type: "text", nullable: false),
                    LogDateTime = table.Column<string>(type: "text", nullable: false),
                    Direction = table.Column<string>(type: "text", nullable: false),
                    LogDate = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReportDateModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "benefitFilterCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefitFilterCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "benefitFilterViewModel",
                schema: "public",
                columns: table => new
                {
                    BenefitId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    BenefitTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    BenefitName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "BenefitTypes",
                schema: "public",
                columns: table => new
                {
                    BenefitTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BenefitName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitTypes", x => x.BenefitTypeId);
                });

            migrationBuilder.CreateTable(
                name: "BloodGroup",
                schema: "public",
                columns: table => new
                {
                    BloodGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BloodGroupName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodGroup", x => x.BloodGroupId);
                });

            migrationBuilder.CreateTable(
                name: "BranchLocation",
                schema: "public",
                columns: table => new
                {
                    BranchLocationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchLocationName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLocation", x => x.BranchLocationId);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "public",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    CityName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "ClientDetails",
                schema: "public",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientName = table.Column<string>(type: "text", nullable: false),
                    ClientCompany = table.Column<string>(type: "text", nullable: false),
                    ClientDetails = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDetails", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "clientFilterCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientFilterCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "clientFilterViewModel",
                schema: "public",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: true),
                    ClientCompany = table.Column<string>(type: "text", nullable: true),
                    ClientDetails = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "public",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    CompanyEmail = table.Column<string>(type: "text", nullable: false),
                    CompanyPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Industry = table.Column<string>(type: "text", nullable: false),
                    ContactPersonFirstName = table.Column<string>(type: "text", nullable: false),
                    ContactPersonLastName = table.Column<string>(type: "text", nullable: false),
                    ContactPersonGender = table.Column<int>(type: "integer", nullable: false),
                    ContactPersonEmail = table.Column<string>(type: "text", nullable: false),
                    ContactPersonPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    PhysicalAddress1 = table.Column<string>(type: "text", nullable: false),
                    PhysicalAddress2 = table.Column<string>(type: "text", nullable: false),
                    PhysicalAddressCity = table.Column<int>(type: "integer", nullable: false),
                    PhysicalAddressState = table.Column<int>(type: "integer", nullable: false),
                    PhysicalAddressZipCode = table.Column<int>(type: "integer", nullable: false),
                    MailingAddress1 = table.Column<string>(type: "text", nullable: true),
                    MailingAddress2 = table.Column<string>(type: "text", nullable: true),
                    MailingAddressCity = table.Column<int>(type: "integer", nullable: true),
                    MailingAddressState = table.Column<int>(type: "integer", nullable: true),
                    MailingAddressZipCode = table.Column<int>(type: "integer", nullable: true),
                    CompanyLogo = table.Column<string>(type: "text", nullable: true),
                    CompanyFilePath = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    CompanyCountryCode = table.Column<string>(type: "text", nullable: true),
                    PhysicalCountryId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MailingCountryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "CompanySetting",
                schema: "public",
                columns: table => new
                {
                    CompanySettingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    ModeId = table.Column<int>(type: "integer", nullable: false),
                    TimeZone = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    GSTNumber = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsTimeLockEnable = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySetting", x => x.CompanySettingId);
                });

            migrationBuilder.CreateTable(
                name: "companyViewModels",
                schema: "public",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    CompanyPhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CompanyEmail = table.Column<string>(type: "text", nullable: false),
                    Industry = table.Column<string>(type: "text", nullable: false),
                    ContactPersonName = table.Column<string>(type: "text", nullable: false),
                    ContactPersonEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CompensatoryRequests",
                schema: "public",
                columns: table => new
                {
                    CompensatoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    WorkedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    DayCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompensatoryRequests", x => x.CompensatoryId);
                });

            migrationBuilder.CreateTable(
                name: "CompensatoryRequestsCountModel",
                schema: "public",
                columns: table => new
                {
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CompnayCounts",
                schema: "public",
                columns: table => new
                {
                    CompnayCountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "public",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CCA2 = table.Column<string>(type: "text", nullable: false),
                    NumericCode = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DashboardMenus",
                schema: "public",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardMenus", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "DashboardRolePrivileges",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<byte>(type: "smallint", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardRolePrivileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "public",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                schema: "public",
                columns: table => new
                {
                    DesignationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DesignationName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                schema: "public",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmailDraft",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DraftType = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StatusName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmailDraftContent",
                schema: "public",
                columns: table => new
                {
                    EmailDraftTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    DraftBody = table.Column<string>(type: "text", nullable: false),
                    DraftVariable = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailDraftContent", x => x.EmailDraftTypeId);
                });

            migrationBuilder.CreateTable(
                name: "EmailDraftCount",
                schema: "public",
                columns: table => new
                {
                    EmailCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmailDraftType",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DraftType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailDraftType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueue",
                schema: "public",
                columns: table => new
                {
                    EmailQueueID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromEmail = table.Column<string>(type: "text", nullable: false),
                    ToEmail = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsSend = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Attachments = table.Column<string>(type: "text", nullable: true),
                    CCEmail = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueue", x => x.EmailQueueID);
                });

            migrationBuilder.CreateTable(
                name: "emailSchedulerFilterCounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailSchedulerFilterCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "emailSchedulerViewModels",
                schema: "public",
                columns: table => new
                {
                    SchedulerId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    DurationId = table.Column<int>(type: "integer", nullable: false),
                    Durations = table.Column<string>(type: "text", nullable: false),
                    ReportName = table.Column<string>(type: "text", nullable: false),
                    FileFormat = table.Column<int>(type: "integer", nullable: false),
                    FileFormatStatus = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsActiveStatus = table.Column<string>(type: "text", nullable: true),
                    MailTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                schema: "public",
                columns: table => new
                {
                    EmailSettingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromEmail = table.Column<string>(type: "text", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    EmailPort = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.EmailSettingId);
                });

            migrationBuilder.CreateTable(
                name: "employeeActivityFilterCounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeActivityFilterCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeActivityLog",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeActivityLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "employeeActivityLogViewModels",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    companyName = table.Column<string>(type: "text", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAppliedLeave",
                schema: "public",
                columns: table => new
                {
                    AppliedLeaveId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppliedLeaveTypeId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    LeaveApplied = table.Column<decimal>(type: "numeric", nullable: false),
                    LeaveFromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LeaveName = table.Column<string>(type: "text", nullable: true),
                    LeaveFilePath = table.Column<string>(type: "text", nullable: true),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAppliedLeave", x => x.AppliedLeaveId);
                });

            migrationBuilder.CreateTable(
                name: "employeeAppliedLeavesEntities",
                schema: "public",
                columns: table => new
                {
                    AppliedLeaveId = table.Column<int>(type: "integer", nullable: false),
                    AppliedLeaveTypeId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeUserName = table.Column<string>(type: "text", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    TotalLeave = table.Column<decimal>(type: "numeric", nullable: false),
                    AppliedLeave = table.Column<decimal>(type: "numeric", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    LeaveTypes = table.Column<string>(type: "text", nullable: false),
                    LeaveFromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveFilePath = table.Column<string>(type: "text", nullable: true),
                    LeaveName = table.Column<string>(type: "text", nullable: true),
                    EmployeeProfileImage = table.Column<string>(type: "text", nullable: true),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBenefit",
                schema: "public",
                columns: table => new
                {
                    BenefitId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    BenefitTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBenefit", x => x.BenefitId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCompensatoryFilterModel",
                schema: "public",
                columns: table => new
                {
                    CompensatoryId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeUserName = table.Column<string>(type: "text", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    WorkedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    DayCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    EmployeeProfileImage = table.Column<string>(type: "text", nullable: true),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "employeeExpensesEntities",
                schema: "public",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ExpenseTitle = table.Column<string>(type: "text", nullable: true),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    EmployeeUserName = table.Column<string>(type: "text", nullable: false),
                    EmployeeProfileImage = table.Column<string>(type: "text", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeHolidays",
                schema: "public",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHolidays", x => x.HolidayId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaveReportCounts",
                schema: "public",
                columns: table => new
                {
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLeaveReportDataModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeUserId = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    LeaveCount = table.Column<decimal>(type: "numeric", nullable: true),
                    LeaveType = table.Column<string>(type: "text", nullable: true),
                    LeaveFromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    AppliedLeaveTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeMedicalBenefit",
                schema: "public",
                columns: table => new
                {
                    MedicalBenefitId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    BenefitTypeId = table.Column<int>(type: "integer", nullable: false),
                    Scheme = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Member = table.Column<string>(type: "text", nullable: false),
                    MembershipNumber = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMedicalBenefit", x => x.MedicalBenefitId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePrivileges",
                schema: "public",
                columns: table => new
                {
                    PrivilegeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    IsEarnLeave = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePrivileges", x => x.PrivilegeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePrivilegesCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePrivilegesCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeReleavingType",
                schema: "public",
                columns: table => new
                {
                    RelieveId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReleavingType = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReleavingType", x => x.RelieveId);
                });

            migrationBuilder.CreateTable(
                name: "employeesCounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeesCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesDataCounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesDataCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesDataModels",
                schema: "public",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    OfficeEmail = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeesDetailsDataModels",
                schema: "public",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    FatherName = table.Column<string>(type: "text", nullable: true),
                    OfficeEmail = table.Column<string>(type: "text", nullable: true),
                    PersonalEmail = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true),
                    IsOnboarding = table.Column<bool>(type: "boolean", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: true),
                    IsProbationary = table.Column<bool>(type: "boolean", nullable: true),
                    RoleId = table.Column<byte>(type: "smallint", nullable: true),
                    DepartmentId = table.Column<int>(type: "integer", nullable: true),
                    DesignationId = table.Column<int>(type: "integer", nullable: true),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    RelieveId = table.Column<int>(type: "integer", nullable: true),
                    EsslId = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    EmployeeProfileImage = table.Column<string>(type: "text", nullable: true),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateOfRelieving = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DesignationName = table.Column<string>(type: "text", nullable: true),
                    DepartmentName = table.Column<string>(type: "text", nullable: true),
                    ReleaveName = table.Column<string>(type: "text", nullable: true),
                    AllAssetId = table.Column<int>(type: "integer", nullable: true),
                    BenefitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSetting",
                schema: "public",
                columns: table => new
                {
                    EmployeeSettingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    ProbationMonths = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSetting", x => x.EmployeeSettingId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesLog",
                schema: "public",
                columns: table => new
                {
                    EmployeesLogId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    SectionName = table.Column<string>(type: "text", nullable: true),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesLog", x => x.EmployeesLogId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesLogReport",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: true),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    AuthorName = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesLogReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesLogReportDataModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    PreviousValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    Event = table.Column<string>(type: "text", nullable: true),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesLogReportDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesPrivilegesViewModel",
                schema: "public",
                columns: table => new
                {
                    PrivilegeID = table.Column<int>(type: "integer", nullable: false),
                    EmployeesName = table.Column<string>(type: "text", nullable: true),
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    IsEarnLeave = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ExpenseAttachments",
                schema: "public",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    ExpenseName = table.Column<string>(type: "text", nullable: true),
                    Document = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseAttachments", x => x.ExpenseId);
                });

            migrationBuilder.CreateTable(
                name: "ExperienceAttachments",
                schema: "public",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ExperienceId = table.Column<int>(type: "integer", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: false),
                    ExperienceName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceAttachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "HelpDeskCount",
                schema: "public",
                columns: table => new
                {
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "HelpdeskDetails",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpdeskDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpDeskFilterEntity",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    TicketType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TicketStatus = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    AttachmentName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LeaveReportDateModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeUserId = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    LeaveCount = table.Column<decimal>(type: "numeric", nullable: false),
                    LeaveType = table.Column<string>(type: "text", nullable: false),
                    LeaveFromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeaveToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    AppliedLeaveTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveReportDateModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeaveTypeId = table.Column<int>(type: "integer", nullable: false),
                    LeaveType = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailScheduler",
                schema: "public",
                columns: table => new
                {
                    SchedulerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    FileFormat = table.Column<int>(type: "integer", nullable: false),
                    ReportName = table.Column<string>(type: "text", nullable: false),
                    WhomToSend = table.Column<string>(type: "text", nullable: false),
                    MailSendingDays = table.Column<string>(type: "text", nullable: false),
                    DurationId = table.Column<int>(type: "integer", nullable: false),
                    MailTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmailDraftId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailScheduler", x => x.SchedulerId);
                });

            migrationBuilder.CreateTable(
                name: "ManualLog",
                schema: "public",
                columns: table => new
                {
                    Sno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EntryStatus = table.Column<string>(type: "text", nullable: false),
                    TotalHours = table.Column<string>(type: "text", nullable: false),
                    BreakHours = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualLog", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "ManualLogReportDateModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EntryStatus = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    EmployeeUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualLogReportDateModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medicalBenefitFilterCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicalBenefitFilterCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "medicalBenefitFilterViewModel",
                schema: "public",
                columns: table => new
                {
                    MedicalBenefitId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    BenefitTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: false),
                    Isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    BenefitName = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Member = table.Column<string>(type: "text", nullable: true),
                    MembershipNumber = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Scheme = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherDetailsAttachments",
                schema: "public",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: false),
                    DocumentName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDetailsAttachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "PolicyAttachments",
                schema: "public",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PolicyId = table.Column<int>(type: "integer", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: false),
                    AttachmentName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyAttachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAssignation",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssignation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                schema: "public",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: false),
                    ProjectDescription = table.Column<string>(type: "text", nullable: false),
                    Technology = table.Column<string>(type: "text", nullable: true),
                    Hours = table.Column<int>(type: "integer", nullable: true),
                    Startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProjectCost = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ProjectRefNumber = table.Column<string>(type: "text", nullable: false),
                    ClientCompanyId = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "projectDetailsEntities",
                schema: "public",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: false),
                    ProjectDescription = table.Column<string>(type: "text", nullable: false),
                    Technology = table.Column<string>(type: "text", nullable: false),
                    Hours = table.Column<int>(type: "integer", nullable: true),
                    Startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProjectCost = table.Column<decimal>(type: "numeric", nullable: true),
                    ProjectRefNumber = table.Column<string>(type: "text", nullable: false),
                    ClientCompanyId = table.Column<int>(type: "integer", nullable: false),
                    ProjectTypeName = table.Column<string>(type: "text", nullable: false),
                    ClientCompanyName = table.Column<string>(type: "text", nullable: false),
                    TechnologyName = table.Column<string>(type: "text", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                schema: "public",
                columns: table => new
                {
                    ProjectTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectTypeName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.ProjectTypeId);
                });

            migrationBuilder.CreateTable(
                name: "QualificationAttachments",
                schema: "public",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    QualificationId = table.Column<int>(type: "integer", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: true),
                    QualificationName = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationAttachments", x => x.AttachmentId);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipType",
                schema: "public",
                columns: table => new
                {
                    RelationshipId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RelationshipName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipType", x => x.RelationshipId);
                });

            migrationBuilder.CreateTable(
                name: "RelievingReason",
                schema: "public",
                columns: table => new
                {
                    RelievingReasonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RelievingReasonName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelievingReason", x => x.RelievingReasonId);
                });

            migrationBuilder.CreateTable(
                name: "ReportingPersons",
                schema: "public",
                columns: table => new
                {
                    ReportingPersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ReportingPersonEmpId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingPersons", x => x.ReportingPersonId);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivileges",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<byte>(type: "smallint", nullable: false),
                    SubModuleId = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsWritable = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "public",
                columns: table => new
                {
                    RoleId = table.Column<byte>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                schema: "public",
                columns: table => new
                {
                    SalaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.SalaryId);
                });

            migrationBuilder.CreateTable(
                name: "SendEmails",
                schema: "public",
                columns: table => new
                {
                    EmailListId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailSettingId = table.Column<int>(type: "integer", nullable: false),
                    EmailId = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendEmails", x => x.EmailListId);
                });

            migrationBuilder.CreateTable(
                name: "SkillSets",
                schema: "public",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSets", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "public",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateName = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "SubModules",
                schema: "public",
                columns: table => new
                {
                    SubModuleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModules", x => x.SubModuleId);
                });

            migrationBuilder.CreateTable(
                name: "teamMeetingCounts",
                schema: "public",
                columns: table => new
                {
                    TeamCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TeamMeetingModels",
                schema: "public",
                columns: table => new
                {
                    AttendeeEmail = table.Column<string>(type: "text", nullable: true),
                    MeetingName = table.Column<string>(type: "text", nullable: true),
                    TeamsMeetingId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TeamsMeeting",
                schema: "public",
                columns: table => new
                {
                    TeamsMeetingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeetingName = table.Column<string>(type: "text", nullable: false),
                    AttendeeEmail = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    MeetingId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamsMeeting", x => x.TeamsMeetingId);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachments ",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HelpdeskId = table.Column<int>(type: "integer", nullable: false),
                    AttachmentName = table.Column<string>(type: "text", nullable: false),
                    Document = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                schema: "public",
                columns: table => new
                {
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    ReportingPersonId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.TicketTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TimeLoggers",
                schema: "public",
                columns: table => new
                {
                    TimeLoggerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ClockInTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClockOutTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EntryStatus = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LogSeconds = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLoggers", x => x.TimeLoggerId);
                });

            migrationBuilder.CreateTable(
                name: "Timesheet",
                schema: "public",
                columns: table => new
                {
                    TimeSheetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    TaskDescription = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "text", nullable: true),
                    AttachmentFilePath = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheet", x => x.TimeSheetId);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheetDataModel",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    TaskDescription = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "text", nullable: true),
                    AttachmentFilePath = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    EmployeeUserId = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheetDataModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheetModels",
                schema: "public",
                columns: table => new
                {
                    TimeSheetId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    EmpId = table.Column<int>(type: "integer", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    TaskDescription = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AttachmentFileName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimesheetStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TimeSheetReport",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    Startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    TaskDescription = table.Column<string>(type: "text", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    AttachmentFilePath = table.Column<string>(type: "text", nullable: true),
                    AttachmentFileName = table.Column<string>(type: "text", nullable: true),
                    EmployeeUserId = table.Column<string>(type: "text", nullable: false),
                    EmployeeName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    TimeSheetStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TimeSheetReportCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheetReportCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "websideJobApplyCounts",
                schema: "public",
                columns: table => new
                {
                    JobApplyCountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "websideJobApplyViewModels",
                schema: "public",
                columns: table => new
                {
                    JobApplyId = table.Column<int>(type: "integer", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JobName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: false),
                    SkillSet = table.Column<string>(type: "text", nullable: false),
                    RelevantExperience = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    FileFormat = table.Column<string>(type: "text", nullable: false),
                    IsRecordForm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Website_CandidateMenu",
                schema: "public",
                columns: table => new
                {
                    CandidateMenuId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobApplyId = table.Column<int>(type: "integer", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    SkillSet = table.Column<string>(type: "text", nullable: true),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    IsRecordForm = table.Column<string>(type: "text", nullable: true),
                    CandidateStatusId = table.Column<int>(type: "integer", nullable: true),
                    CandidateScheduleId = table.Column<int>(type: "integer", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    MeetingLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_CandidateMenu", x => x.CandidateMenuId);
                });

            migrationBuilder.CreateTable(
                name: "Website_CandidatePrivileges",
                schema: "public",
                columns: table => new
                {
                    CandidatePrivilegeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidatescheduleId = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_CandidatePrivileges", x => x.CandidatePrivilegeId);
                });

            migrationBuilder.CreateTable(
                name: "Website_CandidateSchedule",
                schema: "public",
                columns: table => new
                {
                    CandidateScheduleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_CandidateSchedule", x => x.CandidateScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Website_ContactUs",
                schema: "public",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactName = table.Column<string>(type: "text", nullable: false),
                    ContactEmail = table.Column<string>(type: "text", nullable: false),
                    ContactPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ContactWebsiteName = table.Column<string>(type: "text", nullable: true),
                    ContactDescription = table.Column<string>(type: "text", nullable: true),
                    ContactLeadTypeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_ContactUs", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "Website_JobApply",
                schema: "public",
                columns: table => new
                {
                    JobApplyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    SkillSet = table.Column<string>(type: "text", nullable: true),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    IsRecordForm = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_JobApply", x => x.JobApplyId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Jobs",
                schema: "public",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobCode = table.Column<string>(type: "text", nullable: false),
                    JobName = table.Column<string>(type: "text", nullable: false),
                    JobDescription = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: true),
                    Qualification = table.Column<string>(type: "text", nullable: false),
                    Experience = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: true),
                    NoOfPositions = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    JobSummary = table.Column<string>(type: "text", nullable: false),
                    JobPostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SkillName = table.Column<string>(type: "text", nullable: false),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false),
                    Createdby = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Updatedby = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "Website_LeadType",
                schema: "public",
                columns: table => new
                {
                    LeadTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeadType = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_LeadType", x => x.LeadTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Proposals",
                schema: "public",
                columns: table => new
                {
                    QuoteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ERFN = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    ServicesId = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProposalTypeId = table.Column<int>(type: "integer", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Proposals", x => x.QuoteId);
                });

            migrationBuilder.CreateTable(
                name: "Website_ProposalTypes",
                schema: "public",
                columns: table => new
                {
                    ProposalTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProposalTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_ProposalTypes", x => x.ProposalTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Services",
                schema: "public",
                columns: table => new
                {
                    ServicesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Services", x => x.ServicesId);
                });

            migrationBuilder.CreateTable(
                name: "Website_SurveyAnswers",
                schema: "public",
                columns: table => new
                {
                    SurveyAnswerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurveyQuestionId = table.Column<int>(type: "integer", nullable: false),
                    SurveyOptionId = table.Column<int>(type: "integer", nullable: false),
                    SurveyAnswer = table.Column<string>(type: "text", nullable: false),
                    QuoteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_SurveyAnswers", x => x.SurveyAnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Website_SurveyQuestion",
                schema: "public",
                columns: table => new
                {
                    SurveyQuestionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurveyQuestionName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_SurveyQuestion", x => x.SurveyQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "Website_SurveyQuestionOptions",
                schema: "public",
                columns: table => new
                {
                    SurveyOptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurveyQuestionId = table.Column<int>(type: "integer", nullable: false),
                    SurveyQuestionOptionName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_SurveyQuestionOptions", x => x.SurveyOptionId);
                });

            migrationBuilder.CreateTable(
                name: "websiteCandidateMenu",
                schema: "public",
                columns: table => new
                {
                    CandidateScheduleId = table.Column<int>(type: "integer", nullable: true),
                    CandidateMenuId = table.Column<int>(type: "integer", nullable: false),
                    JobApplyId = table.Column<int>(type: "integer", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    JobName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: true),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true),
                    SkillSet = table.Column<string>(type: "text", nullable: true),
                    IsRecordForm = table.Column<string>(type: "text", nullable: true),
                    ScheduleName = table.Column<string>(type: "text", nullable: true),
                    CandidateStatusId = table.Column<int>(type: "integer", nullable: true),
                    CandidateStatusName = table.Column<string>(type: "text", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    MeetingLink = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "websiteCandidateMenuModel",
                schema: "public",
                columns: table => new
                {
                    CandidateMenuId = table.Column<int>(type: "integer", nullable: false),
                    JobApplyId = table.Column<int>(type: "integer", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    SkillSet = table.Column<string>(type: "text", nullable: true),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    IsRecordForm = table.Column<string>(type: "text", nullable: true),
                    JobName = table.Column<string>(type: "text", nullable: true),
                    SortFullName = table.Column<string>(type: "text", nullable: true),
                    CandidateStatusId = table.Column<int>(type: "integer", nullable: true),
                    CandidateScheduleId = table.Column<int>(type: "integer", nullable: true),
                    CandidateScheduleName = table.Column<string>(type: "text", nullable: true),
                    CandidateStatusName = table.Column<string>(type: "text", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Employees = table.Column<string>(type: "text", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    MeetingLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "websiteCandidateMenuModelCount",
                schema: "public",
                columns: table => new
                {
                    EmployeeCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WebsiteJobspost",
                schema: "public",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    JobCode = table.Column<string>(type: "text", nullable: true),
                    JobName = table.Column<string>(type: "text", nullable: true),
                    JobDescription = table.Column<string>(type: "text", nullable: true),
                    Designation = table.Column<string>(type: "text", nullable: true),
                    Qualification = table.Column<string>(type: "text", nullable: true),
                    Experience = table.Column<string>(type: "text", nullable: true),
                    JobType = table.Column<string>(type: "text", nullable: true),
                    NoOfPositions = table.Column<int>(type: "integer", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    JobSummary = table.Column<string>(type: "text", nullable: false),
                    JobPostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<int>(type: "integer", nullable: false),
                    Createdby = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Updatedby = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SkillName = table.Column<string>(type: "text", nullable: true),
                    RelevantExperience = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "WebsiteJobspostcount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteJobspostcount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "workFromHomeFilter",
                schema: "public",
                columns: table => new
                {
                    AppliedLeaveTypeId = table.Column<int>(type: "integer", nullable: false),
                    AppliedLeaveId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    LeaveType = table.Column<string>(type: "text", nullable: true),
                    LeaveApplied = table.Column<decimal>(type: "numeric", nullable: false),
                    LeaveFromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LeaveToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    LeaveFilePath = table.Column<string>(type: "text", nullable: true),
                    LeaveName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    EmployeeName = table.Column<string>(type: "text", nullable: true),
                    ProfileFilePath = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LeaveStatus = table.Column<string>(type: "text", nullable: true),
                    EmployeeStatus = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "workFromHomeFilterCount",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workFromHomeFilterCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPolicy",
                schema: "public",
                columns: table => new
                {
                    PolicyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PolicyName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPolicy", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_CompanyPolicy_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "public",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "public",
                columns: table => new
                {
                    EmpId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FatherName = table.Column<string>(type: "text", nullable: true),
                    OfficeEmail = table.Column<string>(type: "text", nullable: false),
                    DesignationId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    PersonalEmail = table.Column<string>(type: "text", nullable: true),
                    SkillName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsProbationary = table.Column<bool>(type: "boolean", nullable: false),
                    ProbationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RoleId = table.Column<byte>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    RelieveId = table.Column<int>(type: "integer", nullable: true),
                    EsslId = table.Column<int>(type: "integer", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    ReleavedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsOnboarding = table.Column<bool>(type: "boolean", nullable: false),
                    UANNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_employees_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "public",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "public",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ExpenseTitle = table.Column<string>(type: "text", nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    IsApproved = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "public",
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AddressInfo",
                schema: "public",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    Address1 = table.Column<string>(type: "text", nullable: false),
                    Address2 = table.Column<string>(type: "text", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Pincode = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SecondaryAddress1 = table.Column<string>(type: "text", nullable: true),
                    SecondaryAddress2 = table.Column<string>(type: "text", nullable: true),
                    SecondaryCityId = table.Column<int>(type: "integer", nullable: true),
                    SecondaryStateId = table.Column<int>(type: "integer", nullable: true),
                    SecondaryCountryId = table.Column<int>(type: "integer", nullable: true),
                    SecondaryPincode = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressInfo", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_AddressInfo_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                schema: "public",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    AccountHolderName = table.Column<string>(type: "text", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: false),
                    IFSCCode = table.Column<string>(type: "text", nullable: false),
                    BranchName = table.Column<string>(type: "text", nullable: false),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_BankDetails_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                schema: "public",
                columns: table => new
                {
                    ExperienceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    PreviousCompany = table.Column<string>(type: "text", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateOfRelieving = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.ExperienceId);
                    table.ForeignKey(
                        name: "FK_Experience_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherDetails",
                schema: "public",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "integer", nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UANNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_OtherDetails_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileInfo",
                schema: "public",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    MaritalStatus = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BloodGroup = table.Column<int>(type: "integer", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    ProfileImage = table.Column<byte[]>(type: "bytea", nullable: false),
                    ProfileName = table.Column<string>(type: "text", nullable: false),
                    ProfileFilePath = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ContactPersonName = table.Column<string>(type: "text", nullable: true),
                    RelationshipId = table.Column<int>(type: "integer", nullable: true),
                    ContactNumber = table.Column<string>(type: "text", nullable: true),
                    CountryCodeNumber = table.Column<string>(type: "text", nullable: true),
                    OthersName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInfo", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_ProfileInfo_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Qualification",
                schema: "public",
                columns: table => new
                {
                    QualificationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    QualificationType = table.Column<string>(type: "text", nullable: false),
                    Percentage = table.Column<string>(type: "text", nullable: false),
                    YearOfPassing = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    InstitutionName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualification", x => x.QualificationId);
                    table.ForeignKey(
                        name: "FK_Qualification_employees_EmpId",
                        column: x => x.EmpId,
                        principalSchema: "public",
                        principalTable: "employees",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseDetails",
                schema: "public",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpenseId = table.Column<int>(type: "integer", nullable: false),
                    EmpId = table.Column<int>(type: "integer", nullable: false),
                    ExpenseCategory = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BillNumber = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpenseName = table.Column<string>(type: "text", nullable: true),
                    Document = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_ExpenseDetails_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalSchema: "public",
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_EmpId",
                schema: "public",
                table: "AddressInfo",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_EmpId",
                schema: "public",
                table: "BankDetails",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPolicy_CompanyId",
                schema: "public",
                table: "CompanyPolicy",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_CompanyId",
                schema: "public",
                table: "employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseDetails_ExpenseId",
                schema: "public",
                table: "ExpenseDetails",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CompanyId",
                schema: "public",
                table: "Expenses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_EmpId",
                schema: "public",
                table: "Experience",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherDetails_EmpId",
                schema: "public",
                table: "OtherDetails",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfo_EmpId",
                schema: "public",
                table: "ProfileInfo",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualification_EmpId",
                schema: "public",
                table: "Qualification",
                column: "EmpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressInfo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AllLeaveDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Announcement",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AnnouncementAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "announcementFilter",
                schema: "public");

            migrationBuilder.DropTable(
                name: "announcementFilterCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ApplicationLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetBrandType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetCategory",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetLogReportDataModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Assets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetsLogModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetStatus",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AssetTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AttendanceListDataModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AttendanceLogs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AttendanceReportDateModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BankDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "benefitFilterCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "benefitFilterViewModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BenefitTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BloodGroup",
                schema: "public");

            migrationBuilder.DropTable(
                name: "BranchLocation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "City",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ClientDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientFilterCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientFilterViewModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CompanyPolicy",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CompanySetting",
                schema: "public");

            migrationBuilder.DropTable(
                name: "companyViewModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CompensatoryRequests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CompensatoryRequestsCountModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CompnayCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "public");

            migrationBuilder.DropTable(
                name: "DashboardMenus",
                schema: "public");

            migrationBuilder.DropTable(
                name: "DashboardRolePrivileges",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Designation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "DocumentTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailDraft",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailDraftContent",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailDraftCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailDraftType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailQueue",
                schema: "public");

            migrationBuilder.DropTable(
                name: "emailSchedulerFilterCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "emailSchedulerViewModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailSettings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employeeActivityFilterCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeActivityLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employeeActivityLogViewModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeAppliedLeave",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employeeAppliedLeavesEntities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeBenefit",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeCompensatoryFilterModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employeeExpensesEntities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeHolidays",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeLeaveReportCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeLeaveReportDataModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeMedicalBenefit",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeePrivileges",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeePrivilegesCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeReleavingType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employeesCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesDataCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesDataModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesDetailsDataModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeeSetting",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesLogReport",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesLogReportDataModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmployeesPrivilegesViewModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExpenseAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExpenseDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Experience",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExperienceAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "HelpDeskCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "HelpdeskDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "HelpDeskFilterEntity",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LeaveReportDateModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "LeaveTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "MailScheduler",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ManualLog",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ManualLogReportDateModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "medicalBenefitFilterCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "medicalBenefitFilterViewModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OtherDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OtherDetailsAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PolicyAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProfileInfo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectAssignation",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "projectDetailsEntities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Qualification",
                schema: "public");

            migrationBuilder.DropTable(
                name: "QualificationAttachments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RelationshipType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RelievingReason",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ReportingPersons",
                schema: "public");

            migrationBuilder.DropTable(
                name: "RolePrivileges",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Salary",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SendEmails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SkillSets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "State",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SubModules",
                schema: "public");

            migrationBuilder.DropTable(
                name: "teamMeetingCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TeamMeetingModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TeamsMeeting",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TicketAttachments ",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TicketTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeLoggers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Timesheet",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeSheetDataModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeSheetModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeSheetReport",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeSheetReportCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "websideJobApplyCounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "websideJobApplyViewModels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_CandidateMenu",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_CandidatePrivileges",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_CandidateSchedule",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_ContactUs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_JobApply",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_Jobs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_LeadType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_Proposals",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_ProposalTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_Services",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_SurveyAnswers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_SurveyQuestion",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Website_SurveyQuestionOptions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "websiteCandidateMenu",
                schema: "public");

            migrationBuilder.DropTable(
                name: "websiteCandidateMenuModel",
                schema: "public");

            migrationBuilder.DropTable(
                name: "websiteCandidateMenuModelCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WebsiteJobspost",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WebsiteJobspostcount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workFromHomeFilter",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workFromHomeFilterCount",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "employees",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "public");
        }
    }
}
