set dateformat dmy
BULK INSERT GIBDD.dbo.Cars
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\cars.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);

BULK INSERT GIBDD.dbo.Accidents
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\accidents.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);

BULK INSERT GIBDD.dbo.CarsDrivers
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\cars_drivers.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);

BULK INSERT GIBDD.dbo.DriverLicense
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\driver_license.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);

BULK INSERT GIBDD.dbo.Drivers
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\drivers.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);

BULK INSERT GIBDD.dbo.Fines
FROM 'C:\Users\Konstantin\Documents\SQL Server Management Studio\Lab02\Lab02\data_csv\fine.csv'
WITH (
	firstrow = 2,
	fieldterminator = ',',
	rowterminator = '\n'
);



