using Infrastructure.Models;

namespace Application.Interfaces
{
    public interface IDepartmentAndRolesServices
    {
        Department GetDepartmentByName(string departmentName);
        string?[] ExtractDepartmentNames();
        void AddRoleToDepartment(Department selectedDepartment, string newRole);
        Department[]? GetDepartmentsAndRoles();
    }
}
