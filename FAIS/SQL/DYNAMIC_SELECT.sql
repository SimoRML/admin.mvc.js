
CREATE PROCEDURE DYNAMIC_SELECT
	@META_BO_ID int
AS
DECLARE @selecta nvarchar(max) set @selecta = ' SELECT '
	+ ( select STRING_AGG(DB_NAME, ',')  	from META_FIELD  	where META_BO_ID=@META_BO_ID  	AND STATUS not like '%deleted%' )
	+ ' FROM ' 
	+ (select BO_DB_NAME from META_BO where META_BO_ID=@META_BO_ID);
exec sp_executesql @selecta