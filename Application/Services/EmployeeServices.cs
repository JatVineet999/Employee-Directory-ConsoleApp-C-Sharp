using System.Reflection;
using Infrastructure.Models;
using Application.Interfaces;
using Infrastructure.Interfaces;
namespace Application.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepo _employeeRepo;
        public EmployeeServices(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        private string GenerateEmployeeNumber()
        {
            Random rand = new Random();
            return $"TZ{rand.Next(1000, 10000)}";
        }

        public bool AddEmployeeRecord(Employee employee)
        {
            employee.EmployeeNumber = GenerateEmployeeNumber();
            bool isAdded = _employeeRepo.AddEmployee(employee);
            // Returning result
            return isAdded;
        }

        public Employee? GetEmployeeByEmployeeNumber(string employeeNumber)
        {
            var employeeRecords = _employeeRepo.LoadEmployeeRecords();
            if (employeeRecords != null)
            {
                return _employeeRepo.SearchEmployee(employeeRecords, employeeNumber);
            }
            return null;
        }

        public void SaveEmployeeData(Employee employeeToUpdate, string input, string propertyType)
        {
            var employeeRecords = _employeeRepo.LoadEmployeeRecords();
            if (employeeRecords != null)
            {
                PropertyInfo property = typeof(Employee).GetProperty(propertyType)!;
                if (property != null)
                {
                    // Converting the input to the property type whose info is to be updated
                    object? value = Convert.ChangeType(input, property.PropertyType);

                    // Setting the value of the specified property for the employee
                    property.SetValue(employeeToUpdate, value);
                    _employeeRepo.UpdateEmployee(employeeToUpdate, employeeRecords);
                }
            }
        }


        public List<Employee> GetEmployeeRecords()
        {
            return _employeeRepo.LoadEmployeeRecords();
        }

        public bool DeleteEmployee(string employeeNumberToDelete)
        {
            var employee = GetEmployeeByEmployeeNumber(employeeNumberToDelete);

            if (employee != null)
            {
                _employeeRepo.RemoveEmployee(employee);
                return true;
            }
            return false;
        }



    }
}
