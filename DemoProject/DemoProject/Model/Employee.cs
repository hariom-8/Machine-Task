using System.ComponentModel.DataAnnotations;

namespace DemoProject.Model
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ManagerId { get; set; }
        public int? EmployeeSalary { get; set; }
    }
}
