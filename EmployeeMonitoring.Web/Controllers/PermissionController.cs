using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMonitoring.Web.DataAccess.Abstract;
using EmployeeMonitoring.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeMonitoring.Web.Controllers
{
    public class PermissionController : Controller
    {
        private IPermissionRepository _permissionRepository;
        private IEmployeeRepository _employeeRepository;

        public PermissionController(IPermissionRepository permissionRepository,IEmployeeRepository employeeRepository)
        {
            _permissionRepository = permissionRepository;
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            List<Permission> permissions = _permissionRepository.List();
            return View(permissions);
        }

        public IActionResult List(int id)
        {
            List<Permission> permissions = _permissionRepository.ListById(id);
            return View(permissions);
        }

        public IActionResult Create(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Permission permission, int? id)
        {
            if (ModelState.IsValid)
            {
                if (permission.StartDate > permission.EndDate)
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"Bitiş tarihi başlangıç tarihinden büyük olamaz.",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
                else if (permission.StartDate < DateTime.Now.Date)
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"Geriye dönük izin alamazsınız.",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
                else
                {
                    int count = (permission.EndDate - permission.StartDate).Days;
                    var employee = _employeeRepository.GetById((int)id);

                    if (count > employee.AnnualLeave)
                    {
                        var msg = new AlertMessage()
                        {
                            Message = $"İzin aldığınız gün sayısı, izin hakkınızdan fazla olamaz.",
                            AlertType = "danger"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
                    }
                    else
                    {
                        List<Employee> employees = _employeeRepository.Get(employee.DepartmentId);
                        foreach (var e in employees)
                        {
                            List<Permission> permissions = _permissionRepository.ListById(e.Id);
                            foreach (var p in permissions)
                            {
                                if (p.StartDate >= permission.StartDate && p.StartDate < permission.EndDate)
                                {
                                    var msg = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(msg);
                                    return View();
                                }
                                else if (permission.StartDate >= p.StartDate && permission.EndDate < p.EndDate)
                                {
                                    var msg = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(msg);
                                    return View();
                                }
                                else if (permission.StartDate >= p.StartDate && permission.StartDate < p.EndDate)
                                {
                                    var msg = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(msg);
                                    return View();
                                }
                            }

                        }
                        int result = employee.DaysLeft - count;
                        employee.DaysLeft = result;
                        _employeeRepository.Update(employee);

                        var entity = new Permission()
                        {
                            StartDate = permission.StartDate,
                            EndDate = permission.EndDate,
                            DayCount = count,
                            EmployeeId = (int)id,
                        };

                        _permissionRepository.Create(entity);

                        var ms = new AlertMessage()
                        {
                            Message = $"{employee.Name} {employee.Surname} isimli personelin izni eklendi",
                            AlertType = "success"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(ms);
                        return RedirectToAction("Index");
                    }
                }
            }
            
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _permissionRepository.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new Permission()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };

            ViewBag.Name = _employeeRepository.GetById(entity.EmployeeId).Name;
            ViewBag.Surname = _employeeRepository.GetById(entity.EmployeeId).Surname;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Permission permission)
        {
            var entity = _permissionRepository.GetById(permission.Id);

            var model = new Permission()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };

            ViewBag.Name = _employeeRepository.GetById(entity.EmployeeId).Name;
            ViewBag.Surname = _employeeRepository.GetById(entity.EmployeeId).Surname;

            if (ModelState.IsValid)
            {
                if (permission.StartDate > permission.EndDate)
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"Bitiş tarihi başlangıç tarihinden büyük olamaz.",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
                else if (permission.StartDate < DateTime.Now.Date)
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"Geriye dönük izin alamazsınız.",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
                else
                {
                    int count = (permission.EndDate - permission.StartDate).Days;
                    var employee = _employeeRepository.GetById(entity.EmployeeId);

                    if (count > employee.AnnualLeave)
                    {
                        var msg = new AlertMessage()
                        {
                            Message = $"İzin aldığınız gün sayısı, izin hakkınızdan fazla olamaz.",
                            AlertType = "danger"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);
                    }
                    else
                    {
                        List<Employee> employees = _employeeRepository.Get(employee.DepartmentId);
                        foreach (var e in employees)
                        {
                            List<Permission> permissions = _permissionRepository.ListById(e.Id);
                            foreach (var p in permissions)
                            {
                                if (p.StartDate >= permission.StartDate && p.StartDate < permission.EndDate)
                                {
                                    var ms = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(ms);
                                    return View(model);
                                }
                                else if (permission.StartDate >= p.StartDate && permission.EndDate < p.EndDate)
                                {
                                    var ms = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(ms);

                                    return View(model);
                                }
                                else if (permission.StartDate >= p.StartDate && permission.StartDate < p.EndDate)
                                {
                                    var ms = new AlertMessage()
                                    {
                                        Message = $"Bu tarihler arasında, aynı departmanda bir personel izinlidir.",
                                        AlertType = "danger"
                                    };
                                    TempData["message"] = JsonConvert.SerializeObject(ms);

                                    return View(model);
                                }
                            }

                        }

                        if (entity == null)
                        {
                            return NotFound();
                        }

                        int result = employee.DaysLeft - count;
                        employee.DaysLeft = result;
                        _employeeRepository.Update(employee);

                        entity.StartDate = permission.StartDate;
                        entity.EndDate = permission.EndDate;
                        entity.DayCount = count;
                        _permissionRepository.Update(entity);

                        var msg = new AlertMessage()
                        {
                            Message = $"{employee.Name} {employee.Surname} isimli personelin izni güncellendi",
                            AlertType = "warning"
                        };
                        TempData["message"] = JsonConvert.SerializeObject(msg);

                        return RedirectToAction("Index");
                    }

                }
            }

            return View(model);            
        }

        public IActionResult Delete(int id)
        {
            var entity = _permissionRepository.GetById(id);

            if (entity != null)
            {
                _permissionRepository.Delete(entity);
            }

            var employee = _employeeRepository.GetById(entity.EmployeeId);
            var msg = new AlertMessage()
            {
                Message = $"{employee.Name} {employee.Surname} isimli personelin izni silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            
            employee.DaysLeft = employee.DaysLeft+entity.DayCount;
            _employeeRepository.Update(employee);

            return RedirectToAction("Index");
        }

    }
}