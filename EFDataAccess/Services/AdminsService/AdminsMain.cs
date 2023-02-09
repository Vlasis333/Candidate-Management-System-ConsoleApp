using BetterConsoleTables;
using EFDataAccess.Models;
using EFDataAccess.Services.Data;
using EFDataAccess.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using static Org.BouncyCastle.Math.EC.ECCurve;
using EasyConsole;
using System.Windows.Forms;
using System.Data.Entity;
using Org.BouncyCastle.Asn1.X509;
using System.Runtime.Remoting.Contexts;

namespace EFDataAccess.Services.AdminsService
{
    public class AdminsMain
    {
        /// <summary>
        /// Creates a new candidate
        /// </summary>
        public static void AddNewCandidate()
        {
            AppDBContext appDBContext = new AppDBContext();
            // Candidate
            Output.WriteLine(ConsoleColor.Green, "Adding New Candidate");
            var insertedCandidate = appDBContext.Candidates.Add(new Candidate
            {
                FirstName = GlobalMethodsAndVariables.GetPropertyAsString("first name: ", 100, true), // true when something is required
                MiddleName = GlobalMethodsAndVariables.GetPropertyAsString("middle name: ", 100),
                LastName = GlobalMethodsAndVariables.GetPropertyAsString("last name: ", 100),
                Gender = GlobalMethodsAndVariables.GetPropertyAsString("gender (M,F,B etc): ", 1).ToUpper(),
                NativeLanguage = GlobalMethodsAndVariables.GetPropertyAsString("native language: ", 100),
                BirthDate = GlobalMethodsAndVariables.GetPropertyAsDate("birth date: ")
            });
            appDBContext.SaveChanges();

            Output.WriteLine(ConsoleColor.Green, $"Candidate {insertedCandidate.FirstName} {insertedCandidate.LastName} saved.");

            // Contact Information
            try
            {
                // choice if the user would like to insert more related candidate data or not
                if (GlobalMethodsAndVariables.Confirm("Would you like to insert candidate's contact information also?"))
                {
                    appDBContext.CandidateContacts.Add(new CandidateContact
                    {
                        LandlineNumber = GlobalMethodsAndVariables.GetPropertyAsString("land Line number: ", 120),
                        MobileNumber = GlobalMethodsAndVariables.GetPropertyAsString("mobile mumber: ", 120),
                        Email = GlobalMethodsAndVariables.GetPropertyAsString("email: ", 255),
                        Candidate = insertedCandidate
                    });
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Contact information for candidate {insertedCandidate.FirstName} {insertedCandidate.LastName} saved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Location Information
            try
            {
                if (GlobalMethodsAndVariables.Confirm("Would you like to insert candidate's location information also?"))
                {
                    appDBContext.CandidateLocations.Add(new CandidateLocation
                    {
                        Address = GlobalMethodsAndVariables.GetPropertyAsString("address: ", 120),
                        Address2 = GlobalMethodsAndVariables.GetPropertyAsString("address 2: ", 120),
                        Residence = GlobalMethodsAndVariables.GetPropertyAsString("residence: ", 120),
                        Province = GlobalMethodsAndVariables.GetPropertyAsString("province: ", 120),
                        City = GlobalMethodsAndVariables.GetPropertyAsString("city: ", 100),
                        PostalCode = GlobalMethodsAndVariables.GetPropertyAsString("postal code: ", 15),
                        Candidate = insertedCandidate
                    });
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Location information for candidate {insertedCandidate.FirstName} {insertedCandidate.LastName} saved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Photo Identification Information
            try
            {
                if (GlobalMethodsAndVariables.Confirm("Would you like to insert candidate's photo identification info also?"))
                {
                    // needed to get the associated information from the photo identification type table
                    int photoIdSelected = GlobalMethodsAndVariables.GetPropertyAsPhotoId("photo identification id: ");
                    var photoIdType = appDBContext.PhotoIdentificationTypes.Where(p => p.PhotoIdentificationTypeId == photoIdSelected).SingleOrDefault();

                    appDBContext.CandidatePhotoIdentifications.Add(new CandidatePhotoIdentification
                    {
                        Number = GlobalMethodsAndVariables.GetPropertyAsString("photo id number: ", 40),
                        IssueDate = GlobalMethodsAndVariables.GetPropertyAsDate("photo id issue date: "),
                        PhotoIdentificationType = photoIdType,
                        Candidate = insertedCandidate
                    });
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Photo identification information for candidate {insertedCandidate.FirstName} {insertedCandidate.LastName} saved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Updates a candidate selected by the user
        /// </summary>
        public static void UpdateCandidate(int candidatesId)
        {
            // Update an existing candidate
            AppDBContext appDBContext = new AppDBContext();

            // Candidate
            var selectedCandidate = appDBContext.Candidates.Find(candidatesId);
            try
            {
                Output.WriteLine(ConsoleColor.Green, $"Updating Candidate {selectedCandidate.FirstName} {selectedCandidate.LastName}");

                selectedCandidate.FirstName = GlobalMethodsAndVariables.GetPropertyAsString("first name: ", 100, true);
                selectedCandidate.MiddleName = GlobalMethodsAndVariables.GetPropertyAsString("middle name: ", 100);
                selectedCandidate.LastName = GlobalMethodsAndVariables.GetPropertyAsString("last name: ", 100);
                selectedCandidate.Gender = GlobalMethodsAndVariables.GetPropertyAsString("gender (M,F,B etc): ", 1).ToUpper();
                selectedCandidate.NativeLanguage = GlobalMethodsAndVariables.GetPropertyAsString("native language: ", 100);
                selectedCandidate.BirthDate = GlobalMethodsAndVariables.GetPropertyAsDate("birth date: ");

                appDBContext.SaveChanges();

                Output.WriteLine(ConsoleColor.Green, $"Candidate {selectedCandidate.FirstName} {selectedCandidate.LastName} updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Contact Information
            try
            {
                var selectedCandidateContact = appDBContext.CandidateContacts.Find(candidatesId);
                if (selectedCandidateContact == null) // case selected candidate doesnt have contact info registered
                {
                    return;
                }

                if (GlobalMethodsAndVariables.Confirm("Would you like to update candidate's contact information also?"))
                {
                    selectedCandidateContact.LandlineNumber = GlobalMethodsAndVariables.GetPropertyAsString("land Line number: ", 120);
                    selectedCandidateContact.MobileNumber = GlobalMethodsAndVariables.GetPropertyAsString("mobile mumber: ", 120);
                    selectedCandidateContact.Email = GlobalMethodsAndVariables.GetPropertyAsString("email: ", 255);
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Contact information for candidate {selectedCandidate.FirstName} {selectedCandidate.LastName} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Location Information
            try
            {
                var selectedCandidateLocation = appDBContext.CandidateLocations.Find(candidatesId);
                if (selectedCandidateLocation == null) // case selected candidate doesnt have location info registered
                {
                    return;
                }

                if (GlobalMethodsAndVariables.Confirm("Would you like to update candidate's location information also?"))
                {
                    selectedCandidateLocation.Address = GlobalMethodsAndVariables.GetPropertyAsString("address: ", 120);
                    selectedCandidateLocation.Address2 = GlobalMethodsAndVariables.GetPropertyAsString("address 2: ", 120);
                    selectedCandidateLocation.Residence = GlobalMethodsAndVariables.GetPropertyAsString("residence: ", 120);
                    selectedCandidateLocation.Province = GlobalMethodsAndVariables.GetPropertyAsString("province: ", 120);
                    selectedCandidateLocation.City = GlobalMethodsAndVariables.GetPropertyAsString("city: ", 100);
                    selectedCandidateLocation.PostalCode = GlobalMethodsAndVariables.GetPropertyAsString("postal code: ", 15);
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Location information for candidate {selectedCandidate.FirstName} {selectedCandidate.LastName} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Photo Identification Information
            try
            {
                var selectedCandidatePhotoId = appDBContext.CandidatePhotoIdentifications.Find(candidatesId);
                if (selectedCandidatePhotoId == null) // case selected candidate doesnt have photo indentification info registered
                {
                    return;
                }

                if (GlobalMethodsAndVariables.Confirm("Would you like to update candidate's photo identification information also?"))
                {
                    int photoIdSelected = GlobalMethodsAndVariables.GetPropertyAsPhotoId("photo identification id: ");
                    var photoIdType = appDBContext.PhotoIdentificationTypes.Where(p => p.PhotoIdentificationTypeId == photoIdSelected).SingleOrDefault();

                    selectedCandidatePhotoId.Number = GlobalMethodsAndVariables.GetPropertyAsString("photo id number: ", 40);
                    selectedCandidatePhotoId.IssueDate = GlobalMethodsAndVariables.GetPropertyAsDate("photo id issue date: ");
                    selectedCandidatePhotoId.PhotoIdentificationType = photoIdType;
                    appDBContext.SaveChanges();

                    Output.WriteLine(ConsoleColor.Green, $"Photo identification information for candidate {selectedCandidate.FirstName} {selectedCandidate.LastName} updated.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing candidate, selected by the user
        /// </summary>
        public static void DeleteCandidate(int candidatesId)
        {
            // We must delete every entry of the related tables before deleting the candidate itself
            AppDBContext appDBContext = new AppDBContext();
            var candidate = appDBContext.Candidates.Include("CandidateCertificates").Where(p => p.CandidateId == candidatesId).FirstOrDefault();

            string candidateFullName = $"{candidate.CandidateId} {candidate.FirstName} {candidate.LastName}";
            try
            {
                var candidateCertificates = candidate.CandidateCertificates;

                foreach (CandidateCertificates candidateCertificate in candidateCertificates) // routine to delete all certificates and relationships of the candidate
                {
                    appDBContext.Entry(candidateCertificate).Reference(p => p.CertificateAssessment).Load();
                    var assessmet = candidateCertificate.CertificateAssessment;

                    // we need to load the TopicAssessments as it is navigator on CandidateCertificates and we cant remove them just by reference on the navigator
                    // we need the object on memory block to remove all the items it contains (related to the candidate we delete)
                    var tempCandidateCertificates = appDBContext.CandidateCertificates.Include("TopicAssesments").Where(p => p.CandidateCertificatesId == candidateCertificate.CandidateCertificatesId).FirstOrDefault();
                    var topicAssessments = tempCandidateCertificates.TopicAssesments;

                    appDBContext.TopicAssesments.RemoveRange(topicAssessments);
                    appDBContext.CertificateAssessments.Remove(assessmet);
                }

                // removes certificates obtained by candidate - relationship tables
                appDBContext.CandidateCertificates.RemoveRange(candidateCertificates);

                // remove all extra data of the candidate before removing the candidate itself
                appDBContext.Entry(candidate).Reference(p => p.CandidateContact).Load();
                var candidateContact = candidate.CandidateContact;

                appDBContext.Entry(candidate).Reference(p => p.CandidateLocation).Load();
                var candidateLocation = candidate.CandidateLocation;

                appDBContext.Entry(candidate).Reference(p => p.CandidatePhotoIdentification).Load();
                var candidatePhotoId = candidate.CandidatePhotoIdentification;

                appDBContext.CandidateContacts.Remove(candidateContact);
                appDBContext.CandidateLocations.Remove(candidateLocation);
                appDBContext.CandidatePhotoIdentifications.Remove(candidatePhotoId);
                appDBContext.Candidates.Remove(candidate);
                appDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Output.WriteLine(ConsoleColor.Green, $"Deleted Candidate {candidateFullName}");
        }

        /// <summary>
        /// Read all candidates with thier information
        /// </summary>
        public static void ShowCandidatesInfo()
        {
            AppDBContext appDBContext = new AppDBContext();

            Table candidatesMainInfoTable = GlobalMethodsAndVariables.CreateTable(new List<string> { "Candidate Number", "First Name", "Middle Name", "Last Name", "Gender", "Native Language", "Birth Date",
                "Land Line Number", "Mobile Number", "EMail" });

            Table candidatesSecondaryInfoTable = GlobalMethodsAndVariables.CreateTable(new List<string> { "Candidate Number", "Address" , "Address2", "Residence", "Province", "City", "PostalCode",
                "Photo Id Number", "Photo Id IssueDate", "Photo Id Type" });

            candidatesMainInfoTable.Config = TableConfiguration.Unicode();
            candidatesSecondaryInfoTable.Config = TableConfiguration.Unicode();

            try
            {
                var candidates = appDBContext.Candidates.ToList();

                foreach (Candidate candidate in candidates)
                {
                    LoadAllCandidatesInformation(appDBContext, candidate, candidatesMainInfoTable, candidatesSecondaryInfoTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine(candidatesMainInfoTable.ToString());
            Console.WriteLine(candidatesSecondaryInfoTable.ToString());
        }

        /// <summary>
        /// Read all the candidates with thier sertificates's results
        /// </summary>
        public static void ShowCandidatesResults()
        {
            AppDBContext appDBContext = new AppDBContext();

            Table candidatesResultsTable = GlobalMethodsAndVariables.CreateTable(new List<string> { "Candidate Number", "First Name", "Middle Name", "Last Name",
                "Certificate Id", "Certificate Title", "Certificate Exam Data", "Percentage Score", "AssessmentResult" });

            candidatesResultsTable.Config = TableConfiguration.Unicode();

            try
            {
                var candidates = appDBContext.Candidates.Include("CandidateCertificates").ToList();


                foreach (Candidate candidate in candidates)
                {
                    LoadAllCandidatesWithThierResults(appDBContext, candidate, candidatesResultsTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine(candidatesResultsTable.ToString());
        }

        /// <summary>
        /// Loads candidates related tables to RAM and checks if all associated tables have data for each FK from PK of candidates
        /// </summary>
        private static void LoadAllCandidatesInformation(AppDBContext appDBContext, Candidate candidate, Table candidatesMainInfoTable, Table candidatesSecondaryInfoTable)
        {
            appDBContext.Entry(candidate).Reference(p => p.CandidateContact).Load();
            CandidateContact contact = candidate.CandidateContact;
            appDBContext.Entry(candidate).Reference(p => p.CandidateLocation).Load();
            CandidateLocation location = candidate.CandidateLocation;
            appDBContext.Entry(candidate).Reference(p => p.CandidatePhotoIdentification).Load();
            CandidatePhotoIdentification photoIdentification = candidate.CandidatePhotoIdentification;

            if (contact != null) // case certain cadidates dont have all tables filled with an input
            {
                candidatesMainInfoTable.AddRow(candidate.CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.FirstName), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.MiddleName),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.LastName), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.Gender), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.NativeLanguage),
                    candidate.BirthDate.ToShortDateString(), GlobalMethodsAndVariables.CheckIfStringIsNull(contact.LandlineNumber),
                     GlobalMethodsAndVariables.CheckIfStringIsNull(contact.MobileNumber), GlobalMethodsAndVariables.CheckIfStringIsNull(contact.Email));
            }
            else
            {
                candidatesMainInfoTable.AddRow(candidate.CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.FirstName), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.MiddleName),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.LastName), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.Gender), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.NativeLanguage),
                    candidate.BirthDate.ToShortDateString(), "", "", "");
            }

            if (location != null & photoIdentification != null)
            {
                appDBContext.Entry(photoIdentification).Reference(p => p.PhotoIdentificationType).Load();

                candidatesSecondaryInfoTable.AddRow(candidate.CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(location.Address), GlobalMethodsAndVariables.CheckIfStringIsNull(location.Address2),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(location.Residence), GlobalMethodsAndVariables.CheckIfStringIsNull(location.Province),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(location.City), GlobalMethodsAndVariables.CheckIfStringIsNull(location.PostalCode),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(photoIdentification.Number), photoIdentification.IssueDate.ToShortDateString(),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(photoIdentification.PhotoIdentificationType.Type));
            }
            else if (location == null & photoIdentification != null)
            {
                appDBContext.Entry(photoIdentification).Reference(p => p.PhotoIdentificationType).Load();

                candidatesSecondaryInfoTable.AddRow(candidate.CandidateId, "", "", "", "", "", "",
                    GlobalMethodsAndVariables.CheckIfStringIsNull(photoIdentification.Number), photoIdentification.IssueDate.ToShortDateString(),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(photoIdentification.PhotoIdentificationType.Type));
            }
            else if (location != null & photoIdentification == null)
            {
                candidatesSecondaryInfoTable.AddRow(candidate.CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(location.Address), GlobalMethodsAndVariables.CheckIfStringIsNull(location.Address2),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(location.Residence), GlobalMethodsAndVariables.CheckIfStringIsNull(location.Province),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(location.City), GlobalMethodsAndVariables.CheckIfStringIsNull(location.PostalCode),
                    "", "", "");
            }            
            else // case both are null we create an emtpy row to keep the consistency of the table// case both are null we create an emtpy row to keep the consistency of the table
            {
                candidatesSecondaryInfoTable.AddRow(candidate.CandidateId, "", "", "", "", "", "", "", "", "");
            }
        }

        /// <summary>
        /// Loads all candidates with their certificates on RAM
        /// </summary>
        private static void LoadAllCandidatesWithThierResults(AppDBContext appDBContext, Candidate candidate, Table candidatesResultsTable)
        {
            var candidateCertificates = candidate.CandidateCertificates;

            foreach (CandidateCertificates candidateCertificate in candidateCertificates)
            {
                appDBContext.Entry(candidateCertificate).Reference(p => p.CertificateAssessment).Load();
                CertificateAssessment assessmet = candidateCertificate.CertificateAssessment;

                appDBContext.Entry(candidateCertificate).Reference(p => p.Certificate).Load();
                Certificate certificate = candidateCertificate.Certificate;

                candidatesResultsTable.AddRow(candidate.CandidateId, GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.FirstName),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.MiddleName), GlobalMethodsAndVariables.CheckIfStringIsNull(candidate.LastName),
                    certificate.CertificateId, GlobalMethodsAndVariables.CheckIfStringIsNull(certificate.Title), candidateCertificate.ExaminationDate.ToShortDateString(),
                    GlobalMethodsAndVariables.CheckIfStringIsNull(assessmet.PercentageScore.ToString()) + " %", assessmet.AssessmentResult);
            }

        }
    }
}
