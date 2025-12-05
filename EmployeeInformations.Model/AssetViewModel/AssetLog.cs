using EmployeeInformations.CoreModels.DataViewModel;
namespace EmployeeInformations.Model.AssetViewModel
{
    public class AssetLog
    {
        public int AssetLogId { get; set; }
        public int AssetId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public string AssetNo { get; set; }
        public int AssetType { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; }
        public string AssetTypeName { get; set; }
    }

    public class ChangeLog
    {
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
    }

    public class AssetLogViewModel
    {
        public int AssetLogId { get; set; }
        public int AssetId { get; set; }
        public int CompanyId { get; set; }
        public int EmpId { get; set; }
        public string AssetNo { get; set; }
        public int AssetType { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string Event { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AuthorName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string AssetNos { get; set; }
        public string AssetTypeName { get; set; }
        public List<AssetLog> AssetLog { get; set; }
        public List<AssetDropdown> AssetDropdowns { get; set; }
        public List<AssetDetailsDropdown> AssetDetailsDropdown { get; set; }        
        public string? ColumnName { get; set; }
        public string? ColumnDirection { get; set; }
        public List<AssetsLogModel>?  AssetsLogModels { get; set; }
    }

    public class AssetDropdown
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeIdWithName { get; set; }
    }

    public class AssetDetailsDropdown
    {
        public string AssetCodeName { get; set; }
        public string AssetNo { get; set; }
    }

    public class FilterViewAssetLogReport
    {
        public int Id { get; set; }
        public string? AssetNo { get; set; }
        public int AssetTypeId { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string? Event { get; set; }
        public int EmpId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
