declare @ixml int
declare @xml xml
select @xml =
(
	select * from openrowset(bulk 'C:\Users\Konstantin\Documents\db\Lab05\cars.xml', 
                                single_blob) as data
)

exec sp_xml_preparedocument @ixml output, @xml

insert into Cars(CarName, CarVin, CarDate, CarBody)
select *
from openxml (@ixml, '/note/row')
with(CarName varchar(20), CarVin varchar(20), CarDate date, CarBody nvarchar(20))
exec sp_xml_removedocument @ixml

select * from Cars