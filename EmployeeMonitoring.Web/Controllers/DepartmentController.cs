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
    public class DepartmentController : Controller
    {
        private IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            List<Department> departments = _departmentRepository.GetAll();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var entity = new Department()
                {
                    Name = department.Name,
                };

                _departmentRepository.Create(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} departmanı eklendi",
                    AlertType = "success"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            }
            return View(department);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _departmentRepository.GetById((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new Department()
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                var entity = _departmentRepository.GetById(department.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = department.Name;
                _departmentRepository.Update(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} departmanı güncellendi",
                    AlertType = "warning"
                };
                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Index");
            }
            return View(department);
        }

        public IActionResult Delete(int id)
        {
            var entity = _departmentRepository.GetById(id);

            if (entity != null)
            {
                _departmentRepository.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} departmanı silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("Index");
        }
    }
}