IF EXISTS (SELECT name FROM sysobjects WHERE name = 'AccFraction')  
   DROP FUNCTION AccFraction;  
go  

CREATE FUNCTION AccFraction(@dateSince datetime)   
RETURNS TABLE (  
   AccId int,  
   Fraction float
)  
AS EXTERNAL NAME Lab04.third.Accidents
go  

SELECT * FROM AccFraction('2000-01-01');  
go  