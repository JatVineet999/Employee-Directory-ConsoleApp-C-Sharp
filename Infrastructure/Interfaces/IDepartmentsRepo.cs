using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IDepartmentsRepo
    {
        void SaveDepartmentsAndRoles(List<Department> departments);
        List<Department> LoadDepartmentsAndRoles();
        Department GetDepartmentByName(string departmentName);
        void UpdateDepartment(Department departmentToUpdate, string newRole);
    }
}
