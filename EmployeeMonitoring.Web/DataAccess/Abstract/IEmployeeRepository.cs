using EmployeeMonitoring.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.Abstract
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        List<Employee> List();
        List<Employee> Get(int id);
    }
}
