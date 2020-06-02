using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMonitoring.Web.DataAccess.Abstract;
using EmployeeMonitoring.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace EmployeeMonitoring.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
           

            int annual_leave = 0;
            foreach (var item in _employeeRepository.GetAll())
            {
                if (item.StartDate.Day==DateTime.Now.Day && item.StartDate.Month==DateTime.Now.Month && item.StartDate.Year<DateTime.Now.Year)
                {
                    annual_leave = Calculate(item.StartDate);
                    item.AnnualLeave = annual_leave;
                    item.DaysLeft = annual_leave;
                    _employeeRepository.Update(item);
                }                
            }
        }

        public IActionResult Index()
        {
            List<Employee> employees = _employeeRepository.List();
            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                int annual_leave = Calculate(employee.StartDate);

                var entity = new Employee()
                {
                    Name = employee.Name,
                    Surname = employee.Surname,
                    StartDate = employee.StartDate,
                    DepartmentId = employee.DepartmentId,
                    AnnualLeave = annual_leave,
                    DaysLeft = annual_leave
                };

                _employeeRepository.Create(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} {entity.Surname} isimli personel eklendi",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Name");
            return View(employee);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _employeeRepository.GetById((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new Employee()
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname =entity.Surname,
                StartDate = entity.StartDate,
                DepartmentId = entity.DepartmentId
            };
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            var entity = _employeeRepository.GetById(employee.Id);

            if (ModelState.IsValid)
            {                
                if (entity == null)
                {
                    return NotFound();
                }

                int annual_leave = Calculate(employee.StartDate);

                if (entity.AnnualLeave != annual_leave)
                {
                    int difference = annual_leave - entity.AnnualLeave;
                    entity.DaysLeft = difference + entity.DaysLeft;
                }

                entity.Name = employee.Name;
                entity.Surname = employee.Surname;
                entity.StartDate = employee.StartDate;
                entity.DepartmentId = employee.DepartmentId;
                entity.AnnualLeave = annual_leave;

                _employeeRepository.Update(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} {entity.Surname} isimli personel güncellendi",
                    AlertType = "warning"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            }

            var model = new Employee()
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                StartDate = entity.StartDate,
                DepartmentId = entity.DepartmentId
            };
            ViewBag.DepartmentId = new SelectList(_departmentRepository.GetAll(), "Id", "Name");
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var entity = _employeeRepository.GetById(id);

            if (entity != null)
            {
                _employeeRepository.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} {entity.Surname} isimli personel silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("Index");
        }

        public int Calculate(DateTime startDate)
        {

            DateTime date = DateTime.Now.Date;
            int result = (date - startDate).Days / 365;
            int annual_leave = 0;
            if (result >= 1 && result < 5)
            {
                annual_leave = 14;
            }
            else if (result >= 5)
            {
                annual_leave = 20;
            }

            return annual_leave;
        }

    }
}