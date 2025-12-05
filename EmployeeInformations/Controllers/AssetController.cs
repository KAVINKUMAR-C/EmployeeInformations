using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Model.AssetViewModel;
using EmployeeInformations.Model.PagerViewModel;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
//using Document = iTextSharp.text.Document;
//using Font = iTextSharp.text.Font;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
//using PageSize = iTextSharp.text.PageSize;

namespace EmployeeInformations.Controllers
{
    public class AssetController : BaseController
    {
        private readonly IAssetService _assetservice;
        private readonly ICompanyService _companyService;
        private readonly IReportService _reportService;
        private readonly IEmployeesService _employeesService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AssetController(IAssetService assetservice, ICompanyService companyservice, IReportService reportService, IEmployeesService employeesService, IHostingEnvironment hostingEnvironment)
        {
            _assetservice = assetservice;
            _companyService = companyservice;
            _reportService = reportService;
            _employeesService = employeesService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Logic to get all the assetcategory list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Asset()
        {
            var companyId = GetSessionValueForCompanyId;
            var asset = await _assetservice.GetAllAssetCategory(companyId);
            return View(asset);
        }

        /// <summary>
        /// Logic to get create the assetcategory detail  by particular assetcategory
        /// </summary>
        /// <param name="assetCategory" ></param>        
		[HttpPost]
        public async Task<IActionResult> AddAssetCategory(AssetCategory assetCategory)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.Create(assetCategory, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get update the assetcategory detail  by particular assetcategory
        /// </summary>
        /// <param name="assetCategory" ></param>
        [HttpPost]
        public async Task<int> UpdateCategory(AssetCategory assetCategory)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.Upadate(assetCategory,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get check categoryname the assetcategory detail  by particular categoryname not allow repeated categoryname
        /// </summary>
        /// <param name="categoryName" ></param>
        [HttpPost]
        public async Task<int> GetCategoryName(string categoryName)
        {
            var companyId = GetSessionValueForCompanyId;
            var categoryNameCount = await _assetservice.GetCategoryName(categoryName, companyId);
            return categoryNameCount;
        }

        /// <summary>
        /// Logic to get check categoryCode the assetcategory detail  by particular categoryCode not allow repeated categoryCode
        /// </summary>
        /// <param name="categoryCode" ></param>  
        [HttpPost]
        public async Task<int> GetCategoryCode(string categoryCode)
        {
            var companyId = GetSessionValueForCompanyId;
            var categoryCodeCount = await _assetservice.GetCategoryCode(categoryCode, companyId);
            return categoryCodeCount;
        }

        /// <summary>
        /// Logic to get soft deleted the assetcategory detail  by particular assetcategory
        /// </summary>
        /// <param name="categoryId">assetcategory</param>
        [HttpPost]
        public async Task<IActionResult> DeletedAssetCategory(int categoryId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.DeletedAssetCategory(categoryId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get update the assetcategory detail  by particular assetcategory status
        /// </summary>
        /// <param name="assetCategory" ></param>
        [HttpPost]
        public async Task<int> UpdateAssetCategory(AssetCategory assetCategory)
        {
            assetCategory.CategoryId = GetSessionValueForCompanyId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.UpdateAssetCategory(assetCategory, companyId);
            return result;
        }

        //Type

        /// <summary>
        ///  Logic to get all the assettypes list
        /// </summary>       
        [HttpGet]
        public async Task<IActionResult> Types()
        {
            var companyId = GetSessionValueForCompanyId;
            var assetTypes = new AssetTypeViewModal();
            var assetCategory = await _assetservice.GetAllAssetCategoryName(companyId);
            assetTypes = await _assetservice.GetAllAssetType(companyId);
            assetTypes.AssetCategoryName = assetCategory;
            return View(assetTypes);
        }

        /// <summary>
        /// Logic to get create the assettype detail  by particular assettype
        /// </summary>
        /// <param name="assetTypes" ></param>        
        [HttpPost]
        public async Task<IActionResult> AddAssetType(AssetTypes assetTypes)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.CreateAssetType(assetTypes,companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get create the assettype detail  by particular assettype
        /// </summary>
        /// <param name="assetTypes" ></param>
        [HttpPost]
        public async Task<int> UpadateAssetType(AssetTypes assetTypes)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.UpadateAssetType(assetTypes,companyId);
            return result;
        }

        /// <summary>
        /// Logic to GetTypeName
        /// </summary>
        /// <param name="typeName" ></param>
        [HttpPost]
        public async Task<int> GetTypeName(string typeName)
        {
            var companyId = GetSessionValueForCompanyId;
            var typeNameCount = await _assetservice.GetTypeName(typeName,companyId);
            return typeNameCount;
        }

        /// <summary>
        /// Logic to get soft deleted the assettype detail  by particular assettype
        /// </summary>
        /// <param name="Id" >assettype</param>
        [HttpPost]
        public async Task<IActionResult> DeletedAssetType(int id)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.DeletedAssetType(id,companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get update the assettype detail  by particular assettype
        /// </summary>
        /// <param name="assetTypes" ></param>
        [HttpPost]
        public async Task<int> UpdateAssetType(AssetTypes assetTypes)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.UpdateAssetType(assetTypes,companyId);
            return result;
        }

        /// <summary>
        /// Logic to get categoryId based asset type  the assets detail  by particular categoryId
        /// </summary>
        /// <param name="categoryId" >assets</param>
        [HttpGet]
        public async Task<IActionResult> GetBycategoryId(int categoryId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.GetBycategoryId(categoryId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get categoryId based asset code  the assets detail  by particular categoryId
        /// </summary>
        /// <param name="categoryId" >assets</param>
        public async Task<IActionResult> GetRefCategoryId(int categoryId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.GetRefCategoryId(categoryId, companyId);
            return new JsonResult(result);
        }

        //Assets

        /// <summary>
        /// Logic to get the assets list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AllAssets()
        {
            // var allAssets = await _assetservice.GetAllAssets();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets(SysDataTablePager pager, string columnDirection, string ColumnName)
        {
            var companyId = GetSessionValueForCompanyId;
            var assetcount = await _assetservice.GetAssetListCount(pager, companyId);
            var assetdetails = await _assetservice.GetAllEmployeesList(pager, columnDirection, ColumnName, companyId);
            return Json(new
            {
                data = assetdetails.assetViewModels,
                iTotalDisplayRecords = assetcount,
                iTotalRecords = assetcount
            });
        }

        /// <summary>
        /// Logic to get Assets Data
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateAllAssets()
        {
            
            var sessionCompanyId = GetSessionValueForCompanyId;
            var allAssets = new AllAssets();
            var employeeDrop = await _reportService.GetAllEmployeesDrropdown(sessionCompanyId);
            var company = await _companyService.GetByCompanyId(sessionCompanyId);
            allAssets.CompanyName = company.CompanyName;
            allAssets.AssetStatus = await _assetservice.GetAllAssetStatus(sessionCompanyId);
            allAssets.AssetCategory = await _assetservice.GetAssetCategory(sessionCompanyId);
            allAssets.AssetTypes = await _assetservice.GetAssetType(sessionCompanyId);
            allAssets.reportingPeople = employeeDrop;
            allAssets.BranchLocations = await _assetservice.GetBranchLocationId(sessionCompanyId);
            allAssets.AssetBrandType = await _assetservice.GetAssetBrand(sessionCompanyId);
            return View(allAssets);
        }

        /// <summary>
        /// Logic to get create the assets detail  by particular assets
        /// <param name="allAssets" ></param>
        /// </summary>
        [HttpPost]
        public async Task<int> AddAssets(AllAssets allAssets)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var sessionCompanyId = GetSessionValueForCompanyId;
            var result = await _assetservice.CreateAssets(allAssets, sessionEmployeeId, sessionCompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get edit the asset detail  by particular asset
        /// </summary>
        /// <param name="allAssetsId" >asset</param>
        [HttpGet]
        public IActionResult EditAssets(int allAssetsId)
        {
            var allAssets = new AllAssets();
            allAssets.AllAssetsId = allAssetsId;
            return PartialView("EditAssets", allAssets);
        }

        /// <summary>
        /// Logic to get update the assets detail  by particular assets
        /// </summary>
        /// <param name="allAssetsId" >assets</param>
        [HttpGet]
        public async Task<IActionResult> UpdateAllAssets(int allAssetsId)
        {          
            var sessionCompanyId = GetSessionValueForCompanyId;
            var employeeDrop = await _reportService.GetAllEmployeesDrropdown(sessionCompanyId);
            var company = await _companyService.GetByCompanyId(sessionCompanyId);
            var assets = await _assetservice.GetByAllAssetsId(allAssetsId, sessionCompanyId);
            assets.AssetStatus = await _assetservice.GetAllAssetStatus(sessionCompanyId);
            assets.AssetCategory = await _assetservice.GetAssetCategory(sessionCompanyId);
            assets.AssetTypes = await _assetservice.GetAssetType(sessionCompanyId);
            assets.reportingPeople = employeeDrop;
            assets.BranchLocations = await _assetservice.GetBranchLocationId(sessionCompanyId);
            assets.AssetBrandType = await _assetservice.GetAssetBrand(sessionCompanyId);
            assets.CompanyName = company.CompanyName;
            return PartialView("UpdateAllAssets", assets);
        }

        /// <summary>
        /// Logic to get update the assets detail by particular assets
        /// </summary>
        /// <param name="allAssets" >assets</param>
        [HttpPost]
        public async Task<Int32> UpdateAllAssets(AllAssets allAssets)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            allAssets.CompanyId = companyId;
            var result = await _assetservice.CreateAssets(allAssets, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the assets detail  by particular assets
        /// </summary>
        /// <param name="allAssetsId" >assets</param>
        [HttpPost]
        public async Task<IActionResult> DeleteAllAssets(int allAssetsId)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _assetservice.DeleteAllAssets(allAssetsId, sessionEmployeeId, companyId);
            return Json(result);
        }

        /// <summary>
        /// Logic to get all the assetlog list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> HistoryAssetLog(AssetLogViewModel assetLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            assetLogViewModel.AssetDetailsDropdown = await _assetservice.GetAllAssetDetailsDropdown(companyId);
            assetLogViewModel.AssetDropdowns = await _assetservice.GetAllAssetDrropdown(companyId);
            return View(assetLogViewModel);
        }

        /// <summary>
        /// Logic to get the AssetLog details  
        /// </summary>
        /// <param name="pager,assetLogViewModel" ></param>       
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAssetLogList(SysDataTablePager pager, AssetLogViewModel assetLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var assetLog = await _assetservice.GetAllEmployeesAssetLogList(pager, assetLogViewModel, companyId);
            var assetLogCount = await _assetservice.GetAllEmployeesAssetLogCount(pager, assetLogViewModel, companyId);
            return Json(new
            {
                data = assetLog.AssetsLogModels,
                iTotalDisplayRecords = assetLogCount,
                iTotalRecords = assetLogCount
            });
        }

        /// <summary>
        /// Logic to get display the asset detail  by particular asset
        /// </summary>
        /// <param name="allAssetsId" >asset</param>
        public async Task<IActionResult> ViewAllAsset(int allAssetsId,int companyId)
        {
            var assetsDetails = await _assetservice.GetAssetByEmployeeIds(allAssetsId, companyId);
            return View(assetsDetails);
        }

        /// <summary>
        /// Logic to get filter the AssetLog details  
        /// </summary>
        /// <param name="pager,assetLogViewModel" ></param>       
        [HttpGet]
        public async Task<IActionResult> FilterAssetLog(SysDataTablePager pager, AssetLogViewModel assetLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var assetLog = await _assetservice.GetAllEmployessByAssetLogFilter(pager, assetLogViewModel, companyId);
            var assetLogCount = await _assetservice.GetAllEmployeesAssetLogCount(pager, assetLogViewModel, companyId);
            return Json(new
            {
                data = assetLog.AssetsLogModels,
                iTotalDisplayRecords = assetLogCount,
                iTotalRecords = assetLogCount
            });
        }

        /// <summary>
        /// Logic to get DownloadExcel the employeeChangeLogViewModel detail  by particular employeereport
        /// </summary>
        /// <param name="fileGuid" ></param>
        public async Task<string> DownloadExcel(AssetLogViewModel assetLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;
            var fileId = "";
            var attendanceList = await _assetservice.GetAllAssetByEmployeeLogFilter(assetLogViewModel, companyId);
            if (attendanceList.AssetLog.Count() > 0)
            {
                if (assetLogViewModel.EmpId == 0)
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("AssetLog Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Common.Constant.AssetCode;
                    worksheet.Cell(currentRow, 2).Value = Common.Constant.AssetType;
                    worksheet.Cell(currentRow, 3).Value = Common.Constant.FieldName;
                    worksheet.Cell(currentRow, 4).Value = Common.Constant.PreviousValue;
                    worksheet.Cell(currentRow, 5).Value = Common.Constant.NewValue;
                    worksheet.Cell(currentRow, 6).Value = Common.Constant.Event;
                    worksheet.Cell(currentRow, 7).Value = Common.Constant.AuthorName;
                    worksheet.Cell(currentRow, 8).Value = Common.Constant.TimeStamp;

                    foreach (var user in attendanceList.AssetLog)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.AssetNo;
                        worksheet.Cell(currentRow, 2).Value = user.AssetTypeName;
                        worksheet.Cell(currentRow, 3).Value = user.FieldName;
                        worksheet.Cell(currentRow, 4).Value = user.PreviousValue;
                        worksheet.Cell(currentRow, 5).Value = user.NewValue;
                        worksheet.Cell(currentRow, 6).Value = user.Event;
                        worksheet.Cell(currentRow, 7).Value = user.AuthorName;
                        worksheet.Cell(currentRow, 8).Value = user.CreatedDate;

                    }
                    var fileName = string.Format("AssetLogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
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
                else
                {
                    var employeeDetails = await _employeesService.GetEmployeeById(assetLogViewModel.EmpId, companyId);
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add(employeeDetails.UserName + "_AssetLog Details");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = Constant.AssetCode;
                    worksheet.Cell(currentRow, 2).Value = Constant.AssetType;
                    worksheet.Cell(currentRow, 3).Value = Constant.FieldName;
                    worksheet.Cell(currentRow, 4).Value = Constant.PreviousValue;
                    worksheet.Cell(currentRow, 5).Value = Constant.NewValue;
                    worksheet.Cell(currentRow, 6).Value = Constant.Event;
                    worksheet.Cell(currentRow, 7).Value = Constant.AuthorName;
                    worksheet.Cell(currentRow, 8).Value = Constant.TimeStamp;


                    foreach (var user in attendanceList.AssetLog)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = user.AssetNo;
                        worksheet.Cell(currentRow, 2).Value = user.AssetType;
                        worksheet.Cell(currentRow, 3).Value = user.FieldName;
                        worksheet.Cell(currentRow, 4).Value = user.PreviousValue;
                        worksheet.Cell(currentRow, 5).Value = user.NewValue;
                        worksheet.Cell(currentRow, 6).Value = user.Event;
                        worksheet.Cell(currentRow, 7).Value = user.AuthorName;
                        worksheet.Cell(currentRow, 8).Value = user.CreatedDate;

                    }
                    var fileName = string.Format(employeeDetails.UserName + "_AssetLogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
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
            }
            return fileId;
        }

        //Brand

        /// <summary>
        /// Logic to get all the assetbrand list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Brand()
        {
            var companyId = GetSessionValueForCompanyId;
            var brand = await _assetservice.GetAllBrand(companyId);
            return View(brand);
        }

        /// <summary>
        /// Logic to get create the assetBrandType detail  by particular assetBrandType
        /// </summary>
        /// <param name="assetBrandType" ></param>       
        [HttpPost]
        public async Task<IActionResult> AddAssetBrand(AssetBrandType assetBrandType)
        {
            assetBrandType.CompanyId = GetSessionValueForCompanyId;
            var result = await _assetservice.Create(assetBrandType);
            return new JsonResult(result);
        }


        /// <summary>
        /// Logic to GetBrandName
        /// </summary>
        /// <param name="brandType" ></param>
        [HttpPost]
        public async Task<int> GetBrandName(string brandType)
        {
            var companyId = GetSessionValueForCompanyId;
            var typeNameCount = await _assetservice.GetBrandName(brandType, companyId);
            return typeNameCount;
        }

        /// <summary>
        /// Logic to get update status the assetBrandType detail  by particular assetBrandType
        /// </summary>
        /// <param name="assetBrandType" ></param>
        [HttpPost]
        public async Task<int> UpdateAssetBrand(AssetBrandType assetBrandType)
        {
            assetBrandType.CompanyId = GetSessionValueForCompanyId;
            var result = await _assetservice.UpdateAssetBrand(assetBrandType);
            return result;
        }

        /// <summary>
        /// Logic to get update the assettype detail  by particular assettype
        /// </summary>
        /// <param name="assetBrandType" ></param>
        [HttpPost]
        public async Task<int> UpadateAssetBrandType(AssetBrandType assetBrandType)
        {
            assetBrandType.CompanyId = GetSessionValueForCompanyId;
            var result = await _assetservice.UpadateAssetBrandType(assetBrandType);
            return result;
        }

        /// <summary>
        /// Logic to get soft deleted the assetBrandType detail  by particular assetBrandType
        /// </summary>
        /// <param name="brandTypeId" >assetBrandType</param>
        [HttpPost]
        public async Task<IActionResult> DeletedAssetBrand(int brandTypeId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _assetservice.DeletedAssetBrand(brandTypeId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get download the employeeschangelog details  
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult Download(string fileGuid)
        {
            var fileName = string.Format("AssetLogDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.xlsx);
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
        /// Logic to CreatePdf
        /// </summary>
        /// <param name="assetLogViewModel" ></param>
        [HttpPost]
        public async Task<string> CreatePdf(AssetLogViewModel assetLogViewModel)
        {
            var companyId = GetSessionValueForCompanyId;

            var asset = await _assetservice.GetAllAssetByEmployeeLogFilter(assetLogViewModel, companyId);
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
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Asset Report - " + assetLogViewModel.StartDate + " " + "To" + " " + assetLogViewModel.EndDate, titleFont));
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
            float[] headers = { 45, 58, 55, 58, 52, 45, 58, 68 }; //Header Widths  
            itemTable.SetWidths(headers);

            AddCellToHeader(itemTable, "No");
            AddCellToHeader(itemTable, "Type");
            AddCellToHeader(itemTable, "Field");
            AddCellToHeader(itemTable, "Previous");
            AddCellToHeader(itemTable, "New");
            AddCellToHeader(itemTable, "Event");
            AddCellToHeader(itemTable, "Author");
            AddCellToHeader(itemTable, "TimeStamp");

            foreach (var emp in asset.AssetLog)
            {

                AddCellToBody(itemTable, emp.AssetNo);
                AddCellToBody(itemTable, emp.AssetTypeName);
                AddCellToBody(itemTable, emp.FieldName);
                AddCellToBody(itemTable, emp.PreviousValue);
                AddCellToBody(itemTable, emp.NewValue);
                AddCellToBody(itemTable, emp.Event);
                AddCellToBody(itemTable, emp.AuthorName);
                AddCellToBody(itemTable, emp.CreatedDate.ToString());

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
            string strPDFFileName = string.Format("AssetHistoryReports" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
            pdfDoc.Close();
            var content = PDFData.ToArray();
            HttpContext.Session.Set(Constant.fileId, content);
            var fileId = Guid.NewGuid().ToString() + "_" + strPDFFileName;
            return fileId;
        }

        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 10, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = WebColors.GetRGBColor("#1a76d1")
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });
        }

        /// <summary>
        /// Logic to download pdf file
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult DownloadPdf(string fileGuid)
        {
            var fileName = string.Format("AssetHistoryReport_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Common.Constant.pdf);
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
    }
}
