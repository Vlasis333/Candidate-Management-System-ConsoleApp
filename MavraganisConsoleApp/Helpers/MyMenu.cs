using EasyConsole;
using System;
using EFDataAccess.Services.AdminsService;
using EFDataAccess.Services.CandidatesUI;
using EFDataAccess.Services;

namespace MavraganisConsoleApp.Helpers
{
    internal class MyMenu
    {
        private int _loggedInUser;
        private int _candidateId; 

        public MyMenu()
        {
            Output.WriteLine(ConsoleColor.Green, "Welcome Visitor, please select your corresponding category.");
            MainMenuInitializer();
        }

        /// <summary>
        /// All the methods taht will run our application
        /// </summary>
        private void MainMenuInitializer()
        {
            Console.WriteLine();
            var myMenu = new Menu() // Main menus for the user to select the his/her profession
            .Add("Admin's Service", () => AdminsServicePage())
            .Add("Candidate's UI", () => CandidatesUIPage())
            .Add("CLS and Log Out", () => ClearConsoleWindow())
            .Add("Exit", () => Environment.Exit(0));
            myMenu.Display();
        }

        /// <summary>
        /// we use that to return to the previous menu (after the user made a choice) - based on logged in user Candidate or Admin
        /// </summary>
        private void ReturnToPreviousMenu()
        {
            Console.WriteLine();
            if (_loggedInUser == 0) // Case admin
            {
                AdminsServicePage();
            }
            else if (_loggedInUser == 1) // Else is candidate
            {
                Console.WriteLine(); // We dont want to ask to insert his Id again (we keep him logged in until he chooses to go back)
                var candidatesUIPage = new Menu()
                .Add("Show my Certificates", () => CandidatesMain.ShowCertificatesOfCandidate(_candidateId))
                .Add("Download my Certificates", () => CandidatesMain.DownloadCertificatesOfCandidate(_candidateId))
                .Add("Move Back", () => MainMenuInitializer());
                candidatesUIPage.Display();

                ReturnToPreviousMenu();
            }
        }

        /// <summary>
        /// Clears the console window and takes user to the main view
        /// </summary>
        private void ClearConsoleWindow()
        {
            Console.Clear();
            Output.WriteLine(ConsoleColor.Green, "Welcome Visitor, please select your corresponding category.");
            MainMenuInitializer();
        }

        #region Admin's Serice
        /// <summary>
        /// All main routines for the Admin's service
        /// </summary>
        private void AdminsServicePage()
        {
            _loggedInUser = 0;

            Console.WriteLine();
            var adminsServicePage = new Menu()
            .Add("Add new Candidate", () => AdminsMain.AddNewCandidate())
            .Add("Update Candidate", () => UpdateCandidate())
            .Add("Delete Candidate", () => DeleteCandidate())
            .Add("Show Candidates' Information", () => AdminsMain.ShowCandidatesInfo())
            .Add("Show Candidates' Results", () => AdminsMain.ShowCandidatesResults())
            .Add("Move Back", () => MainMenuInitializer());
            adminsServicePage.Display();

            ReturnToPreviousMenu();
        }

        /// <summary>
        /// Routine to check if candidate exists before executing update algorithm
        /// </summary>
        private void UpdateCandidate()
        {
            AskForIdFromUser("Please provide us with the Candidate's Id that you want to update.", "Candidate's Id: ");

            var input = Console.ReadLine();
            input = CheckIfInt(input);

            int candidateId = int.Parse(input);

            CheckIfCandidateExists(ref input, ref candidateId);

            Console.WriteLine();

            AdminsMain.UpdateCandidate(candidateId);
        }

        /// <summary>
        /// Routine to check if candidate exists before executing delete algorithm
        /// </summary>
        private void DeleteCandidate()
        {
            AskForIdFromUser("Please provide us the Id of the Candidate that will be deleted.", "Candidate's Id: ");

            var input = Console.ReadLine();
            input = CheckIfInt(input);

            int candidateId = int.Parse(input);

            // Checks if the given Id exists before proceeding
            CheckIfCandidateExists(ref input, ref candidateId);

            Console.WriteLine();

            AdminsMain.DeleteCandidate(candidateId);
        }
        #endregion

        #region Candidate's UI

        /// <summary>
        /// All main routines for the Candidate's UI
        /// </summary>
        private void CandidatesUIPage()
        {
            _loggedInUser = 1;

            AskForIdFromUser("Please provide us with your personal number to Identify you.", "Your Id: ");

            var input = Console.ReadLine();
            input = CheckIfInt(input);

            int candidateId = int.Parse(input);

            // Checks if the given Id exists before proceeding
            CheckIfCandidateExists(ref input, ref candidateId, true);

            Console.WriteLine();

            _candidateId = candidateId; // private field is set here for future use

            var candidatesUIPage = new Menu()
            .Add("Show my Certificates", () => CandidatesMain.ShowCertificatesOfCandidate(candidateId))
            .Add("Download my Certificates", () => CandidatesMain.DownloadCertificatesOfCandidate(candidateId))
            .Add("Move Back", () => MainMenuInitializer());
            candidatesUIPage.Display();

            ReturnToPreviousMenu();
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Checks if the given string from the user input is an integer
        /// </summary>
        private static string CheckIfInt(string input)
        {
            // Only moves forward in case user provides us with integer
            while (!int.TryParse(input, out int _))
            {
                Console.Write("Please provide us with an integer: ");
                input = Console.ReadLine();
            }

            return input;
        }

        /// <summary>
        /// Console promp to ask a Id from the user
        /// </summary>
        private static void AskForIdFromUser(string message, string idMessage)
        {
            Console.WriteLine();
            Output.WriteLine(ConsoleColor.Green, message);
            Console.Write(idMessage);
        }

        /// <summary>
        /// Routine to check if the given Id exists in the database as a candidate's Id
        /// </summary>
        private static void CheckIfCandidateExists(ref string input, ref int candidateId, bool printData = false)
        {
            try
            {
                while (!LogInService.CandidateExists(candidateId, printData))
                {
                    input = Console.ReadLine();
                    input = CheckIfInt(input);
                    candidateId = int.Parse(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
