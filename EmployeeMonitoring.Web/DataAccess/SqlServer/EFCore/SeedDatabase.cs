using EmployeeMonitoring.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.SqlServer.EFCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new DataContext();

            if (context.Departments.Count() == 0)
            {
                context.Departments.AddRange(Departments);
            }

            if (context.Employees.Count() == 0)
            {
                context.Employees.AddRange(Employees);
            }

            if (context.Permissions.Count() == 0)
            {
                context.Permissions.AddRange(Permissions);
            }

            context.SaveChanges();
        }

        private static Department[] Departments = {
            new Department(){Name="IT"},
            new Department(){Name="HR"}
        };

        private static Employee[] Employees = {
            new Employee(){Name="Şeymanur",Surname="Taşçı",StartDate=DateTime.Parse("20.09.2014"),DepartmentId=1,AnnualLeave=20,DaysLeft=18},
            new Employee(){Name="Sinem",Surname="Sarı",StartDate=DateTime.Parse("03.06.2019"),DepartmentId=2,AnnualLeave=14,DaysLeft=8},
        };

        private static Permission[] Permissions = {
            new Permission(){StartDate=DateTime.Parse("11.06.2020"),EndDate=DateTime.Parse("13.06.2020"),DayCount=2,EmployeeId=1},
            new Permission(){StartDate=DateTime.Parse("04.07.2020"),EndDate=DateTime.Parse("10.07.2020"),DayCount=6,EmployeeId=2},
        };
    }
}
