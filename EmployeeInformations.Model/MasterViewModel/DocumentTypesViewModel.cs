namespace EmployeeInformations.Model.MasterViewModel
{
    public class DocumentTypesViewModel
    {
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public List<DocumentType> DocumentType { get; set; }
    }

    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int DocumentNameCount {  get; set; }
    }
}
