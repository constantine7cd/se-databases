use GIBDD;
go

/*
--N1 ????????????? ????????? ???????
if OBJECT_ID (N'getMaxCostUnpaid', N'FN') IS NOT NULL
    drop function getMaxCostUnpaid;
go

create function getMaxCostUnpaid (@DriverId int)
returns int
as
begin
	declare @ret int;
	select @ret = max(Cost)
	from Fines as f
	where DriverId = @DriverId and IsPaid = 0
		if (@ret is null)
			set @ret = 0
	
	return @ret;
end;
go

select DriverId, dbo.getMaxCostUnpaid (DriverId) as maxFine
from Fines
where DriverId between 30 and 40
group by DriverId

go


--N2 ????????????? ????????????? ????????? ???????

if OBJECT_ID (N'CarAvgYear', N'IF') is not null
	drop function CarAvgYear;
go

create function CarAvgYear()
returns table
as
return
(
	select CarName, AVG(year(CarDate)) as AVG_YEAR
	from Cars
	group by CarName
	having AVG(year(CarDate)) > 
		(
			select AVG(year(CarDate)) as AVG_GEN
			from Cars
		)
);
go

select * from CarAvgYear()


--N3 ????????????? ???????????????? ????????? ???????

if OBJECT_ID (N'fineCostAndCarDateHigherThatAvg', N'IF') is not null
	drop function fineCostAndCarDateHigherThatAvg;
go

create function fineCostAndCarDateHigherThatAvg (@ExperienceLeft int, @ExperienceRight int)
returns @resTable table
(
	LicenseId int primary key not null,
	DriverId int not null,
	DateOfExp date not null,
	Experience int not null
)
as
begin
	insert @resTable
	select dl.LicenseId, dl.DriverId, dl.DateOfExpiration, d.Experience 
	from DriverLicense as dl join Drivers as d on dl.DriverId = d.DriverId 
	where d.DriverId in
		(
			select DriverId 
			from Fines as f
			where Cost > all
			(
				select avg(cost)
				from Fines
			) and DriverId in
				(
					select DriverId
					from Cars as c join CarsDrivers as cd on c.CarId = cd.CarId
					where year(CarDate) > all
						(
							select avg(year(CarDate))
							from Cars
						)
				)
		) and d.Experience between @ExperienceLeft and @ExperienceRight

	return 
end;
go

select * from dbo.fineCostAndCarDateHigherThatAvg(5, 15)


--N4 ??????? ? ??????????? ???

if OBJECT_ID(N'getDepartmentHierarchyTable', N'IF') is not null
	drop function getDepartmentHierarchyTable;
go

create function getDepartmentHierarchyTable()
returns @resTable table
(
	DepartmentId int primary key not null,
	DepartmentName nvarchar(30) not null,
	HigherDepartmentId int null,
	AmountOfEmployees int not null,
	Prioroty int not null
)
as
begin
	;with dep_hierarchy(DepartmentId, DepartmentName, HigherDepartmentId, AmountOfEmployees, Priority)
	as 
	(
		select d.DepartmentId, d.DepartmentName, d.HigherDepartmentId, d.AmountOfEmployees, 0 as Priority
		from departments as d
		where d.HigherDepartmentId is null
		union all
		select e.DepartmentId, e.DepartmentName, e.HigherDepartmentId, e.AmountOfEmployees, Priority + 1
		from departments as e join dep_hierarchy as dep on e.HigherDepartmentId = dep.DepartmentId
	)
	insert @resTable
	select DepartmentId, DepartmentName, HigherDepartmentId, AmountOfEmployees, Priority
	from dep_hierarchy
	return

end;
go

select *from getDepartmentHierarchyTable()
*/

--???????? ?????????
--N1 ???????? ????????? ??? ?????????? ??? ? ???????????

/*
create procedure updateFines
	@Cause nvarchar(50),
	@Factor int
as
	update dbo.Fines 
	set Cost = Cost * @Factor
	where Cause = @Cause
go

exec updateFines N'busline', 1.5
*/
drop procedure if exists getDriversWithExperience

create procedure getDriversWithExperience
	@Did int
