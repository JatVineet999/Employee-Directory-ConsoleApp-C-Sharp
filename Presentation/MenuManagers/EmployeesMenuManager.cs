using Infrastructure.Models;
using Application.Constants;
using Application.Interfaces;
using Presentation.Interfaces;
using Presentation.Constants;

namespace Presentation.MenuManagers
{
    class EmployeesMenuManager : IEmployeesMenuManager
    {
        private readonly IMainMenuManager _mainMenuManager;
        private readonly IInputReader _InputReader;
        private readonly IRolesMenuManager _rolesMenuManager;
        private readonly Dictionary<EmployeesMenuOption, Action> _menuActions;
        private readonly IEmployeeServices _employeeServices;

        public EmployeesMenuManager(IMainMenuManager mainMenuManager, IRolesMenuManager rolesMenuManager, IInputReader InputReader, IEmployeeServices employeeServices)
        {
            _InputReader = InputReader;
            _mainMenuManager = mainMenuManager;
            _rolesMenuManager = rolesMenuManager;
            _employeeServices = employeeServices;
            _menuActions = new Dictionary<EmployeesMenuOption, Action>
            {
                { EmployeesMenuOption.ViewEmployees, ViewEmployees },
                { EmployeesMenuOption.AddEmployee, AddEmployee },
                { EmployeesMenuOption.UpdateEmployee, UpdateEmployee },
                { EmployeesMenuOption.DisplayOne, DisplayEmployeeByNumber },
                { EmployeesMenuOption.DeleteEmployee, DeleteEmployee },
                { EmployeesMenuOption.ReturnToMainMenu, _mainMenuManager.DisplayMainMenu }
            };
        }
        public void EmployeesMenuHandler()
        {
            while (true)
            {
                DisplayMenuOptions();

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                if (Enum.TryParse(choice.ToString(), out EmployeesMenuOption selectedOption)
                    && _menuActions.TryGetValue(selectedOption, out Action? option))
                {
                    option.Invoke();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }

                if (!RequestMenuReturnSelection())
                {
                    _mainMenuManager.DisplayMainMenu();
                    return;
                }
            }
        }
        private void DisplayMenuOptions()
        {
            Console.WriteLine("--------------------------------------------------------------Employees Menu-------------------------------------------------------------------------");
            Console.WriteLine("1. View Employees");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. View Particular Employee Record");
            Console.WriteLine("5. Delete Employee\n");
            Console.WriteLine("Press '0' to Go Back to Previous Menu\n");
        }

        private bool RequestMenuReturnSelection()
        {
            Console.WriteLine("Press '0' to Go Back to Previous Menu options\n                 or\nPress any other key to return to Main Menu");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            return input == '0';
        }
        private void AddEmployee()
        {
            Employee newEmployee = _GatherEmployeeDetails();

            if (_employeeServices.AddEmployeeRecord(newEmployee))
            {
                Console.WriteLine("Employee Added Successfully");
            }
            else
            {
                Console.WriteLine("An error occurred while adding the employee record");
            }
        }

