namespace EmployeeInformations.Model.PrivilegeViewModel
{
    public class PageData
    {
        public string PageName { get; set; }
        public string CheckboxName { get; set; }

        public PageData() { }

        public string GetCheckboxName()
        {
            if (!String.IsNullOrEmpty(CheckboxName))
                return CheckboxName;
            return PageName;
        }
    }
}
