using System;
using System.Collections.Generic;
namespace FastGuard.Models
{
    public class Employee : ApplicationUser
    {
        public float? EmployeeSalary { get; set; }
    }
}
