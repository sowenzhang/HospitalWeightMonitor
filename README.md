# HospitalWeightMonitor
a toy project

ASSUMPTIONS
==============================================================
- Patient: each patient has only First Name and Last Name
- Graph: shows the weight history 
- No authentication and authroization is implemented 
- the requirement on "Show a table of previously input items for the selected patient" is vague. The implementation assumes the previously input items are referred to weight only. 
- No testing is implemented
- No tracing is implemented, only exception is logged using NLog in the file format 

STRUCTURE 
==============================================================
- the backend is a Web API in C# + Entity Framework + OWIN + SQL Express 
- the front end is AngularJS + Material Design
- Hospital.Data: the project contains data entities, Code-First
- Hospital.Services: the business layer + DTO 
- Hospital.WebAPI: the Web API project, versioned 
- Hospital.Common + Hospital.Web.Common: some shared functionalities 
- Hospital.PatientManager.Web: the front end website 

DEPENDENCIES 
==============================================================
- generic unit of work: https://www.nuget.org/packages/Repository.Pattern/
- angularJs: https://angularjs.org/
- angular material design: https://material.angularjs.org/latest/#/
- ui router: https://github.com/angular-ui/ui-router
- angular charts + d3: https://github.com/chinmaymk/angular-charts/

FEATURES
==============================================================
- add a new patient 
- update the patient's name
- record a patient's weight
- remove a patient
- remove a patient's weight
- check the patient's weight change diagram 

POTENTIAL FEATURES
==============================================================
- some improvements in the performance
- a list of recent select patients 
- authorization and authentication
- show histories of changes on deleted records 

DATA DESIGN 
==============================================================
- the intented design is nothing to be deleted: insert only, never update 
- there is an operation exposed for deleting a patient, and it's for testing only 
- each patient has a patient information, which contains only First Name and Last Name 
- each patient has a weight table, each record represents a weight record 
- when query a patient summary, only the latest record from those child-tables will be returned 

SETUP 
==============================================================
- open the solution file
- hit F5 or click the run button
- the solution is setup to have multiple start up, so the default browser will launch 2 websites: one is the WebAPI, another is the front-end portal 
- the machine must have SQL Express installed 
- no need to run update-database
- there is no connection string defined in the web.config, hence the default location will be used. A new database file is likely to be created under the folder C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA

