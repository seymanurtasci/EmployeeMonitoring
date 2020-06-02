using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMonitoring.Web.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Departman ismi en fazla 50 karakter olabilir.")]
        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Departman ismi en fazla 50 karakter olabilir.")]
        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]
        public string Surname { get; set; }

        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public int AnnualLeave { get; set; }
        public int DaysLeft { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public List<Permission> Permissions { get; set; }

    }
}
