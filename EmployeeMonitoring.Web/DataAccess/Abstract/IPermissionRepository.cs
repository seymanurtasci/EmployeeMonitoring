﻿using EmployeeMonitoring.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.DataAccess.Abstract
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        List<Permission> List();
        List<Permission> ListById(int Id);
    }
}
