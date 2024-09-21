using Infrastructure.Models;
namespace Application.Interfaces
{
    public interface IEmployeeServices
    {
        bool AddEmployeeRecord(Employee employee);
        Employee? GetEmployeeByEmployeeNumber(string employeeNumber);
        void SaveEmployeeData(Employee employeeToUpdate, string input, string propertyType);
        List<Employee> GetEmployeeRecords();
        bool DeleteEmployee(string employeeNumberToDelete);
    }
}
