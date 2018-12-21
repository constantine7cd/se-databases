drop trigger if exists deleteCarsTrigger

CREATE TRIGGER deleteCarsTrigger  
ON Fines 
FOR DELETE  
AS  
EXTERNAL NAME Lab04.Triggers.fifth 

delete Fines
where FineId = 2