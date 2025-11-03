/*
Do not use database modifying (ALTER DATABASE), crea[RWAdatabase]ting (CREATE DATABASE) or switching (USE) statements 
in this file.
*/

CREATE TABLE [Role](
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(250) NOT NULL
)
GO

CREATE TABLE Person(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(250) NOT NULL,
	LastName NVARCHAR(250) NOT NULL,
	Email NVARCHAR(250) NOT NULL,
	Phone NVARCHAR(50) NOT NULL,
	Username NVARCHAR(50) NOT NULL,
	PasswordHash NVARCHAR(250) NOT NULL,
	PasswordSalt NVARCHAR(250) NOT NULL,
	CreatedAt DATETIME NOT NULL DEFAULT SYSDATETIME(),--vraca trenutno vrijeme i datum
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE PersonRole(
	PersonId INT NOT NULL,
	RoleId INT NOT NULL,
	CONSTRAINT PK_PersonRole PRIMARY KEY(PersonId, RoleId),
	CONSTRAINT FK_PersonRole_Person FOREIGN KEY(PersonId) REFERENCES Person(Id),
    CONSTRAINT FK_PersonRole_Role FOREIGN KEY (RoleId) REFERENCES [Role](Id)
)
GO

CREATE TABLE JobType(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(250) NOT NULL,
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE Contractor(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	PersonId INT UNIQUE NULL,
	CONSTRAINT FK_Contractor_Person FOREIGN KEY (PersonId) REFERENCES Person(Id),
	CompanyName NVARCHAR(250) NULL,
	JobTypeId INT NOT NULL,
	CONSTRAINT FK_Contractor_JobType FOREIGN KEY (JobTypeId) REFERENCES JobType(Id), --referenca na job type
	IsFreelancer BIT DEFAULT 0,
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE Country(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(250) NOT NULL
)
GO

CREATE TABLE Town(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(250) NOT NULL,
	CountryId INT  NOT NULL,
	CONSTRAINT FK_Town_Country FOREIGN KEY (CountryId) REFERENCES Country(Id)
)
GO

CREATE TABLE [Location](
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	PostalCode NVARCHAR(50) NOT NULL,
	TownId INT NOT NULL,
	CONSTRAINT FK_Location_Town FOREIGN KEY (TownId) REFERENCES Town(Id),
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO
CREATE TABLE ContractorLocation(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	ContractorId INT NOT NULL,
    LocationId INT NOT NULL,
	CONSTRAINT FK_ContractorLocation_Contractor FOREIGN KEY (ContractorId) REFERENCES Contractor(Id),
	CONSTRAINT FK_ContractorLocation_Location FOREIGN KEY (LocationId) REFERENCES [Location](Id)
    --PRIMARY KEY(ContractorId, LocationId)
)
GO
CREATE TABLE JobPost(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	ContractorLocationId INT  NOT NULL,
	CONSTRAINT FK_JobPost_ContractorLocation FOREIGN KEY (ContractorLocationId) REFERENCES ContractorLocation(Id),
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE JobApplication(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	JobPostId INT NOT NULL,
	PersonId INT  NOT NULL,
	CONSTRAINT FK_JobApplication_JobPost FOREIGN KEY (JobPostId) REFERENCES JobPost(Id),
	CONSTRAINT FK_JobApplication_Person FOREIGN KEY (PersonId) REFERENCES Person(Id),
	CreatedAt DATETIME NOT NULL DEFAULT SYSDATETIME(),--vraca trenutno vrijeme i datum
	IsDeleted BIT DEFAULT 0 NOT NULL
)
GO

CREATE TABLE Logs(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Timestamp] DATETIME NOT NULL DEFAULT SYSDATETIME(),
	[Level] INT,
	[Message] NVARCHAR(250)
)
GO
/* Use table data inserting, modifying, deleting and retrieving statements here */

--ALTER TABLE Contractor ADD CONSTRAINT CheckFreelancerPersonId CHECK (
--	(IsFreelancer = 1 AND PersonId IS NOT NULL) OR (IsFreelancer = 0 AND PersonId IS NULL))
--ensure not null ID for freelancer 

INSERT INTO [Role] ([Name])
	VALUES('Admin'), ('User'),('CompanyAdmin'),('Freelancer')


--Pa$$word -> hash ->
--salt ->

--{
--  "firstName": "Admin",
--  "lastName": "Admin",
--  "email": "aa@example.com",
--  "phone": "1341341241",
--  "username": "admin",
--  "password": "Pa$$w0rd"
--} --kreirao sam usera, zatim sam uzeo njegov hash i salt te ih iskoitisio kod kreiranja admina kao seed u DDL skripti, zatim sam obrisao usera


INSERT INTO Person(FirstName, LastName, Email, Phone, Username, PasswordHash, PasswordSalt, CreatedAt, IsDeleted)
	VALUES('Admin','Admin','admin@gmail.com', '1341341241','admin','5I0IPz1JXj93P0wkInNHgTJ0+8lSyuJ2UZOdXNvxzaA=' , 'ypnw6H/YGdHlp+gNVV3WMg==',SYSUTCDATETIME(),0)

DECLARE @AdminId INT = SCOPE_IDENTITY()

INSERT INTO PersonRole(PersonId, RoleId)
	VALUES(@AdminId, 1)
GO


ALTER TABLE Town DROP CONSTRAINT FK_Town_Country
GO
ALTER TABLE Town
ADD CONSTRAINT FK_Town_Country 
FOREIGN KEY (CountryId) REFERENCES Country(Id) 
ON DELETE CASCADE
GO


ALTER TABLE [Location] DROP CONSTRAINT FK_Location_Town
GO

ALTER TABLE [Location]
ADD CONSTRAINT FK_Location_Town 
FOREIGN KEY (TownId) REFERENCES Town(Id) 
ON DELETE CASCADE
GO
