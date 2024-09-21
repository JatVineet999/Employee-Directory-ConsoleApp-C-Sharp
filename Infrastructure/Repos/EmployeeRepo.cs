using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Repos
{
    public class EmployeeRepo : BaseRepo, IEmployeeRepo
    {
        private void SaveEmployeeRecords(List<Employee> employees)
        {
            var data = LoadData();
            data.EmployeeRecords = employees;
            SaveData(data);
        }

        public List<Employee> LoadEmployeeRecords()
        {
            return LoadData().EmployeeRecords ?? new List<Employee>();
        }

        public bool AddEmployee(Employee employee)
        {
            try
            {
                var employees = LoadEmployeeRecords();
                employees.Add(employee);
                SaveEmployeeRecords(employees);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Employee? SearchEmployee(List<Employee> employeeRecords, string employeeNumber)
        {
            return employeeRecords.FirstOrDefault(e => e.EmployeeNumber == employeeNumber);
        }


        public void RemoveEmployee(Employee employee)
        {
            var employees = LoadEmployeeRecords();
            if (employees != null)
            {
                int index = employees.FindIndex(e => e.EmployeeNumber == employee.EmployeeNumber);
                if (index != -1)
                {
                    employees.RemoveAt(index);
                    SaveEmployeeRecords(employees);
                }
            }
        }
        public void UpdateEmployee(Employee updatedEmployee, List<Employee> employeeRecords)
        {
            int index = employeeRecords.FindIndex(e => e.EmployeeNumber == updatedEmployee.EmployeeNumber);
            if (index != -1)
            {
                employeeRecords[index] = updatedEmployee;
                SaveEmployeeRecords(employeeRecords);
            }

        }


    }
}
