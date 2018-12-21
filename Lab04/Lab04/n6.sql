CREATE TYPE dbo.Vector  
EXTERNAL NAME Lab04.Vector3f

CREATE TABLE dbo.Test
( 
  id INT IDENTITY(1,1) NOT NULL, 
  v Vector NULL,
);
GO

INSERT INTO dbo.Test(v) VALUES('2, 6, 1'); 
INSERT INTO dbo.Test(v) VALUES('1,0, 15'); 
INSERT INTO dbo.Test(v) VALUES('2, 1,8');  
GO 

SELECT * FROM dbo.Test;

SELECT id, v.ToString() AS Point 
FROM dbo.Test;

DECLARE @v dbo.Vector
SET @v = CAST('3, 1, 8' AS dbo.Vector)
SELECT @v.Length() AS 'Length'
GO
 
DROP TABLE dbo.Test
GO

DROP TYPE Vector
GO

