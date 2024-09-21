using Infrastructure.Interfaces;
using Infrastructure.Models;
namespace Infrastructure.Repos
{
    public class DepartmentsRepo : BaseRepo, IDepartmentsRepo
    {
        public void SaveDepartmentsAndRoles(List<Department> departments)
        {
            var data = LoadData();
            data.DepartmentsAndRoles = departments;
            SaveData(data);
        }
        public Department GetDepartmentByName(string departmentName)
        {
            var departments = LoadDepartmentsAndRoles();
            return departments.FirstOrDefault(d => d.Name == departmentName)!;
        }

        public void UpdateDepartment(Department departmentToUpdate, string newRole)
        {
            var departments = LoadDepartmentsAndRoles();
            
            //updating role in departmentTOUpdate
            var updatedRoles = departmentToUpdate.Roles != null ? departmentToUpdate.Roles.ToList() : new List<string>();
            updatedRoles.Add(newRole);
            departmentToUpdate.Roles = updatedRoles.ToArray();

           //saving updated role in existing department's role
            var existingDepartment = departments.FirstOrDefault(d => d.Name == departmentToUpdate.Name);
            if (existingDepartment != null)
            {
                existingDepartment.Roles = departmentToUpdate.Roles;
                SaveDepartmentsAndRoles(departments);
            }
        }
        public List<Department> LoadDepartmentsAndRoles()
        {
            return LoadData().DepartmentsAndRoles ?? new List<Department>();
        }
    }
}
