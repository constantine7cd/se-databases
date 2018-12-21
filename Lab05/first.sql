/*
RAW - ??? ?????? ?????? ????????? ????????? ??????? <row>

AUTO - ????????????, ? ??????????? ?? ?????? ??????????? select

EXPLICIT

PATH
*/

use GIBDD;
go

select * from Accidents
for xml auto

select * from Cars
where CarId between 10 and 15
for xml raw

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
	)
for xml auto


select Cost, Cause, IsPaid, Experience
from Fines f join
	(
		select DriverId, Experience
		from Drivers
		where Experience > all
		(
			select AVG(Experience)
			from Drivers
		)
	) as exp_ on f.DriverId = exp_.DriverId
union
select Cost, Cause, IsPaid, Experience
from Fines cf join
	(
		select d.DriverId, Experience
		from Drivers as d join Fines as f on f.DriverId = d.DriverId
		where Cost > all
		(
			select avg(cost)
			from fines
		)

	) as cost on cf.DriverId = cost.DriverId
for xml auto



select 1    as Tag,  
       null as Parent,  
       d.DriverId as [Drivers!1!Did],  
       null       as [Fines!2!FineId],  
       null       as [Fines!2!Cause],
	   null		  as [FCost!3!Cost]
from   dbo.Drivers as d  
inner join Fines as f
ON  d.DriverId = f.DriverId
UNION ALL  
SELECT 2 as Tag,  
       1 as Parent,  
       d.DriverId,
       f.FineId,
	   f.Cause,
	   null
FROM   dbo.Drivers AS d  
INNER JOIN Fines AS f  
ON  d.DriverId = f.DriverId
UNION ALL  
SELECT 3 as Tag,  
       2 as Parent,  
       d.DriverId,
       f.FineId,
	   null,
	   f.Cost
FROM   dbo.Drivers AS d  
INNER JOIN Fines AS f  
ON  d.DriverId = f.DriverId
order by [Drivers!1!Did], [Fines!2!FineId], [FCost!3!Cost]
for xml explicit;


select d.DriverId "Did",
	f.FineId "Fines/FineId",
	f.Cause "Fines/Cause"
from Drivers as d inner join Fines as f
on d.DriverId = f.DriverId
order by d.DriverId, f.FineId
for xml path;
go
