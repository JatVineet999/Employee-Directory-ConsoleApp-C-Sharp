using Infrastructure.Models;
using Application.Interfaces;
using Presentation.Constants;
using Presentation.Interfaces;

namespace Presentation.MenuManagers
{
    class RolesMenuManager : IRolesMenuManager
    {
        private readonly IMainMenuManager _mainMenuManager;
        private readonly IDepartmentAndRolesServices _DepartmentAndRolesServices;
        private readonly Dictionary<RolesMenuOption, Action> _menuActions;

        public RolesMenuManager(IMainMenuManager mainMenuManager, IDepartmentAndRolesServices DepartmentAndRolesServices)
        {
            _DepartmentAndRolesServices = DepartmentAndRolesServices;
            _mainMenuManager = mainMenuManager;
            _menuActions = new Dictionary<RolesMenuOption, Action>
            {
                { RolesMenuOption.AddRole, AddRole },
                { RolesMenuOption.DisplayRoles, DisplayRoles },
                { RolesMenuOption.ReturnToMainMenu,_mainMenuManager.DisplayMainMenu}

            };
        }

        public void DisplayRolesMenu()
        {
            while (true)
            {
                DisplayMenuOptions();
                if (!GetUserChoice(out RolesMenuOption selectedOption))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                if (_menuActions.ContainsKey(selectedOption))
                {
                    _menuActions[selectedOption].Invoke();
                }
                else
                {
                    Console.WriteLine("Invalid option selected. Please try again.");
                }

                if (!RequestMenuReturnSelection())
                {
                    return;
                }
            }
        }

        private void DisplayMenuOptions()
        {
            Console.WriteLine("--------------------------------------------------------------Roles Menu-------------------------------------------------------------------------");
            Console.WriteLine($"1.AddRole");
            Console.WriteLine($"2.DisplayRoles");
            Console.WriteLine($"Press '0' to Go Back to Menu");
        }

        private bool GetUserChoice(out RolesMenuOption selectedOption)
        {
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return Enum.TryParse(choice.ToString(), out selectedOption);
        }

        private bool RequestMenuReturnSelection()
        {
            Console.WriteLine("Press '0' to Go Back to Previous Menu options\n                 or\nPress any other key to return to Main Menu");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (input == '0')
            {
                return true;
            }
            else
            {
                _mainMenuManager.DisplayMainMenu();
                return false;
            }
        }

        public Department SelectDepartment()
        {
            string?[] departmentNames = _DepartmentAndRolesServices.ExtractDepartmentNames();
            Console.WriteLine("Available Departments:");
            for (int i = 0; i < departmentNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {departmentNames[i]}");
            }
            int departmentIndex = GetUserInput("Select a department by entering its number:", departmentNames.Length);
            string selectedDepartmentName = departmentNames[departmentIndex - 1]!;
            Department selectedDepartment = _DepartmentAndRolesServices.GetDepartmentByName(selectedDepartmentName);
            return selectedDepartment;
        }
        public string SelectRole(string departmentName)
        {
            Department selectedDepartment = _DepartmentAndRolesServices.GetDepartmentByName(departmentName);
            Console.WriteLine($"Roles in {selectedDepartment.Name} department:");
            for (int i = 0; i < selectedDepartment.Roles!.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {selectedDepartment.Roles[i]}");
            }
            int roleIndex = GetUserInput("Select a role by entering its number:", selectedDepartment.Roles.Length);
            return selectedDepartment.Roles[roleIndex - 1];
        }

        private int GetUserInput(string prompt, int maxInput)
        {
            int userInput;
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out userInput) && userInput >= 1 && userInput <= maxInput)
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine($"Invalid input. Please enter a valid number between 1 and {maxInput}.");
                }
            }
        }

        private void AddRole()
        {
            Console.WriteLine("Select Department in which you would like to add a new role:");
            Department selectedDepartment = SelectDepartment();

            if (selectedDepartment == null)
            {
                Console.WriteLine("Invalid department selected. Please try again.");
                return;
            }
            Console.Write("Enter the new role: ");
            string newRole = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(newRole))
            {
                Console.WriteLine("Invalid role name. Role name cannot be empty.");
                return;
            }

            if (selectedDepartment.Roles != null && selectedDepartment.Roles.Contains(newRole))
            {
                Console.WriteLine($"The role '{newRole}' already exists in the '{selectedDepartment.Name}' department.");
                return;
            }

            _DepartmentAndRolesServices.AddRoleToDepartment(selectedDepartment, newRole);
            Console.WriteLine($"Role '{newRole}' added to the '{selectedDepartment.Name}' department.");
        }


        private void DisplayRoles()
        {
            var departmentsAndRoles = _DepartmentAndRolesServices.GetDepartmentsAndRoles();
            if (departmentsAndRoles != null)
            {
                Console.WriteLine("Available Roles:");
                foreach (var department in departmentsAndRoles)
                {
                    Console.WriteLine($"Department: {department.Name}");
                    foreach (var role in department.Roles!)
                    {
                        Console.WriteLine($"- {role}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve department and roles data.");
            }
        }


    }
}
