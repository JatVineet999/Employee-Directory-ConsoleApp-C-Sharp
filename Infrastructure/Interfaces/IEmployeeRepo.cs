using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IEmployeeRepo
    {
        List<Employee> LoadEmployeeRecords();
        Employee? SearchEmployee(List<Employee> employeeRecords, string employeeNumber);
        bool AddEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
        void UpdateEmployee(Employee updatedEmployee, List<Employee> employeeRecords);
    }
}
