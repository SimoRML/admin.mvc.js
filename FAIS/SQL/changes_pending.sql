
GO
/****** Object:  StoredProcedure [dbo].[MoveFromTmp]    Script Date: 14-Jun-19 16:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
ALTER PROCEDURE [dbo].[MoveFromTmp]
	@metBoId int
AS
BEGIN
	print '-----[MoveFromTmp]-----';
	
	DECLARE @currentVersion int;
	DECLARE @table varchar(100);
		select @table = BO_DB_NAME, @currentVersion=[VERSION] from META_BO where META_BO_ID= @metBoId;
	DECLARE @table_tmp varchar(100); set @table_tmp = @table + 'tmp';
	
	print '@currentVersion : ' + convert(varchar,@table);
	print '@table : ' + convert(varchar,@table);
	print '@table_tmp : ' + convert(varchar,@table_tmp);

		
	DECLARE @oneField varchar(100);
	DECLARE @fields varchar(MAX) set @fields = '[BO_ID]';
			
	DECLARE fields_cursor CURSOR FOR 
		select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [STATUS] <> 'NEW' AND FORM_TYPE not like 'subform-%';

	OPEN fields_cursor  
	FETCH NEXT FROM fields_cursor INTO @oneField

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @fields += ', [' + @onefield + ']';
		FETCH NEXT FROM fields_cursor INTO @oneField
	END 

	CLOSE fields_cursor  
	DEALLOCATE fields_cursor 

	print '@fields : ' + @fields;
		
	Declare @insertStatement nvarchar(MAX);
		set @insertStatement = 'insert into ' + @table + '('+@fields+') select * from '+@table_tmp;
	print '@@insertStatement : ' + @insertStatement;

	exec sp_executesql @insertStatement;

	print '--------------------------------';
END

GO

USE [akkornet]
GO
/****** Object:  StoredProcedure [dbo].[MoveBoToCurrentVersion]    Script Date: 14-Jun-19 16:10:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
ALTER PROCEDURE [dbo].[MoveBoToCurrentVersion]
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
		DECLARE @fields varchar(MAX) set @fields = '[BO_ID]';
		
		
		DECLARE fields_cursor CURSOR FOR 
			select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [VERSION] <= @boVersion AND FORM_TYPE not like 'subform-%';

		OPEN fields_cursor  
		FETCH NEXT FROM fields_cursor INTO @oneField

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			set @fields += ', [' + @onefield + ']';
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
