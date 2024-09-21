using Infrastructure.Models;
using Application.Interfaces;
using Infrastructure.Interfaces;
namespace Application.Services
{
    public class DepartmentAndRolesServices : IDepartmentAndRolesServices
    {
        private readonly IDepartmentsRepo _departmentsRepo;
        public DepartmentAndRolesServices(IDepartmentsRepo departmentsRepo)
        {
            _departmentsRepo = departmentsRepo;
        }

        public Department GetDepartmentByName(string departmentName)
        {
            var departments = _departmentsRepo.LoadDepartmentsAndRoles();
            foreach (var department in departments)
            {
                if (department.Name == departmentName)
                {
                    return department;
                }
            }
            throw new ArgumentException("Department not found", nameof(departmentName));
        }
        public string?[] ExtractDepartmentNames()
        {
            var departments = _departmentsRepo.LoadDepartmentsAndRoles();
            return departments?
                .Select(department => department.Name)
                .ToArray() ?? new string?[0];
        }

        public void AddRoleToDepartment(Department selectedDepartment, string newRole)
        {
            var departmentToUpdate = _departmentsRepo.GetDepartmentByName(selectedDepartment.Name!);
            if (departmentToUpdate != null)
            {
                _departmentsRepo.UpdateDepartment(departmentToUpdate, newRole);
            }
        }

        public Department[]? GetDepartmentsAndRoles()
        {
            var departmentsList = _departmentsRepo.LoadDepartmentsAndRoles();
            if (departmentsList != null)
            {
                return departmentsList.ToArray();
            }
            return null;
        }

    }



}