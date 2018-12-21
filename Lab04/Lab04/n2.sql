use GIBDD;
go

IF EXISTS (SELECT name FROM sysobjects WHERE name = 'AvgFines')  
   DROP AGGREGATE AvgFines;  
go  

CREATE AGGREGATE AvgFines (@value int, @isPaid bit) RETURNS int  
EXTERNAL NAME Lab04.AvgWithCondition;  
go  

select dbo.AvgFines(Cost, IsPaid) as 'Correct avg fines' from Fines;
go

