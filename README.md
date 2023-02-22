# Candidate Management Console Application - .NET Framework 4.8 (Console Application)

The application includes a powerful administration interface that allows for CRUD operations on candidates and the ability to view all certificates for each candidate. Additionally, candidates have the ability to view or download their certificates in PDF format. The certificate entity also includes CRUD operations.

The application is composed of two projects:

DataAccess: a class library containing the models, entities, and context for the application,
MainApplication: which houses the main console application

The MainApplication project allows for the management of candidate data through commands entered in the console, providing a simple and efficient way to manage candidates and their associated certificates.

## Instructions
1. Clone repository
2. On EFDataAccess project run this command at Package Manager Console
    - 'Update-Database'
    - You can change the location of the database from the App.config file on both EFDataAccess and MavraganisConsoleApp.
    - Default: localdb
4. Run console application
