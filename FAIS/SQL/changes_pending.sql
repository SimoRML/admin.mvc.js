
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

GO

UPDATE [dbo].[META_FIELD] SET [FORM_OPTIONAL] = 1 WHERE META_FIELD_ID = 6;

/****** Object:  StoredProcedure [dbo].[InitMetaBo]    Script Date: 19-Jun-19 12:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InitMetaBo]
AS
BEGIN

IF not exists( SELECT object_id 
FROM sys.indexes 
WHERE name='UNIQUE_META_BO_BO_DB_NAME' AND object_id = OBJECT_ID('dbo.META_BO')
)
BEGIN
       CREATE UNIQUE INDEX UNIQUE_META_BO_BO_DB_NAME   
   ON dbo.META_BO (BO_DB_NAME);   
END

IF not exists( SELECT object_id 
FROM sys.indexes 
WHERE name='UNIQUE_META_FIELD_DB_NAME' AND object_id = OBJECT_ID('dbo.META_FIELD')
)
BEGIN
       CREATE UNIQUE INDEX UNIQUE_META_FIELD_DB_NAME
       ON dbo.META_FIELD (META_BO_ID, DB_NAME);   
END

delete from META_FIELD;
delete from VERSIONS;
delete from META_BO;
delete from bo;
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;

SET IDENTITY_INSERT [dbo].[META_BO] ON ;
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE]) 
VALUES (1, N'META_BO', 1, N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'-1', N'META_BO', NULL);

INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE]) 
VALUES (2, N'WORKFLOW', 1, N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'-1', N'WORKFLOW', NULL);
/*PAGE */
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE], [JSON_DATA]) VALUES (3, N'Page', 1, N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'-1', N'PAGE', N'form', N'{"TITLE":"","GROUPE":null}')
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] ON ;
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (1, 1, N'META_BO_ID', N'bigint', 0, N'#', NULL, 0, N'', NULL, N'', NULL, 0, 0, NULL, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'PK', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (2, 1, N'BO_NAME', N'varchar(100)', 1, N'Nom', NULL, 1, N'Nom', NULL, N'v-text', NULL, 1, 0, 0, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'amdin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (3, 1, N'STATUS', N'varchar(50)', 1, N'Nom', NULL, 1, N'STATUS', NULL, N'v-label', NULL, 1, 0, NULL, N'PENDING', N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (4, 1, N'TYPE', N'varchar(50)', 1, N'Type', N'{"fct":"Display","source":"TYPE"}', 1, N'Type', NULL, N'v-select', N'[{ "Value": "form", "Display": "FORM" }, { "Value": "subform", "Display": "SUB FORM" }]', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'LOCKED', NULL);

INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (5, 2, N'LIBELLE', N'varchar(50)', 1, N'Libelle', '', 1, N'Libelle', NULL, N'v-text', '', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (6, 2, N'ACTIVE', N'int', 1, N'Active', '', 1, N'Active', NULL, N'v-checkbox', '', 1, 1, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (7, 2, N'additional.column', '', 1, N'', '{"type":"button", "color":"btn-success","icon":"add","action":"redirect", "data":"#workflow.home"}', 1, N'', NULL, N'', '', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
/*PAGE FIELD*/
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) 
VALUES (8, 3, N'TITLE', N'varchar(100)', 1, N'TITLE', NULL, 1, N'TITLE', NULL, N'v-text', N'', 1, 0, 0, NULL, N'admin', NULL, N'admin', NULL, N'LOCKED', NULL, N'')
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (9, 3, N'additional.column', '', 1, N'', '{"type":"button", "color":"btn-success","icon":"settings","action":"redirect", "data":"#admin.page"}', 1, N'', NULL, N'', '', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);

SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;

DECLARE @tname varchar(100);
DECLARE @sql nvarchar(100);
DECLARE _bo_cursor CURSOR FOR 
             SELECT t.name FROM SYSOBJECTS t WHERE xtype in ( 'U' ) AND (t.name like '%_BO_%' );

OPEN _bo_cursor  
FETCH NEXT FROM _bo_cursor INTO @tname

WHILE @@FETCH_STATUS = 0  
BEGIN  
       SET @sql = 'DROP TABLE ' + @tname;
       EXEC sp_executesql @sql;
       FETCH NEXT FROM _bo_cursor INTO @tname
END 

CLOSE _bo_cursor  
DEALLOCATE _bo_cursor 

END

GO
