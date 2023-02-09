﻿namespace EFDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CandidateCertificates",
                c => new
                    {
                        CandidateCertificatesId = c.Int(nullable: false, identity: true),
                        ExaminationDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                        Certificate_CertificateId = c.Int(),
                        Candidate_CandidateId = c.Int(),
                    })
                .PrimaryKey(t => t.CandidateCertificatesId)
                .ForeignKey("dbo.Certificates", t => t.Certificate_CertificateId)
                .ForeignKey("dbo.Candidates", t => t.Candidate_CandidateId)
                .Index(t => t.Certificate_CertificateId)
                .Index(t => t.Candidate_CandidateId);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        CertificateId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AssessmentTestCode = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CertificateId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        PossibleMarks = c.Int(nullable: false),
                        Certificate_CertificateId = c.Int(),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.Certificates", t => t.Certificate_CertificateId)
                .Index(t => t.Certificate_CertificateId);
            
            CreateTable(
                "dbo.CertificateAssessments",
                c => new
                    {
                        CertificateAssessmentId = c.Int(nullable: false),
                        ScoreReportDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                        CandidateScore = c.Int(nullable: false),
                        MaximumScore = c.Int(nullable: false),
                        PercentageScore = c.Int(nullable: false),
                        AssessmentResult = c.String(),
                    })
                .PrimaryKey(t => t.CertificateAssessmentId)
                .ForeignKey("dbo.CandidateCertificates", t => t.CertificateAssessmentId)
                .Index(t => t.CertificateAssessmentId);
            
            CreateTable(
                "dbo.TopicAssesments",
                c => new
                    {
                        TopicAssesmentId = c.Int(nullable: false, identity: true),
                        AwardedMarks = c.Int(nullable: false),
                        CandidateCertificates_CandidateCertificatesId = c.Int(),
                        Topic_TopicId = c.Int(),
                    })
                .PrimaryKey(t => t.TopicAssesmentId)
                .ForeignKey("dbo.CandidateCertificates", t => t.CandidateCertificates_CandidateCertificatesId)
                .ForeignKey("dbo.Topics", t => t.Topic_TopicId)
                .Index(t => t.CandidateCertificates_CandidateCertificatesId)
                .Index(t => t.Topic_TopicId);
            
            CreateTable(
                "dbo.CandidateContacts",
                c => new
                    {
                        CandidateContactId = c.Int(nullable: false),
                        LandlineNumber = c.String(),
                        MobileNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.CandidateContactId)
                .ForeignKey("dbo.Candidates", t => t.CandidateContactId)
                .Index(t => t.CandidateContactId);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        NativeLanguage = c.String(),
                        BirthDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                    })
                .PrimaryKey(t => t.CandidateId);
            
            CreateTable(
                "dbo.CandidateLocations",
                c => new
                    {
                        CandidateLocationId = c.Int(nullable: false),
                        Address = c.String(),
                        Address2 = c.String(),
                        Residence = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.CandidateLocationId)
                .ForeignKey("dbo.Candidates", t => t.CandidateLocationId)
                .Index(t => t.CandidateLocationId);
            
            CreateTable(
                "dbo.CandidatePhotoIdentifications",
                c => new
                    {
                        CandidatePhotoIdentificationId = c.Int(nullable: false),
                        Number = c.String(),
                        IssueDate = c.DateTime(nullable: false, defaultValue: DateTime.Now),
                        PhotoIdentificationType_PhotoIdentificationTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.CandidatePhotoIdentificationId)
                .ForeignKey("dbo.Candidates", t => t.CandidatePhotoIdentificationId)
                .ForeignKey("dbo.PhotoIdentificationTypes", t => t.PhotoIdentificationType_PhotoIdentificationTypeId)
                .Index(t => t.CandidatePhotoIdentificationId)
                .Index(t => t.PhotoIdentificationType_PhotoIdentificationTypeId);
            
            CreateTable(
                "dbo.PhotoIdentificationTypes",
                c => new
                    {
                        PhotoIdentificationTypeId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.PhotoIdentificationTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CandidateContacts", "CandidateContactId", "dbo.Candidates");
            DropForeignKey("dbo.CandidatePhotoIdentifications", "PhotoIdentificationType_PhotoIdentificationTypeId", "dbo.PhotoIdentificationTypes");
            DropForeignKey("dbo.CandidatePhotoIdentifications", "CandidatePhotoIdentificationId", "dbo.Candidates");
            DropForeignKey("dbo.CandidateLocations", "CandidateLocationId", "dbo.Candidates");
            DropForeignKey("dbo.CandidateCertificates", "Candidate_CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.TopicAssesments", "Topic_TopicId", "dbo.Topics");
            DropForeignKey("dbo.TopicAssesments", "CandidateCertificates_CandidateCertificatesId", "dbo.CandidateCertificates");
            DropForeignKey("dbo.CertificateAssessments", "CertificateAssessmentId", "dbo.CandidateCertificates");
            DropForeignKey("dbo.CandidateCertificates", "Certificate_CertificateId", "dbo.Certificates");
            DropForeignKey("dbo.Topics", "Certificate_CertificateId", "dbo.Certificates");
            DropIndex("dbo.CandidatePhotoIdentifications", new[] { "PhotoIdentificationType_PhotoIdentificationTypeId" });
            DropIndex("dbo.CandidatePhotoIdentifications", new[] { "CandidatePhotoIdentificationId" });
            DropIndex("dbo.CandidateLocations", new[] { "CandidateLocationId" });
            DropIndex("dbo.CandidateContacts", new[] { "CandidateContactId" });
            DropIndex("dbo.TopicAssesments", new[] { "Topic_TopicId" });
            DropIndex("dbo.TopicAssesments", new[] { "CandidateCertificates_CandidateCertificatesId" });
            DropIndex("dbo.CertificateAssessments", new[] { "CertificateAssessmentId" });
            DropIndex("dbo.Topics", new[] { "Certificate_CertificateId" });
            DropIndex("dbo.CandidateCertificates", new[] { "Candidate_CandidateId" });
            DropIndex("dbo.CandidateCertificates", new[] { "Certificate_CertificateId" });
            DropTable("dbo.PhotoIdentificationTypes");
            DropTable("dbo.CandidatePhotoIdentifications");
            DropTable("dbo.CandidateLocations");
            DropTable("dbo.Candidates");
            DropTable("dbo.CandidateContacts");
            DropTable("dbo.TopicAssesments");
            DropTable("dbo.CertificateAssessments");
            DropTable("dbo.Topics");
            DropTable("dbo.Certificates");
            DropTable("dbo.CandidateCertificates");
        }
    }
}
