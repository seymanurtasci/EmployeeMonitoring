using EmployeeMonitoring.Web.DataAccess.Abstract;
using EmployeeMonitoring.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.SqlServer.EFCore
{
    public class EFCorePermissionRepository : EFCoreGenericRepository<Permission, DataContext>, IPermissionRepository
    {
        public List<Permission> List()
        {
            using (var context = new DataContext())
            {
                return context.Set<Permission>().Include(p=>p.Employee).ToList();
            }
        }

        public List<Permission> ListById(int id)
        {
            using (var context = new DataContext())
            {
                return context.Set<Permission>().Where(p=>p.EmployeeId==id).Include(p => p.Employee).ToList();
            }
        }
    }
}
