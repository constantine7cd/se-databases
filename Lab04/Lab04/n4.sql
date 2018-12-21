drop procedure if exists updFines;
go

create procedure updFines
	@Cause nvarchar(50),
	@Factor float
as external name Lab04.fourth.updateFines;
go

exec updFines N'busline', 1.5;
go