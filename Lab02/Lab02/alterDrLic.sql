use GIBDD
--truncate table DriverLicense;
ALTER TABLE DriverLicense 
ADD SerialNum nvarchar(20) NOT NULL;

