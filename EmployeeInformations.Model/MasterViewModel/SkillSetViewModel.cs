namespace EmployeeInformations.Model.MasterViewModel
{
    public class SkillSetViewModel
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public List<SkillSets> SkillSets { get; set; }
    }

    public class SkillSets
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int SkillNameCount { get; set; }
    }
}
