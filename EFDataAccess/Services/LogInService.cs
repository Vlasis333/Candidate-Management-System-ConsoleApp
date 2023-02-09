using EasyConsole;
using EFDataAccess.Models;
using EFDataAccess.Services.Data;
using System;
using System.Linq;

namespace EFDataAccess.Services
{
    public class LogInService
    {
        /// <summary>
        /// Checks if given Id exists in database
        /// </summary>
        public static bool CandidateExists(int candidatesId, bool printData = false)
        {
            AppDBContext appDBContext = new AppDBContext();
            var currentCandidate = appDBContext.Candidates.Where(p => p.CandidateId == candidatesId).SingleOrDefault();

            if (currentCandidate != null)
            {
                if (printData)
                {
                    Output.WriteLine(ConsoleColor.Green, $"Welcome {currentCandidate.FirstName} {currentCandidate.LastName}:");
                    Console.WriteLine(currentCandidate);
                    LoadCandidateInformation(appDBContext, currentCandidate);
                }
                return true;
            }
            else
            {
                Console.Write("Please provide us with the correct Candidate Number (Id): ");
                return false;
            }
        }

        /// <summary>
        /// Loads the candidate's basic information
        /// </summary>
        private static void LoadCandidateInformation(AppDBContext appDBContext, Candidate currentCandidate)
        {
            // Check if the given entity is loaded and print the needed connections, else loads the enity before printing
            appDBContext.Entry(currentCandidate).Reference(p => p.CandidateContact).Load();
            Console.WriteLine(currentCandidate.CandidateContact);

            appDBContext.Entry(currentCandidate).Reference(p => p.CandidateLocation).Load();
            Console.WriteLine(currentCandidate.CandidateLocation);

            appDBContext.Entry(currentCandidate).Reference(p => p.CandidatePhotoIdentification).Load();

            appDBContext.Entry(currentCandidate.CandidatePhotoIdentification).Reference(p => p.PhotoIdentificationType).Load();

            Console.WriteLine(currentCandidate.CandidatePhotoIdentification);
        }
    }
}
