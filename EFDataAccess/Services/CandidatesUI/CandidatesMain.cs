using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using BetterConsoleTables;
using EFDataAccess.Helpers;
using EFDataAccess.Models;
using EFDataAccess.Services.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace EFDataAccess.Services.CandidatesUI
{
    public class CandidatesMain
    {
        /// <summary>
        /// Displays a list of Candidate’s Certificates in a console table
        /// </summary>
        public static void ShowCertificatesOfCandidate(int candidatesId)
        {
            AppDBContext appDBContext = new AppDBContext();

            Table certificatesTable = GlobalMethodsAndVariables.CreateTable(new List<string> { "Certificate Title", "Assessment Test Code", "Examination Date",
                "Score Report Date", "Candidate Score", "Maximum Score",
                "Percentage Score", "Assessment Result", "Active" });

            certificatesTable.Config = TableConfiguration.Unicode();

            try
            {
                var currentCandidate = appDBContext.Candidates.Include("CandidateCertificates").Where(p => p.CandidateId == candidatesId).FirstOrDefault();
                var candidateCertificates = currentCandidate.CandidateCertificates;


                foreach (CandidateCertificates candidateCertificate in candidateCertificates)
                {
                    LoadCandidateCertificates(appDBContext, candidateCertificate, certificatesTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return; // exits methods (no need to display a broken table)
            }

            Console.WriteLine(certificatesTable.ToString());
        }

        /// <summary>
        /// Download a pdf file with all the certificates of a candidate
        /// </summary>
        public static void DownloadCertificatesOfCandidate(int candidatesId)
        {
            // Export of Candidate’s Certificates in a .pdf format
            AppDBContext appDBContext = new AppDBContext();

            var currentCandidate = appDBContext.Candidates.Include("CandidateCertificates").Where(p => p.CandidateId == candidatesId).FirstOrDefault();
            var candidateCertificates = currentCandidate.CandidateCertificates;

            try
            {
                // routine to load pdf string and print it to the pdf page,
                // then save the file to a location
                var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                using (var memoryStream = new MemoryStream()) // start new memory stream to write data on the pdf and save it to hard drive
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();
                    iTextSharp.text.Font fontDarkBlue = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, new BaseColor(Color.DarkBlue)); // set font style to be used later

                    pdfDoc.Add(new Paragraph(""));
                    pdfDoc.Add(new Chunk($"Certificates of {currentCandidate.FirstName} {currentCandidate.LastName} (all attempts)", fontDarkBlue)); // Title of the PDF
                    pdfDoc.Add(new Paragraph(" "));

                    foreach (CandidateCertificates candidateCertificate in candidateCertificates)
                    {
                        appDBContext.Entry(candidateCertificate).Reference(p => p.CertificateAssessment).Load();
                        CertificateAssessment assessmet = candidateCertificate.CertificateAssessment;

                        appDBContext.Entry(candidateCertificate).Reference(p => p.Certificate).Load();
                        Certificate certificate = candidateCertificate.Certificate;

                        pdfDoc.Add(new Paragraph(""));

                        pdfDoc.Add(new Chunk("CERTIFICATE: ", fontDarkBlue)); // chuck = same line formating
                        pdfDoc.Add(new Chunk(certificate.Title));
                        pdfDoc.Add(new Paragraph("")); // paragraph = Enviroment.NewLine

                        pdfDoc.Add(new Chunk("Assessment Test Code: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(certificate.AssessmentTestCode + " "));
                        pdfDoc.Add(new Chunk("Examination Date: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(candidateCertificate.ExaminationDate.ToShortDateString()));
                        pdfDoc.Add(new Paragraph(""));

                        pdfDoc.Add(new Chunk("Score Report Date: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(assessmet.ScoreReportDate.ToShortDateString() + " "));
                        pdfDoc.Add(new Chunk("Candidate: Score: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(assessmet.CandidateScore.ToString()));
                        pdfDoc.Add(new Paragraph(""));

                        pdfDoc.Add(new Chunk("Maximum Score: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(assessmet.MaximumScore.ToString() + " "));
                        pdfDoc.Add(new Chunk("Percentage: Score: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(assessmet.PercentageScore.ToString() + " %"));
                        pdfDoc.Add(new Paragraph(""));

                        pdfDoc.Add(new Chunk("Assessment Result: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(assessmet.AssessmentResult.ToString() + " "));
                        pdfDoc.Add(new Chunk("Active: ", fontDarkBlue));
                        pdfDoc.Add(new Chunk(certificate.Active.ToString()));

                        pdfDoc.Add(new Paragraph(" "));
                    }

                    pdfDoc.Close();

                    byte[] bytes = memoryStream.ToArray();
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                    string fileName = $@"\{currentCandidate.FirstName} {currentCandidate.LastName} Certificates.pdf";
                    File.WriteAllBytes(path + fileName, bytes);

                    memoryStream.Close();

                    Console.WriteLine("File save at at " + Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // we want a new try catch in case the pdf won't open, but we still manage to save it
            try
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) +
                    $@"\{currentCandidate.FirstName} {currentCandidate.LastName} Certificates.pdf"); // open saved pdf with the default application
            }
            catch (Exception)
            {
                Console.WriteLine("Could not open the PDF File, you can view it at the location shown above.");
            }

        }

        /// <summary>
        /// Routine to load in RAM all the CandidateCertificates associated tables
        /// </summary>
        private static void LoadCandidateCertificates(AppDBContext appDBContext, CandidateCertificates candidateCertificate, Table certificatesTable)
        {
            appDBContext.Entry(candidateCertificate).Reference(p => p.CertificateAssessment).Load();
            CertificateAssessment assessmet = candidateCertificate.CertificateAssessment;

            appDBContext.Entry(candidateCertificate).Reference(p => p.Certificate).Load();
            Certificate certificate = candidateCertificate.Certificate;

            certificatesTable.AddRow(certificate.Title, GlobalMethodsAndVariables.CheckIfStringIsNull(certificate.AssessmentTestCode)
                , candidateCertificate.ExaminationDate.ToShortDateString(),
                assessmet.ScoreReportDate.ToShortDateString(), GlobalMethodsAndVariables.CheckIfStringIsNull(assessmet.CandidateScore.ToString()),
                GlobalMethodsAndVariables.CheckIfStringIsNull(assessmet.MaximumScore.ToString()),
                GlobalMethodsAndVariables.CheckIfStringIsNull(assessmet.PercentageScore.ToString()) + " %", assessmet.AssessmentResult, certificate.Active);
        }
    }
}
