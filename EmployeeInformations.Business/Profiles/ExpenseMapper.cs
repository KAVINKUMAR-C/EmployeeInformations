using AutoMapper;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ExpensesViewModel;

namespace EmployeeInformations.Business.Profiles
{
    public class ExpenseMapper : Profile
    {
        public ExpenseMapper()
        {
            CreateMap<ExpensesEntity, Expenses>().ReverseMap();
            CreateMap<ExpenseDetailsEntity, ExpenseDetails>().ReverseMap();
            CreateMap<ExpenseAttachmentsEntity, ExpenseAttachments>().ReverseMap();
        }
    }
}
