SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
ALTER PROCEDURE MoveBoToCurrentVersion
	@BO_ID bigint
AS
BEGIN
	print '-----MoveBoToCurrentVersion-----';
	print '@BO_ID : ' + convert(varchar,@BO_ID);
	DECLARE @metBoId int;
	DECLARE @boVersion int;
		select @metBoId=BO_TYPE, @boVersion=[VERSION] from BO where BO_ID = @BO_ID;
	
	DECLARE @currentVersion int;
	DECLARE @boDbName varchar(100);
		select @boDbName = BO_DB_NAME, @currentVersion=[VERSION] from META_BO where META_BO_ID= @metBoId;
	DECLARE @currentTable varchar(100) set @currentTable = @boDbName + convert(varchar, @currentVersion);
	DECLARE @boTable varchar(100) set @boTable = @boDbName + convert(varchar, @boVersion);

	print '@metBoId : ' + convert(varchar,@metBoId);
	print '@boDbName : ' + @boDbName;
	print '@boVersion : ' + convert(varchar,@boVersion);
	print '@boTable : ' + @boTable;
	print '@currentVersion : ' + convert(varchar,@currentVersion);
	print '@currentTable : ' + @currentTable;

	IF @boVersion != @currentVersion BEGIN
		
		DECLARE @oneField varchar(100);
		DECLARE @fields varchar(MAX) set @fields = 'BO_ID';
		
		
		DECLARE fields_cursor CURSOR FOR 
			select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [VERSION] <= @boVersion AND FORM_TYPE not like 'subform-%';

		OPEN fields_cursor  
		FETCH NEXT FROM fields_cursor INTO @oneField

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			set @fields += ', ' + @onefield;
			FETCH NEXT FROM fields_cursor INTO @oneField
		END 

		CLOSE fields_cursor  
		DEALLOCATE fields_cursor 

		print '@fields : ' + @fields;
		
		Declare @insertStatement nvarchar(MAX);
			set @insertStatement = 'insert into ' + @currentTable + '('+@fields+') select * from '+@boTable+' where BO_ID=' + convert(varchar,@BO_ID);
		print '@@insertStatement : ' + @insertStatement;

		Declare @deleteStatement nvarchar(MAX);
			set @deleteStatement = 'delete from '+@boTable+' where BO_ID=' + convert(varchar,@BO_ID);
		print '@@deleteStatement  : ' + @deleteStatement ;

		BEGIN TRAN TR_SWITCH;
			exec sp_executesql @insertStatement;
			exec sp_executesql @deleteStatement;
			update BO set [VERSION]=@currentVersion where BO_ID = @BO_ID;
		COMMIT TRAN TR_SWITCH;
	END
	print '--------------------------------';
END
GO
--exec MoveBoToCurrentVersion 14;
--exec MoveBoToCurrentVersion 16;
--exec MoveBoToCurrentVersion 17;
