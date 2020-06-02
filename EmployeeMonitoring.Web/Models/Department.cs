using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.Models
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(50,ErrorMessage ="Departman ismi en fazla 50 karakter olabilir.")]
        [Required(ErrorMessage ="Departman ismi alanı zorunludur.")]
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