as
	select AVG(Experience)
	from Drivers
	where DriverId > @Did
return;
go

exec getDriversWithExperience 5

--N2 ??????????? ???????? ????????? ??? ???????? ???????? ? ??????????? ???
/*
drop procedure if exists getDepartmentHierarchy;
go

create procedure getDepartmentHierarchy
as
	with dep_hierarchy(DepartmentId, DepartmentName, HigherDepartmentId, AmountOfEmployees, Priority)
	as 
	(
		select d.DepartmentId, d.DepartmentName, d.HigherDepartmentId, d.AmountOfEmployees, 0 as Priority
		from departments as d
		where d.HigherDepartmentId is null
		union all
		select e.DepartmentId, e.DepartmentName, e.HigherDepartmentId, e.AmountOfEmployees, Priority + 1
		from departments as e join dep_hierarchy as dep on e.HigherDepartmentId = dep.DepartmentId
	)
	select *
	from dep_hierarchy;
go

drop procedure if exists sadas;
go



exec getDepartmentHierarchy
*/

--N3 ???????? ????????? ? ????????
/*
drop procedure if exists selectCarBody;  
go

create procedure selectCarBody
	@CarCursor cursor varying output,
	@Body nvarchar(20)
as
	set @CarCursor = cursor for
		select CarName, CarVin
		from dbo.Cars
		where CarBody = @Body;

	open @CarCursor;

	fetch next from @CarCursor;
go


declare @Ccursor cursor;
declare @name nvarchar(20), @vin nvarchar(20);
exec selectCarBody @CarCursor = @Ccursor output, @Body = N'suv';
while @@FETCH_STATUS = 0
begin
	fetch next from @Ccursor into @name, @vin
	print @name + ' ' +  @vin
end;

close @Ccursor;
deallocate @Ccursor;
go
*/

/*
create procedure selectCarByBody
	@Body nvarchar(50)
as
	declare carCursor cursor scroll for 
	select * from Cars
	where CarBody = @Body;

	open carCursor;
	fetch first from carCursor;
	while @@FETCH_STATUS = 0 
	begin
		fetch next from carCursor
	end;

	close carCursor;
	deallocate carCursor;
go
*/
--N4 ???????? ????????? ??????? ? ??????????
/*
drop procedure if exists objInfo;  
go

create procedure objInfo
	@ObjName nvarchar(20)
as
	declare @MId int;
	set @MId = OBJECT_ID(@ObjName);
	select name, object_id, type_desc
	from sys.objects
	where name = OBJECT_NAME(@MId);
go

exec objInfo N'selectCarByBody'
go
*/

--DML ????????
--N1 ??????? After
/*
drop trigger if exists deleteDeniedFines;
go

create trigger deleteDeniedFines
on Fines
after delete
as raiserror('Delete request is denied', 16, 10);
go

delete Fines
where FineId = 115
*/

--N2 ??????? Instead of
/*
drop trigger if exists softDelete;
go

create trigger softDelete
on Fines
instead of delete
as
update Fines
set IsPaid = 1
where FineId  = (select FineId from deleted)
*/

/*
select * from Fines
where FineId = 8

delete Fines
where FineId = 8 

select * from Fines
where FineId = 8
*/
/*
create table InsCouts 
(
	row_count int not null
);
go
*/


drop trigger if exists countInserts;
go

create trigger countInserts
on dbo.Cars
after insert
as
insert InsCouts(row_count)
select count(*) from inserted;
go


insert dbo.Cars(CarName, CarVin, CarDate, CarBody)
values ('bmw', '234rfwe3', cast('2018-10-01' as date), 'targa'),
('audi', '234jkh', cast('2015-10-01' as date), 'coupe'),
('wolkswagen', '5456hkjh', cast('2014-10-01' as date), 'hatch'),
('merc', 'dfg0989', cast('2012-10-01' as date), 'coupe')

select * from InsCouts
go

--select * from Cars

delete InsCouts;
go




create procedure sadas
as 
	with tmp(col1, col2)
	as
		(
			select *, ROW_NUMBER() over (partition by asd, asd2 order by asd) as n
			from fds
		)
	delete from tmp
	where n > 2;
go


