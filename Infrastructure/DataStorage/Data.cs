using Infrastructure.Models;

namespace Infrastructure.DataStorage
{
    public class Data
    {
        public List<Employee>? EmployeeRecords { get; set; }
        public List<Department>? DepartmentsAndRoles { get; set; }
    }
}
