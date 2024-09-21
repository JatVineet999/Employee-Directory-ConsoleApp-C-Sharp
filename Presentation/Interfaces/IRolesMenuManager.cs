using Infrastructure.Models;

namespace Presentation.Interfaces
{
    interface IRolesMenuManager
    {
        void DisplayRolesMenu();
        Department SelectDepartment();
        string SelectRole(string departmentName);
    }
}
