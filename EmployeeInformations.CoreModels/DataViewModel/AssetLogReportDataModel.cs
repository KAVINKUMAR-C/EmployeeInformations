namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class AssetLogReportDataModel
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public int AssetTypeId { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string? Event { get; set; }
        public int EmpId { get; set; }
        public DateTime CreatedDate { get; set; }
    }


     public class AssetsLogModel
     {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string? AssetCode { get; set; }
        public int AssetTypeId { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string? Event { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AuthorName { get; set; }
        public string? TypeName { get; set; }
     }
}
