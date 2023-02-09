using BetterConsoleTables;
using EasyConsole;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;

namespace EFDataAccess.Helpers
{
    /// <summary>
    /// Class used to store many methods that will be used in the class library as public methods
    /// </summary>
    public class GlobalMethodsAndVariables
    {
        /// <summary>
        /// Checks if given string is null and if true returns empty string
        /// (needed for tables we cant input null inside a cell)
        /// </summary>
        public static string CheckIfStringIsNull(string tempS)
        {
            if (tempS == null)
            {
                return "";
            }

            return tempS;
        }

        /// <summary>
        /// Dynamic method to create table columns from a list of strings
        /// </summary>
        public static Table CreateTable(List<string> tableColumns)
        {
            // Dynamic way of creating tables with any number of columns
            Table table = new Table();

            foreach (string s in tableColumns)
            {
                table.AddColumn(s);
            }

            return table;
        }

        #region Console Promp Methods
        /// <summary>
        /// Get console promp property from the user and checks if it the desired string type
        /// </summary>
        public static string GetPropertyAsString(string name, int stringSize, bool requried = false)
        {
            Console.Write($"Please type the {name}: ");
            string input = Console.ReadLine();

            if (input.Length == 0 & requried == false)
            {
                return "";
            }

            while (input.Length > stringSize || input.Length == 0)
            {
                Console.Write($"Input can not exceed {stringSize} characters (length) and it can not be empty: ");
                input = Console.ReadLine();
            }

            return input;
        }

        /// <summary>
        /// Get console promp property from the user and checks if it the desired date type
        /// </summary>
        public static DateTime GetPropertyAsDate(string name)
        {
            Console.Write($"Please type the {name} (Format dd/MM/yyyy): ");
            string input = Console.ReadLine();

            while (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime _) || input.Contains("-"))
            {
                Console.Write("Please provide us with a date (Format dd/MM/yyyy): ");
                input = Console.ReadLine();
            }

            return DateTime.Parse(input);
        }

        /// <summary>
        /// Get console promp property from the user and checks if it the desired date type
        /// </summary>
        public static int GetPropertyAsPhotoId(string name)
        {
            Console.WriteLine($"Please type the {name}: ");
            Console.Write("1=Passport, 2=Driver Licence, 3=Identity Card, 4=Company Id Card, 5=Military Id: ");
            string input = Console.ReadLine();

            bool isValid = false;
            while (!isValid)
            {
                if (!int.TryParse(input, out int _))
                {
                    Console.Write("Please provide us with an integer between 1 to 5: ");
                    input = Console.ReadLine();
                }
                else if (int.Parse(input) < 1 || int.Parse(input) > 5)
                {
                    Console.Write("Please provide us with an integer between 1 to 5: ");
                    input = Console.ReadLine();
                }
                else
                {
                    isValid = true;
                }
            }

            return int.Parse(input);
        }

        /// <summary>
        /// Console promp for the user to select y for yes and n for no
        /// </summary>
        public static bool Confirm(string name)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{name} [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
        #endregion
    }
}
