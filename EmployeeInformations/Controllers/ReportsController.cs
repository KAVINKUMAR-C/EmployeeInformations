using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.CompanyPolicyViewModel;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using PageSize = iTextSharp.text.PageSize;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using Rectangle = iTextSharp.text.Rectangle;

namespace EmployeeInformations.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IEmployeesService _employeesService;
        private readonly ILeaveService _leaveService;
        private readonly IReportService _reportService;
        private readonly ICompanyPolicyService _companyPolicyService;
        private readonly ITimeSheetService _timeSheetService;
        private readonly IProjectDetailsService _projectDetailsService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ReportsController(IEmployeesService employeesService, ILeaveService leaveService, IReportService reportService, ICompanyPolicyService companyPolicyService, ITimeSheetService timeSheetService, IProjectDetailsService projectDetailsService, IHostingEnvironment hostingEnvironment)
        {
            _employeesService = employeesService;
            _leaveService = leaveService;
            _reportService = reportService;
            _companyPolicyService = companyPolicyService;
            _timeSheetService = timeSheetService;
            _projectDetailsService = projectDetailsService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Leave Report

        /// <summary>
        /// Logic to get all the leavereports list
        /// </summary>
        ///  /// <param name="pager" ></param>
        [HttpGet]
        public async Task<IActionResult> LeaveReports(SysDataTablePager pager)
        {
            var companyId = GetSessionValueForCompanyId;
            var listOfReports = new Reports();
            var employeeDropdown = await _reportService.GetAllEmployeesDrropdown(companyId);
            var leaveTypeDropdown = await _reportService.GetAllLeave();
            listOfReports.reportingPeople = employeeDropdown;
            listOfReports.LeaveTypes = leaveTypeDropdown;
       
            return View(listOfReports);
        }
        [HttpGet]
        /// <summary>
        /// Logic to get all the leavereports list
        /// </summary>
        /// <param name="reports,columnName,columnDirection,pager" ></param>
        public async Task<IActionResult>LeaveReportByPager(SysDataTablePager pager, string columnDirection, string columnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var listOfReports = new Reports();
            var employeeDropdown = await _reportService.GetAllEmployeesDrropdown(companyId);
            var leaveTypeDropdown = await _reportService.GetAllLeave();
            listOfReports.reportingPeople = employeeDropdown;
            listOfReports.LeaveTypes = leaveTypeDropdown;            
            listOfReports.employeeAppliedLeaves = await _reportService.GetAllEmployeeLeaveReports(pager, columnDirection, columnName);
            var leaveCount = await _reportService.GetAllEmployessCount(pager);
            return Json(new
            {
                iTotalRecords = leaveCount,
                iTotalDisplayRecords = leaveCount,
                data = listOfReports.employeeAppliedLeaves
            }); 
        }
 


        /// <summary>
        /// Logic to get all employees remaining leave report list
        /// </summary>
        public async Task<IActionResult> LeaveRemainingReport()
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
			var sessionRoleId = GetSessionValueForRoleId;		
			var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
			var leave = new EmployeeLeaveViewModel();					
		    var leaveSummary = await _leaveService.GetAllRemainingLeave(empId,companyId);
			var employeeDropdown = await _reportService.GetAllEmployeesDrropdown(companyId);
			leaveSummary.reportingPeople = employeeDropdown;
			leave.LeaveCounts = leaveSummary.LeaveCounts;           
			leaveSummary.EmployeeId = empId == 0 ? 0 : sessionEmployeeId;
			return View(leaveSummary);
        }


        [HttpPost]
        public  async Task<IActionResult> LeaveRemainingReportByEmpId(int empId)
        {
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetAllRemainingLeave(empId,companyId);
			return new JsonResult(leaveSummary);
		}


        /// <summary>
        /// Logic to get filter the employee leave report detail  by fromDate to toDate
        /// </summary>
        /// <param name="reports,columnName,columnDirection,pager" ></param>
        [HttpPost]
        public async Task<IActionResult> FilterEmployeeByLeaveType(SysDataTablePager pager,Reports reports, string columnDirection, string columnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var listOfReports = new Reports();
           listOfReports.employeeAppliedLeaves = await _reportService.GetAllEmployessByLeaveTypeFilterData(pager, reports,columnDirection,columnName, companyId);
           var employeeLeaveCount = await _reportService.GetAllEmployessByLeaveTypeFilterDataCount(pager, reports);
            return Json(new
            {
                iTotalRecords = employeeLeaveCount,
                iTotalDisplayRecords = employeeLeaveCount,
                data = listOfReports.employeeAppliedLeaves
            });
          

        }
        
        /// <summary>
        /// Logic to get download the employeeleavereport detail  by particular employeereport
        /// </summary>
        /// <param name="reports" ></param>
        public async Task<string> DownloadExcelLeave(Reports reports)
        {
            var companyId = GetSessionValueForCompanyId;
            var fileId = "";
            var leaves = await _reportService.GetAllEmployessByLeaveTypeFilter(reports, companyId);
            
          
            if (leaves.Count() > 0)
            {
                
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employee Leave Details");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                worksheet.Cell(currentRow, 3).Value = Constant.LeaveType;
                worksheet.Cell(currentRow, 4).Value = Constant.LeaveFromDate;
                worksheet.Cell(currentRow, 5).Value = Constant.LeaveToDate;
                worksheet.Cell(currentRow, 6).Value = Constant.Reason;
                worksheet.Cell(currentRow, 7).Value = Constant.LeaveCount;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 7).Style.Font.Bold = true;

                foreach (var user in leaves)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat("yyyy/MM/dd hh:mm:ss AM/PM");
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat("yyyy/MM/dd hh:mm:ss AM/PM");
                    worksheet.Cell(currentRow, 1).Value = user.EmployeeUserId;
                    worksheet.Cell(currentRow, 2).Value = user.FirstName + " " + user.LastName;
                    worksheet.Cell(currentRow, 3).Value = user.LeaveType;
                    worksheet.Cell(currentRow, 4).Value = user.LeaveFromDate;
                    worksheet.Cell(currentRow, 5).Value = user.LeaveToDate;
                    worksheet.Cell(currentRow, 6).Value = user.Reason;
                    worksheet.Cell(currentRow, 7).Value = user.LeaveCount;

                }
                var fileName = string.Format("EmployeeLeaveDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                fileId = Guid.NewGuid().ToString() + "_" + fileName;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    var content = memoryStream.ToArray();
                    HttpContext.Session.Set(Constant.fileId, content);
                }
            }
            return fileId;
        }

        /// <summary>
        /// Logic to get download the employeeleavereport detail  by particular employeereport
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult Download(string fileGuid)
        {
            var fileName = string.Format("EmployeeLeaveDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                //HttpContext.Session.Set(Constant.fileId,null);
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Logic to Create Pdf
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpPost]
        public async Task<string> CreatePdf(Reports reports)
        {
            var companyId = GetSessionValueForCompanyId;
            var leaves = await _reportService.GetAllEmployessByLeaveTypeFilter(reports, companyId);

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

            // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
            headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
            string webRootPath = _hostingEnvironment.WebRootPath;
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
            logo.ScaleToFit(100, 70);

            {
                PdfPCell pdfCelllogo = new PdfPCell(logo);
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCelllogo.BorderWidthBottom = 1f;
                pdfCelllogo.PaddingTop = 10f;
                pdfCelllogo.PaddingBottom = 10f;
                headertable.AddCell(pdfCelllogo);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middlecell.BorderWidthBottom = 1f;
                headertable.AddCell(middlecell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Leave Report - " + reports.LeaveFromDate + " " + "To" + " " + reports.LeaveToDate, titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.PaddingBottom = 20f;
                nested.AddCell(nextPostCell1);

                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 6;
                nesthousing.PaddingTop = 10f;
                headertable.AddCell(nesthousing);
            }

            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                Invoicetable.AddCell(middlecell);
            }

            PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
            emptyCell.Border = Rectangle.NO_BORDER;
            Invoicetable.AddCell(emptyCell);

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                middlecell.PaddingTop = 20f;
                Invoicetable.AddCell(middlecell);
            }

            pdfDoc.Add(headertable);
            pdfDoc.Add(Invoicetable);

            //Create body table
            PdfPTable itemTable = new PdfPTable(5);
            float[] headers = { 30, 45, 52, 52, 68 }; //Header Widths  
            itemTable.SetWidths(headers);

            AddCellToHeader(itemTable, "Id");
            AddCellToHeader(itemTable, "Leave Type");
            AddCellToHeader(itemTable, "From Date");
            AddCellToHeader(itemTable, "To Date");
            AddCellToHeader(itemTable, "Reason");

            foreach (var emp in leaves)
            {

                AddCellToBody(itemTable, emp.EmployeeUserId);
                AddCellToBody(itemTable, emp.LeaveType);
                if (emp.LeaveType == Constant.Permission)
                {
                    AddCellToBody(itemTable, emp.LeaveFromDate.ToString());
                    AddCellToBody(itemTable, emp.LeaveToDate.ToString());
                }
                else
                {
                    AddCellToBody(itemTable, emp.LeaveFromDate.ToString(Constant.DateFormat));
                    AddCellToBody(itemTable, emp.LeaveToDate.ToString(Constant.DateFormat));
                }

                AddCellToBody(itemTable, emp.Reason);

            }
            pdfDoc.Add(itemTable);

            PdfContentByte cb = new PdfContentByte(writer);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = new PdfContentByte(writer);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
            cb.SetColorFill(BaseColor.LIGHT_GRAY);
            cb.EndText();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            cb.Stroke();
            string strPDFFileName = string.Format("EmployeeLeaveReports" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
            pdfDoc.Close();
            var content = PDFData.ToArray();
            HttpContext.Session.Set(Constant.fileId, content);
            var fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
            return fileId;

        }

        //TimeSheet Reports

        /// <summary>
        /// Logic to get all the timesheet list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TimeSheetReports()
        {
            var listOfReports = new TimeSheetReports();
            var companyId = GetSessionValueForCompanyId;
            var employeeDropdown = await _reportService.GetAllEmployeesDrropdown(companyId);
            // var designation = await _employeesService.GetAllDesignation();
            listOfReports.EmployeeId = 0;
            var department = await _timeSheetService.GetAllTimeSheet(listOfReports.EmployeeId,companyId);
            listOfReports.ReportingPeople = employeeDropdown;
            listOfReports.ProjectNames = await _reportService.GetAllProjectNames(listOfReports.EmployeeId, companyId);          
            
            return View(listOfReports);
        }

        /// <summary>
        /// Logic to get filter the timesheet detail and count
        /// </summary>
        /// <param name="timeSheetReports,pager,columnName,columnDirection" ></param>
        [HttpGet]
        public async Task<IActionResult> FilterEmployeeByTimeSheet(TimeSheetReports timeSheetReport,SysDataTablePager pager, string columnName, string columnDirection)
        {  
            var companyId = HttpContext.Session.GetInt32("CompanyId");
            timeSheetReport.CompanyId = (int)companyId;
            var timesheetReports = await _reportService.GetAllEmployessByTimeSheetFilters(timeSheetReport,pager,columnName,columnDirection);
            var filteredRecord = await _reportService.GetFilterTimeSheetReportCount(timeSheetReport, pager);            
            return Json(new
            {
                iTotalRecords = filteredRecord,
                iTotalDisplayRecords = filteredRecord,
                data = timesheetReports.timeSheetReport
            });          
        }
        
        /// <summary>
        /// Logic to Create Pdf TimeSheet
        /// </summary>
        [HttpPost]
        public async Task<string> CreatePdfTimeSheet(TimeSheetReports timeSheet)
        {
            var fileId = "";
            var companyId = HttpContext.Session.GetInt32("CompanyId");
            if (companyId == null)
            {
                return fileId;
            }
                var sheet = await _reportService.GetAllEmployessByTimeSheetFilter(timeSheet, (int)companyId);
            try
            {            
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                if (PDFData == null)
                {
                    return fileId;
                }
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);
                if (writer == null)
                {
                    return fileId;
                }
                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;

                pdfDoc.Open();

                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 200f, 5f, 350f });


                headertable.DefaultCell.Border = Rectangle.BOX;
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                {
                    PdfPCell pdfCelllogo = new PdfPCell(logo);
                    pdfCelllogo.Border = Rectangle.NO_BORDER;
                    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    pdfCelllogo.BorderWidthBottom = 1f;
                    pdfCelllogo.PaddingTop = 10f;
                    pdfCelllogo.PaddingBottom = 10f;
                    headertable.AddCell(pdfCelllogo);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Time Sheet Log Report - " + timeSheet.StartTime + " " + "To" + " " + timeSheet.EndTime, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nextPostCell1.PaddingBottom = 20f;
                    nested.AddCell(nextPostCell1);

                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 6;
                    nesthousing.PaddingTop = 10f;
                    headertable.AddCell(nesthousing);
                }

                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(middlecell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.PaddingTop = 20f;
                    Invoicetable.AddCell(middlecell);
                }

                pdfDoc.Add(headertable);
                pdfDoc.Add(Invoicetable);

                //Create body table
                PdfPTable itemTable = new PdfPTable(8);
                float[] headers = { 58, 40, 52, 68, 45, 46, 44, 46 }; //Header Widths  
                itemTable.SetWidths(headers);

                AddCellToHeader(itemTable, "Name");
                AddCellToHeader(itemTable, "Id");
                AddCellToHeader(itemTable, "Project");
                AddCellToHeader(itemTable, "Task Name");
                AddCellToHeader(itemTable, "Date");
                AddCellToHeader(itemTable, "Start Time");
                AddCellToHeader(itemTable, "End Time");
                AddCellToHeader(itemTable, "Status");


                foreach (var emp in sheet.FilterViewTimeSheet)
                {
                    AddCellToBody(itemTable, emp.FirstName + " " + emp.LastName);
                    AddCellToBody(itemTable, emp.EmployeeUserId);
                    AddCellToBody(itemTable, emp.ProjectName);
                    AddCellToBody(itemTable, emp.TaskName);
                    AddCellToBody(itemTable, emp.StartDate.ToString(Constant.DateFormat));
                    AddCellToBody(itemTable, emp.StartTime.ToString("hh:mm:tt"));
                    AddCellToBody(itemTable, emp.EndTime.ToString("hh:mm:tt"));
                    if (emp.Status == 1)
                    {
                        AddCellToBody(itemTable, Constant.Pending);
                    }
                    else if (emp.Status == 2)
                    {
                        AddCellToBody(itemTable, Constant.InProgress);
                    }
                    else
                    {
                        AddCellToBody(itemTable, Constant.Completed);
                    }

                }
                pdfDoc.Add(itemTable);

                PdfContentByte cb = new PdfContentByte(writer);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();
                string strPDFFileName = string.Format("TimesheetLogReport" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
                pdfDoc.Close();
                var content = PDFData.ToArray();
                HttpContext.Session.Set(Constant.fileId, content);
                fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
                return fileId;
            }
            catch (Exception ex) { }

            return fileId;

        }

        /// <summary>
        /// Logic to Download TimeSheet Pdf
        /// </summary>
        [HttpGet]
        public virtual ActionResult DownloadTimeSheetPdf(string fileGuid)
        {
            var fileName = string.Format("TimeSheetReport_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                return File(data, "pdf/application", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public async Task<string> DownloadExcelForTimeSheet(TimeSheetReports timeSheetReports, SysDataTablePager pager)
        {
            var fileId = "";
            var companyId = HttpContext.Session.GetInt32("CompanyId");

            //var timeSheetRep = await _reportService.GetAllEmployessByTimeSheetFilters(timeSheetReports,pager);
            var timeSheetReport = await _reportService.GetAllEmployessByTimeSheetFilter(timeSheetReports, (int)companyId);

            if (timeSheetReport.FilterViewTimeSheet.Count > 0)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employee TimeSheet Details");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                worksheet.Cell(currentRow, 3).Value = Constant.ProjectName;
                worksheet.Cell(currentRow, 4).Value = Constant.TaskName;
                worksheet.Cell(currentRow, 5).Value = Constant.Date;
                worksheet.Cell(currentRow, 6).Value = Constant.StartTime;
                worksheet.Cell(currentRow, 7).Value = Constant.EndTime;
                worksheet.Cell(currentRow, 8).Value = Constant.Status;
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 8).Style.Font.Bold = true;

                foreach (var user in timeSheetReport.FilterViewTimeSheet)
                {
                    var strstaus = "";
                    var Status = user.Status;
                    currentRow++;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.DateFormatRevers);
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                    worksheet.Cell(currentRow, 1).Value = user.EmployeeUserId;
                    worksheet.Cell(currentRow, 2).Value = user.FirstName + " " + user.LastName;
                    worksheet.Cell(currentRow, 3).Value = user.ProjectName;
                    worksheet.Cell(currentRow, 4).Value = user.TaskName;
                    worksheet.Cell(currentRow, 5).Value = user.StartDate;
                    worksheet.Cell(currentRow, 6).Value = user.StartTime;
                    worksheet.Cell(currentRow, 7).Value = user.EndTime;
                    if (Status == (int)TimeSheetStatus.Pending)
                    {
                        strstaus = Constant.Pending;
                    }
                    else if (Status == (int)TimeSheetStatus.Inprogress)
                    {
                        strstaus = Constant.InProgress;
                    }
                    else if (Status == (int)TimeSheetStatus.Completed)
                    {
                        strstaus = Constant.Completed;
                    }
                    worksheet.Cell(currentRow, 8).Value = strstaus;

                }
                var fileName = string.Format("EmployeeTimeSheetDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                fileId = Guid.NewGuid().ToString() + "_" + fileName;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    var content = memoryStream.ToArray();
                    HttpContext.Session.Set(Constant.fileId, content);
                }
                return fileId;
            }
            return fileId;
        }

        /// <summary>
        /// Logic to get download the employeetimesheet detail  by particular employeetimesheetreport
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult DownloadTimeSheet(string fileGuid)
        {
            var fileName = string.Format("EmployeeTimeSheetDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                //HttpContext.Session.Set(Constant.fileId,null);
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Logic to download Pdf
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult DownloadPdf(string fileGuid)
        {
            var fileName = string.Format("EmployeeLeaveDetailsReport_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                return File(data, "pdf/application", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = WebColors.GetRGBColor("#1a76d1")
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 7, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5
            });
        }

        //Manual Log Report

        /// <summary>
        /// Logic to get all the manualLog list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManualLogReports()
        {
            var companyId = GetSessionValueForCompanyId;
            var listOfReports = new ManualLogReports();
            var employeeDropdown = await _reportService.GetAllEmployeesDrropdown(companyId);
            // var designation = await _employeesService.GetAllDesignation();
            var department = await _reportService.GetAllManualLog();
            listOfReports.ReportingPeople = employeeDropdown;
            listOfReports.ManualLog = null;
            return View(listOfReports);
        }

        /// <summary>
        /// Logic to get filter the employeemanuallog detail  by particular employeemanuallog
        /// </summary>
        /// <param name="manualLogReports" ></param>
        [HttpPost]
        public async Task<IActionResult> FilterEmployeeByManualLog(ManualLogReports manualLogReports)
        {
            var manuallogReports = await _reportService.GetAllEmployessByManualLogFilter(manualLogReports);
            return new JsonResult(manuallogReports);
        }

        /// <summary>
        /// Logic to get filter the ManualLog detail  by particular ManualLog
        /// </summary>
        /// <param name="file" ></param>
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = false;
            var manualLog = new List<ManualLog>();
            if (file != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    var data = 0;
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        data = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                        if (data != 0)
                        {
                            var user = new ManualLog();
                            user.Sno = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                            user.UserName = workSheet.Cells[rowIterator, 2].Text.ToString();
                            user.StartTime = workSheet.Cells[rowIterator, 3].Text.ToString();
                            user.EndTime = workSheet.Cells[rowIterator, 4].Text.ToString();
                            user.EntryStatus = workSheet.Cells[rowIterator, 5].Text.ToString();
                            user.TotalHours = workSheet.Cells[rowIterator, 6].Text.ToString();
                            user.BreakHours = workSheet.Cells[rowIterator, 7].Text.ToString();
                            user.EmpId = Convert.ToInt32(workSheet.Cells[rowIterator, 8].Value);
                            manualLog.Add(user);
                        }
                    }
                }
            }
            if (manualLog.Count > 0)
            {
                result = await _reportService.ImoprtFile(manualLog);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get all the manualLog list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManualLog()
        {
            var benefitsDetails = await _reportService.GetAllManualLog();
            return View(benefitsDetails);
        }

        /// <summary>
        /// Logic to create empty excel file
        /// </summary>
        public async Task<FileResult> Excel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ManualLog");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = Constant.Sno;
            worksheet.Cell(currentRow, 2).Value = Constant.UserName;
            worksheet.Cell(currentRow, 3).Value = Constant.StartTimeyyyymmddhhmm;
            worksheet.Cell(currentRow, 4).Value = Constant.Endtimeyyyymmddhhmm;
            worksheet.Cell(currentRow, 5).Value = Constant.EntryStatus;
            worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
            worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
            worksheet.Cell(currentRow, 8).Value = Constant.EmpId;
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ManualLog" + Constant.xlsx);
        }

        /// <summary>
        /// Logic to GetByEmployeeIdForProject
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetByEmployeeIdForProject(string employeeIds)
        {
            var companyId = GetSessionValueForCompanyId;
            if (string.IsNullOrEmpty(employeeIds))
            {
                var dropdownProjects = new DropdownProjects();
                dropdownProjects.ProjectNames = new List<ProjectNames>();
                dropdownProjects.ProjectNames.Add(new ProjectNames()
                {
                    ProjectId = 0,
                    ProjectName = "--Select Project--",
                });
                return new JsonResult(dropdownProjects);
            }
            var result = await _reportService.GetByEmployeeIdForProject(employeeIds, companyId);
            return new JsonResult(result);
        }


        //     /// <summary>
        //     /// Logic to get employee remaining leave report by particular empid
        //     /// </summary>
        //     [HttpGet]
        //     public async Task<IActionResult> EmployeeRemainingLeaveReport()
        //     {
        //         var sessionEmployeeId = GetSessionValueForEmployeeId;
        //         var leave = new EmployeeLeaveViewModel();
        //// var leaveSummary = await _leaveService.GetAllLeaveDetailsByEmpid(sessionEmployeeId);
        //var leaveSummary = await _leaveService.GetAllRemainingLeave(sessionEmployeeId);		
        //leave.LeaveCounts = leaveSummary.LeaveCounts;
        //         leaveSummary.EmpId = sessionEmployeeId;
        //         return View(leaveSummary);
        //     }       

        //AllEmployeesReport

        /// <summary>
        /// Logic to get all the AllEmployeesReport list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AllEmployeesReport()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the GetAllEmployees list
        /// </summary>
        /// <param name="pager" ></param>
        [HttpGet]
        public async Task<IActionResult>GetAllEmployees(SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var employeeslist = await _reportService.GetAllEmployeesList(pager, columnDirection, ColumnName, companyId);
            var employeesCount = await _reportService.GetAllEmployeesListCount(pager, companyId);           
            return Json(new
            {
                data = employeeslist.EmployeesDataModel,               
                iTotalDisplayRecords = employeesCount,
                iTotalRecords = employeesCount
            });
        }

        /// <summary>
        /// Logic to get all the EmployeesListCreatePdf 
        /// </summary>       
        [HttpPost]
        public async Task<string> EmployeesListCreatePdf()
        {
            var companyId = GetSessionValueForCompanyId;
            var employees = await _reportService.GetAllEmployeesListPdf(companyId);
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);
            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

            // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
            headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
            string webRootPath = _hostingEnvironment.WebRootPath;
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
            logo.ScaleToFit(100, 70);

            {
                PdfPCell pdfCelllogo = new PdfPCell(logo);
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCelllogo.BorderWidthBottom = 1f;
                pdfCelllogo.PaddingTop = 10f;
                pdfCelllogo.PaddingBottom = 10f;
                headertable.AddCell(pdfCelllogo);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middlecell.BorderWidthBottom = 1f;
                headertable.AddCell(middlecell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employees Report - " + DateTime.Now , titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.PaddingBottom = 20f;
                nested.AddCell(nextPostCell1);

                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 6;
                nesthousing.PaddingTop = 10f;
                headertable.AddCell(nesthousing);
            }

            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                Invoicetable.AddCell(middlecell);
            }

            {
                PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                emptyCell.Border = Rectangle.NO_BORDER;
                Invoicetable.AddCell(emptyCell);
            }

            {
                PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                emptyCell.Border = Rectangle.NO_BORDER;
                Invoicetable.AddCell(emptyCell);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                middlecell.PaddingTop = 20f;
                Invoicetable.AddCell(middlecell);
            }

            pdfDoc.Add(headertable);
            pdfDoc.Add(Invoicetable);

            //Create body table
            PdfPTable itemTable = new PdfPTable(4);
            float[] headers = { 45, 58, 55, 58 }; //Header Widths  
            itemTable.SetWidths(headers);

            AddCellToHeader(itemTable, "EmployeeName");
            AddCellToHeader(itemTable, "UserName");
            AddCellToHeader(itemTable, "OfficeEmail");
            AddCellToHeader(itemTable, "PhoneNumber");           

            foreach (var emp in employees.EmployeesDataModel)
            {
                AddCellToBody(itemTable, emp.EmployeeName);
                AddCellToBody(itemTable, emp.UserName);
                AddCellToBody(itemTable, emp.OfficeEmail);
                AddCellToBody(itemTable, emp.PhoneNumber);                
            }
            pdfDoc.Add(itemTable);

            PdfContentByte cb = new PdfContentByte(writer);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = new PdfContentByte(writer);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
            cb.SetColorFill(BaseColor.LIGHT_GRAY);
            cb.EndText();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            cb.Stroke();
            string strPDFFileName = string.Format("EmployeesListReports" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
            pdfDoc.Close();
            var content = PDFData.ToArray();
            HttpContext.Session.Set(Constant.fileId, content);
            var fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
            return fileId;
        }

        /// <summary>
        /// Logic to get all the DownloadEmployeesPdf 
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult DownloadEmployeesPdf(string fileGuid)
        {
            var fileName = string.Format("EmployeesListReports_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.pdf);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                return File(data, "pdf/application", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Logic to get all the DownloadExcel 
        /// </summary>        
        [HttpPost]
        public async Task<string> DownloadExcelEmployees()
        {
            var companyId = GetSessionValueForCompanyId;
            var fileId = "";
            var employeesList = await _reportService.GetAllEmployeesListPdf(companyId); ;
            if (employeesList.EmployeesDataModel.Count() > 0)
            {                
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Employees Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Common.Constant.EmployeeName;
                    worksheet.Cell(currentRow, 2).Value = Common.Constant.UserName;
                    worksheet.Cell(currentRow, 3).Value = Common.Constant.OfficeEmail;
                    worksheet.Cell(currentRow, 4).Value = Common.Constant.PhoneNumber;              
                    foreach (var user in employeesList.EmployeesDataModel)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.EmployeeName;
                        worksheet.Cell(currentRow, 2).Value = user.UserName;
                        worksheet.Cell(currentRow, 3).Value = user.OfficeEmail;
                        worksheet.Cell(currentRow, 4).Value = user.PhoneNumber;                       
                    }
                    var fileName = string.Format("EmployeesListReports__" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
                    fileId = Guid.NewGuid().ToString() + "_" + fileName;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        var content = memoryStream.ToArray();
                        HttpContext.Session.Set(Constant.fileId, content);
                    }
                    return fileId;                              
            }
            return fileId;
        }

        /// <summary>
        /// Logic to get all the DownloadEmployees 
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult DownloadEmployees(string fileGuid)
        {
            var fileName = string.Format("EmployeesListReports__" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
            if (HttpContext.Session.Get(Constant.fileId) != null)
            {
                byte[] data = HttpContext.Session.Get(Constant.fileId);
                //HttpContext.Session.Set(Constant.fileId,null);
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }


		/// <summary>
		/// Logic to Create Pdf Leave Remaining Report
		/// </summary>
		/// <param name="empId" ></param>
		[HttpPost]
		public async Task<string> CreatePdfLeaveRemaining(int empId)
		{
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetAllRemainingLeave(empId,companyId);
			
			Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
			MemoryStream PDFData = new MemoryStream();
			PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

			var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
			var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
			var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
			var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
			var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
			BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

			Rectangle pageSize = writer.PageSize;
			// Open the Document for writing
			pdfDoc.Open();
			//Add elements to the document here

			// Create the header table 
			PdfPTable headertable = new PdfPTable(3);
			headertable.HorizontalAlignment = 0;
			headertable.WidthPercentage = 100;
			headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

			// headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
			headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
			string webRootPath = _hostingEnvironment.WebRootPath;
			iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
			logo.ScaleToFit(100, 70);

			{
				PdfPCell pdfCelllogo = new PdfPCell(logo);
				pdfCelllogo.Border = Rectangle.NO_BORDER;
				pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
				pdfCelllogo.BorderWidthBottom = 1f;
				pdfCelllogo.PaddingTop = 10f;
				pdfCelllogo.PaddingBottom = 10f;
				headertable.AddCell(pdfCelllogo);
			}

			{
				PdfPCell middlecell = new PdfPCell();
				middlecell.Border = Rectangle.NO_BORDER;
				middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
				middlecell.BorderWidthBottom = 1f;
				headertable.AddCell(middlecell);
			}

			{
				PdfPTable nested = new PdfPTable(1);
				nested.DefaultCell.Border = Rectangle.NO_BORDER;
				PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Remaining Leave Report  " , titleFont));
				nextPostCell1.Border = Rectangle.NO_BORDER;
				nextPostCell1.PaddingBottom = 20f;
				nested.AddCell(nextPostCell1);

				nested.AddCell("");
				PdfPCell nesthousing = new PdfPCell(nested);
				nesthousing.Border = Rectangle.NO_BORDER;
				nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
				nesthousing.BorderWidthBottom = 1f;
				nesthousing.Rowspan = 6;
				nesthousing.PaddingTop = 10f;
				headertable.AddCell(nesthousing);
			}

			PdfPTable Invoicetable = new PdfPTable(3);
			Invoicetable.HorizontalAlignment = 0;
			Invoicetable.WidthPercentage = 100;
			Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
			Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

			{
				PdfPCell middlecell = new PdfPCell();
				middlecell.Border = Rectangle.NO_BORDER;
				Invoicetable.AddCell(middlecell);
			}

			PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
			emptyCell.Border = Rectangle.NO_BORDER;
			Invoicetable.AddCell(emptyCell);

			{
				PdfPCell middlecell = new PdfPCell();
				middlecell.Border = Rectangle.NO_BORDER;
				middlecell.PaddingTop = 20f;
				Invoicetable.AddCell(middlecell);
			}

			pdfDoc.Add(headertable);
			pdfDoc.Add(Invoicetable);

			//Create body table
			PdfPTable itemTable = new PdfPTable(10);
			float[] headers = { 45, 30, 30, 30, 30, 40, 40, 40, 40, 45 }; //Header Widths  
			itemTable.SetWidths(headers);

			AddCellToHeader(itemTable, "Employee Name");
			AddCellToHeader(itemTable, "Id");
			AddCellToHeader(itemTable, "Casual Leave");
			AddCellToHeader(itemTable, "Sick Leave");
			AddCellToHeader(itemTable, "Earned Leave");
			AddCellToHeader(itemTable, "Maternity Leave");
			AddCellToHeader(itemTable, "Compensatory Off");
			AddCellToHeader(itemTable, "Approved Lop");
			AddCellToHeader(itemTable, "Approved Leave");
			AddCellToHeader(itemTable, "Remaining Leave");

			foreach (var emp in leaveSummary.EmployeeTotalLeaveDetails)
			{

				AddCellToBody(itemTable, emp.EmployeeName);
				AddCellToBody(itemTable, emp.UserName);
				AddCellToBody(itemTable, emp.CasualLeaveRemaining.ToString());
				AddCellToBody(itemTable, emp.SickLeaveRemaining.ToString());
				AddCellToBody(itemTable, emp.EarnedLeaveRemaining.ToString());
				AddCellToBody(itemTable, emp.MaternityLeaveRemaining.ToString());
				AddCellToBody(itemTable, emp.CompensatoryOffRemaining.ToString());
				AddCellToBody(itemTable, emp.ApprovedLOP.ToString());
				AddCellToBody(itemTable, emp.ApprovedLeave.ToString());
				AddCellToBody(itemTable, emp.RemaingLeave.ToString());				

			}
			pdfDoc.Add(itemTable);

			PdfContentByte cb = new PdfContentByte(writer);

			BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
			cb = new PdfContentByte(writer);
			cb = writer.DirectContent;
			cb.BeginText();
			cb.SetFontAndSize(bf, 8);
			cb.SetTextMatrix(pageSize.GetLeft(120), 20);
			cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
			cb.SetColorFill(BaseColor.LIGHT_GRAY);
			cb.EndText();

			//Move the pointer and draw line to separate footer section from rest of page
			cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
			cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
			cb.Stroke();
			string strPDFFileName = string.Format("EmployeeRemainingLeaveReports" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
			pdfDoc.Close();
			var content = PDFData.ToArray();
			HttpContext.Session.Set(Constant.fileId, content);
			var fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
			return fileId;

		}


		/// <summary>
		/// Logic to Remaining Leave download Pdf
		/// </summary>
		/// <param name="fileGuid" ></param>
		[HttpGet]
		public virtual ActionResult RemainingLeaveDownloadPdf(string fileGuid)
		{
			var fileName = string.Format("EmployeeRemainingLeaveReports" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
			if (HttpContext.Session.Get(Constant.fileId) != null)
			{
				byte[] data = HttpContext.Session.Get(Constant.fileId);
				return File(data, "pdf/application", fileName);
			}
			else
			{				
				return new EmptyResult();
			}
		}


		/// <summary>
		/// Logic to get download the employee remaining leave report detail  by particular employeereport
		/// </summary>
		/// <param name="empId" ></param>
		[HttpPost]
		public async Task<string> DownloadExcelRemainingLeave(int empId)
		{
            var companyId = GetSessionValueForCompanyId;
            var fileId = "";
			var leaveSummary = await _leaveService.GetAllRemainingLeave(empId,companyId);


			if (leaveSummary.EmployeeTotalLeaveDetails.Count() > 0)
			{

				using var workbook = new XLWorkbook();
				var worksheet = workbook.Worksheets.Add("EmployeeRemainingLeaveReports");
				var currentRow = 1;
				worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
				worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
				worksheet.Cell(currentRow, 3).Value = Constant.CasualLeave;
				worksheet.Cell(currentRow, 4).Value = Constant.SickLeave;
				worksheet.Cell(currentRow, 5).Value = Constant.EarnedLeave;
				worksheet.Cell(currentRow, 6).Value = Constant.MaternityLeave;
				worksheet.Cell(currentRow, 7).Value = Constant.CompensatoryOff;
				worksheet.Cell(currentRow, 8).Value = Constant.ApprovedLOP;
                worksheet.Cell(currentRow, 9).Value = Constant.ApproveLeave;
				worksheet.Cell(currentRow, 10).Value = Constant.RemainingLeave;
				worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
				worksheet.Cell(currentRow, 10).Style.Font.Bold = true;

				foreach (var user in leaveSummary.EmployeeTotalLeaveDetails)
				{
					currentRow++;					
					worksheet.Cell(currentRow, 1).Value = user.UserName;
					worksheet.Cell(currentRow, 2).Value = user.EmployeeName ;
					worksheet.Cell(currentRow, 3).Value = user.CasualLeaveRemaining;
					worksheet.Cell(currentRow, 4).Value = user.SickLeaveRemaining;
					worksheet.Cell(currentRow, 5).Value = user.EarnedLeaveRemaining;
					worksheet.Cell(currentRow, 6).Value = user.MaternityLeaveRemaining;
					worksheet.Cell(currentRow, 7).Value = user.CompensatoryOffRemaining;
					worksheet.Cell(currentRow, 8).Value = user.ApprovedLOP;
					worksheet.Cell(currentRow, 9).Value = user.ApprovedLeave;
					worksheet.Cell(currentRow, 10).Value = user.RemaingLeave;

				}
				var fileName = string.Format("EmployeeRemainingLeaveReports_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
				fileId = Guid.NewGuid().ToString() + "_" + fileName;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					workbook.SaveAs(memoryStream);
					memoryStream.Position = 0;
					var content = memoryStream.ToArray();
					HttpContext.Session.Set(Constant.fileId, content);
				}
			}
			return fileId;
		}



		/// <summary>
		/// Logic to get download the employee remaining leave report detail  by particular employeereport
		/// </summary>
		/// <param name="fileGuid" ></param>
		[HttpGet]
		public virtual ActionResult DownloadRemainingleave(string fileGuid)
		{
			var fileName = string.Format("EmployeeRemainingLeaveReports_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
			if (HttpContext.Session.Get(Constant.fileId) != null)
			{
				byte[] data = HttpContext.Session.Get(Constant.fileId);				
				return File(data, "application/vnd.ms-excel", fileName);
			}
			else
			{				
				return new EmptyResult();
			}
		}
	}
}
