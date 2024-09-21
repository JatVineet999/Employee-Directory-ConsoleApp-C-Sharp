using Presentation.Constants;
using Presentation.Interfaces;

namespace Presentation.MenuManagers
{
    class MainMenuManager : IMainMenuManager
    {
        private readonly Lazy<IEmployeesMenuManager> _employeesMenuManager;
        private readonly Lazy<IRolesMenuManager> _rolesMenuManager;

        public MainMenuManager(Lazy<IEmployeesMenuManager> employeesMenuManager, Lazy<IRolesMenuManager> rolesMenuManager)
        {
            _employeesMenuManager = employeesMenuManager ?? throw new ArgumentNullException(nameof(employeesMenuManager));
            _rolesMenuManager = rolesMenuManager ?? throw new ArgumentNullException(nameof(rolesMenuManager));
        }

        public void DisplayMainMenu()
        {
            var menuActions = new Dictionary<MainMenuOption, Action>
            {
                { MainMenuOption.EmployeesMenu, () => _employeesMenuManager.Value.EmployeesMenuHandler() },
                { MainMenuOption.RolesMenu, () => _rolesMenuManager.Value.DisplayRolesMenu() },
                { MainMenuOption.Exit, () => Environment.Exit(0) }
            };

            Console.WriteLine("--------------------------------------------------------------Main Menu-------------------------------------------------------------------------");
            Console.WriteLine("1. Employees Menu");
            Console.WriteLine("2. Roles Menu");
            Console.WriteLine("3. Exit");

            string userInput = Console.ReadLine()!.Trim();
            MainMenuOption selectedOption;

            if (Enum.TryParse(userInput, out selectedOption) && menuActions.ContainsKey(selectedOption))
            {
                menuActions[selectedOption].Invoke();
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
                DisplayMainMenu();
            }
        }
    }
}
