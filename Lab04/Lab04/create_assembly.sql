IF EXISTS (SELECT name FROM sys.assemblies WHERE name = 'Lab04')  
   DROP ASSEMBLY Lab04;  
go  

CREATE ASSEMBLY  Lab04 from 'C:\Users\Konstantin\Documents\db\Lab04\Lab04\bin\Debug\Lab04.dll';  
GO  

ALTER ASSEMBLY Lab04 FROM 'C:\Users\Konstantin\Documents\db\Lab04\Lab04\bin\Debug\Lab04.dll';  
GO  