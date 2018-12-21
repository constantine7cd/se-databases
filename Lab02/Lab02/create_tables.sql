/* Change to the GIBDD database */
USE GIBDD;
GO

/* Create tables */
CREATE TABLE Cars (
    CarId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CarName nvarchar(255) NOT NULL,
    CarVin nvarchar(20) NOT NULL,
    CarDate DATE NOT NULL,
    CarBody nvarchar(20) NOT NULL,

    CONSTRAINT CarDate CHECK (CarDate > CAST('1900-01-01' as DATE)), 
);

CREATE TABLE Drivers (
    DriverId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Experience int NOT NULL,
);

CREATE TABLE CarsDrivers (
    DriverId int FOREIGN KEY REFERENCES Drivers(DriverId),
    CarId int FOREIGN KEY REFERENCES Cars(CarId),
    IsOwner bit NOT NULL,
);


CREATE TABLE DriverLicense (
    LicenseId int IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    DateOfIssue DATE NOT NULL,
    DateOfExpiration DATE NOT NULL, 

    DriverId int NOT NULL REFERENCES Drivers(DriverId),

    CONSTRAINT DateOfIssue CHECK (DateOfIssue > CAST('1900-01-01' as DATE)), 
    CONSTRAINT DateOfExpiration CHECK (DateOfExpiration > DateOfIssue), 
);

CREATE TABLE Fines (
    FineId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Cost int NOT NULL,
    Cause nvarchar(20) NOT NULL,
    IsPaid bit NOT NULL,

    DriverId int NOT NULL REFERENCES Drivers(DriverId),

    CONSTRAINT Cost CHECK (Cost > 0)
);

CREATE TABLE Accidents (
    AccidentId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    AmountParticipants int NOT NULL,
    AmountOfDied int NOT NULL,
    DateOfAccident DATE NOT NULL,

    CarId int NOT NULL REFERENCES Cars(CarId),
);
GO