        private void ViewEmployees()
        {
            var employeeRecords = _employeeServices.GetEmployeeRecords();
            PrintEmployeeRecordsHeader();
            foreach (var employee in employeeRecords)
            {
                DisplayEmployeeRecord(employee);
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        }
        private void DisplayEmployeeByNumber()
        {
            ViewEmployees();
            Console.WriteLine("Enter Employee Number Of Employee to see only his Record:");
            string employeeNumber = Console.ReadLine()!;
            var employee = _employeeServices.GetEmployeeByEmployeeNumber(employeeNumber);
            if (employee != null)
            {
                PrintEmployeeRecordsHeader();
                DisplayEmployeeRecord(employee);
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("Employee with the provided Employee Number does not exist !!!\n");
            }
        }

        private void PrintEmployeeRecordsHeader()
        {
            Console.WriteLine("Employee Records:");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Employee Number | Name                               | Joining Date | Location       | Job Title                     | Department           | Manager Name   | Project Name               |");
            Console.WriteLine("|-----------------|------------------------------------|--------------|----------------|------------------------------ |----------------------|----------------|----------------------------|");
        }

        private void DisplayEmployeeRecord(Employee employee)
        {
            try
            {
                Console.WriteLine($"| {employee.EmployeeNumber,-15} | {employee.FirstName,-18} {employee.LastName,-15} | {employee.JoiningDate.ToString("yyyy-MM-dd"),-12} | {employee.Location,-14} | {employee.JobTitle,-29} | {employee.Department,-20} | {employee.ManagerName,-14} | {employee.ProjectName,-26} |");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying employee record: {ex.Message}, Please Enter valid Employee Number!!!");
            }
        }

        private void UpdateEmployee()
        {
            Console.WriteLine("Update Employee");
            Console.WriteLine("Please select an employee to update:");
            ViewEmployees();
            Console.Write("Enter Employee Number to update: ");
            string employeeNumberToUpdate = Console.ReadLine()!;
            Employee employeeToUpdate = _employeeServices.GetEmployeeByEmployeeNumber(employeeNumberToUpdate)!;
            if (employeeToUpdate != null)
            {
                UpdateEmployeeDetails(employeeToUpdate);
            }
            else
            {
                Console.WriteLine("Employee not found!!!");
            }
        }
        private string _userInput = "";
        private string _propertyType = "";
        private void UpdateEmployeeDetails(Employee employeeToUpdate)
        {
            Console.WriteLine($"Updating employee: {employeeToUpdate.FirstName} {employeeToUpdate.LastName}");
            Console.WriteLine("Select which information you would like to update:");
            Console.WriteLine("1. First Name");
            Console.WriteLine("2. Last Name");
            Console.WriteLine("3. Mobile Number");
            Console.WriteLine("4. Email");
            Console.WriteLine("5. DateOfBirth");
            Console.WriteLine("6. Joining Date");
            Console.WriteLine("7. Location");
            Console.WriteLine("8. Job Title");
            Console.WriteLine("9. Department");
            Console.WriteLine("10. Manager Name");
            Console.WriteLine("11. Project Name");
            do
            {
                Console.Write("Enter your choice: ");
                string choiceInput = Console.ReadLine()!;
                if (int.TryParse(choiceInput, out int choice))
                {
                    Console.WriteLine();
                    switch (choice)
                    {
                        case 1:
                            _userInput = _InputReader.GetValidInput("Enter new First Name: ", ValidationType.Name);
                            _propertyType = "FirstName";
                            break;
                        case 2:
                            _userInput = _InputReader.GetValidInput("Enter new Last Name: ", ValidationType.Name);
                            _propertyType = "LastName";
                            break;
                        case 3:
                            _userInput = _InputReader.GetValidInput("Enter new Mobile Number: ", ValidationType.MobileNumber);
                            _propertyType = "MobileNumber";
                            break;
                        case 4:
                            _userInput = _InputReader.GetValidInput("Enter new Email: ", ValidationType.Email);
                            _propertyType = "Email";
                            break;
                        case 5:
                            _userInput = _InputReader.GetValidInput("Enter correct Date of Birth (yyyy-mm-dd): ", ValidationType.Date);
                            _propertyType = "DateOfBirth";
                            break;
                        case 6:
                            _userInput = _InputReader.GetValidInput("Enter correct Joining Date (yyyy-mm-dd): ", ValidationType.Date);
                            _propertyType = "JoiningDate";
                            break;
                        case 7:
                            _userInput = _InputReader.GetValidInput("Enter new Location: ", ValidationType.Name);
                            _propertyType = "Location";
                            break;
                        case 8:
                            _userInput = _rolesMenuManager.SelectRole(employeeToUpdate.Department!);
                            _propertyType = "JobTitle";
                            break;
                        case 9:
                            _userInput = _rolesMenuManager.SelectDepartment().Name!;
                            _propertyType = "Department";
                            _employeeServices.SaveEmployeeData(employeeToUpdate, _userInput, _propertyType);
                            goto case 8;
                        case 10:
                            _userInput = _InputReader.GetValidInput("Enter new Manager Name: ", ValidationType.Name);
                            _propertyType = "ManagerName";
                            break;
                        case 11:
                            _userInput = _InputReader.GetValidInput("Enter new Project Name: ", ValidationType.Name);
                            _propertyType = "ProjectName";
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                if (_userInput.Length > 0)
                {
                    Console.WriteLine("Employee information updated successfully.");
                    _employeeServices.SaveEmployeeData(employeeToUpdate, _userInput, _propertyType);
                }


                Console.Write("Do you want to update another detail? (Y/N): ");
                char continueChoice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
                if (continueChoice == 'N')
                {
                    break;
                }

            } while (true);
        }
        private Employee _GatherEmployeeDetails()
        {
            Employee employee = new Employee();

            var detailsToCollect = new List<(string prompt, Func<string> inputProvider, string _propertyType)>
            {
              ("First Name", () => _InputReader.GetValidInput("Enter First Name: ", ValidationType.Name), "FirstName"),
              ("Last Name", () => _InputReader.GetValidInput("Enter Last Name: ", ValidationType.Name), "LastName"),
              ("Mobile Number", () => _InputReader.GetValidInput("Enter Mobile Number: ", ValidationType.MobileNumber), "MobileNumber"),
              ("Email", () => _InputReader.GetValidInput("Enter Email: ", ValidationType.Email), "Email"),
              ("Date of Birth (yyyy-mm-dd)", () => _InputReader.GetValidInput("Enter Date of Birth (yyyy-mm-dd): ", ValidationType.Date), "DateOfBirth"),
              ("Joining Date (yyyy-mm-dd)", () => _InputReader.GetValidInput("Enter Joining Date (yyyy-mm-dd): ", ValidationType.Date), "JoiningDate"),
              ("Location", () => _InputReader.GetValidInput("Enter Location: ", ValidationType.Name), "Location"),
              ("Department", () => _rolesMenuManager.SelectDepartment().Name!, "Department"),
              ("Job Title", () => _rolesMenuManager.SelectRole(employee.Department!), "JobTitle"),
              ("ManagerName", () => _InputReader.GetValidInput("Enter Manager Name: ", ValidationType.Name), "ManagerName"),
              ("ProjectName", () => _InputReader.GetValidInput("Enter Project Name: ", ValidationType.Name), "ProjectName")
          };

            foreach (var (prompt, inputProvider, _propertyType) in detailsToCollect)
            {
                _userInput = inputProvider.Invoke();
                _employeeServices.SaveEmployeeData(employee, _userInput, _propertyType);
            }

            return employee;
        }

        private void DeleteEmployee()
        {
            ViewEmployees();
            Console.Write("Enter the Employee Number of the employee to delete record: ");
            string employeeNumberToDelete = Console.ReadLine()!;
            bool deletedSuccessfully = _employeeServices.DeleteEmployee(employeeNumberToDelete);
            if (deletedSuccessfully)
            {
                Console.WriteLine($"Employee with Employee Number {employeeNumberToDelete} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Employee not found!!!");
            }
        }

    }
}
