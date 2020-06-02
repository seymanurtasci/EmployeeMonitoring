using EmployeeMonitoring.Web.DataAccess.Abstract;
using EmployeeMonitoring.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.SqlServer.EFCore
{
    public class EFCoreDepartmentRepository:EFCoreGenericRepository<Department, DataContext>, IDepartmentRepository
    {

    }
}
