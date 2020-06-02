using EmployeeMonitoring.Web.DataAccess.Abstract;
using EmployeeMonitoring.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.SqlServer.EFCore
{
    public class EFCoreEmployeeRepository : EFCoreGenericRepository<Employee, DataContext>, IEmployeeRepository
    {
        public List<Employee> Get(int id)
        {
            using (var context = new DataContext())
            {
                return context.Set<Employee>().Where(e=>e.DepartmentId==id).Include(e => e.Department).ToList();
            }
        }

        public List<Employee> List()
        {
            using (var context = new DataContext())
            {
                return context.Set<Employee>().Include(e=>e.Department).ToList();
            }
        }
    }
}
