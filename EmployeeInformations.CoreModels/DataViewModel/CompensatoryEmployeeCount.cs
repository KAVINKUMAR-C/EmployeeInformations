using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class CompensatoryEmployeeCount
    {
        public  int EmployeeCount {  get; set; }
    }
}
