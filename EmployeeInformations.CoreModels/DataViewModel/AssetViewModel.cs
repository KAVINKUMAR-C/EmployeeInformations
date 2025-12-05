using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class AssetViewModels
    {
        public int AllAssetsId { get; set; }
        public string? AssetName { get; set; }
        public string? AssetCode { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? TypeName { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandType { get; set; }
        public string? EmployeeName { get; set;}
        public string? BranchLocationName { get; set; }
        public string? StatusName { get; set; }
       
    }
    public class AssetCount
    {
        public int AssetCountId { get; set; } 
    }
}
