IF EXISTS (SELECT name FROM sysobjects WHERE name = 'CountFines')  
   DROP FUNCTION CountFines;  
go  

CREATE FUNCTION CountFines() RETURNS INT   
AS EXTERNAL NAME Lab04.first.ReturnFineCount;   
GO  

SELECT dbo.CountFines();  
GO  