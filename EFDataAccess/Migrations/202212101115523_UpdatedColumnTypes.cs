namespace EFDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedColumnTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Certificates", "Title", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Certificates", "AssessmentTestCode", c => c.String(maxLength: 30));
            AlterColumn("dbo.Topics", "Title", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Topics", "Description", c => c.String(maxLength: 120));
            AlterColumn("dbo.Topics", "PossibleMarks", c => c.Short(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "CandidateScore", c => c.Short(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "MaximumScore", c => c.Short(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "PercentageScore", c => c.Short(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "AssessmentResult", c => c.String(maxLength: 40));
            AlterColumn("dbo.TopicAssesments", "AwardedMarks", c => c.Short(nullable: false));
            AlterColumn("dbo.CandidateContacts", "LandlineNumber", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateContacts", "MobileNumber", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateContacts", "Email", c => c.String(maxLength: 255));
            AlterColumn("dbo.Candidates", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Candidates", "MiddleName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Candidates", "LastName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Candidates", "Gender", c => c.String(maxLength: 1));
            AlterColumn("dbo.Candidates", "NativeLanguage", c => c.String(maxLength: 100));
            AlterColumn("dbo.CandidateLocations", "Address", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateLocations", "Address2", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateLocations", "Residence", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateLocations", "Province", c => c.String(maxLength: 120));
            AlterColumn("dbo.CandidateLocations", "City", c => c.String(maxLength: 100));
            AlterColumn("dbo.CandidateLocations", "PostalCode", c => c.String(maxLength: 15));
            AlterColumn("dbo.CandidatePhotoIdentifications", "Number", c => c.String(maxLength: 40));
            AlterColumn("dbo.PhotoIdentificationTypes", "Type", c => c.String(maxLength: 80));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhotoIdentificationTypes", "Type", c => c.String());
            AlterColumn("dbo.CandidatePhotoIdentifications", "Number", c => c.String());
            AlterColumn("dbo.CandidateLocations", "PostalCode", c => c.String());
            AlterColumn("dbo.CandidateLocations", "City", c => c.String());
            AlterColumn("dbo.CandidateLocations", "Province", c => c.String());
            AlterColumn("dbo.CandidateLocations", "Residence", c => c.String());
            AlterColumn("dbo.CandidateLocations", "Address2", c => c.String());
            AlterColumn("dbo.CandidateLocations", "Address", c => c.String());
            AlterColumn("dbo.Candidates", "NativeLanguage", c => c.String());
            AlterColumn("dbo.Candidates", "Gender", c => c.String());
            AlterColumn("dbo.Candidates", "LastName", c => c.String());
            AlterColumn("dbo.Candidates", "MiddleName", c => c.String());
            AlterColumn("dbo.Candidates", "FirstName", c => c.String());
            AlterColumn("dbo.CandidateContacts", "Email", c => c.String());
            AlterColumn("dbo.CandidateContacts", "MobileNumber", c => c.String());
            AlterColumn("dbo.CandidateContacts", "LandlineNumber", c => c.String());
            AlterColumn("dbo.TopicAssesments", "AwardedMarks", c => c.Int(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "AssessmentResult", c => c.String());
            AlterColumn("dbo.CertificateAssessments", "PercentageScore", c => c.Int(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "MaximumScore", c => c.Int(nullable: false));
            AlterColumn("dbo.CertificateAssessments", "CandidateScore", c => c.Int(nullable: false));
            AlterColumn("dbo.Topics", "PossibleMarks", c => c.Int(nullable: false));
            AlterColumn("dbo.Topics", "Description", c => c.String());
            AlterColumn("dbo.Topics", "Title", c => c.String());
            AlterColumn("dbo.Certificates", "AssessmentTestCode", c => c.String());
            AlterColumn("dbo.Certificates", "Title", c => c.String());
        }
    }
}
