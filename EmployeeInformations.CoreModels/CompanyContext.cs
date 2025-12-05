namespace EmployeeInformations.CoreModels
{
    public class CompanyContext : ICompanyContext
    {
        private int _companyId;
        private int _displayMode;
        public int CompanyId
        {
            get
            {
                return this._companyId;
            }
            set
            {
                this._companyId = value;
            }
        }

        public int DisplayMode
        {
            get
            {
                return this._displayMode;
            }
            set
            {
                this._displayMode = value;
            }
        }

    }
}
