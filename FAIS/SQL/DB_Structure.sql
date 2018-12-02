USE [FAIS]
GO
/****** Object:  Table [dbo].[BO]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BO](
	[BO_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [char](10) NULL,
	[BO_TYPE] [varchar](100) NULL,
	[VERSION] [int] NULL,
 CONSTRAINT [PK__BO__D4ABCFC65599051D] PRIMARY KEY CLUSTERED 
(
	[BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BO_CHILDS]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BO_CHILDS](
	[BO_PARENT_ID] [bigint] NOT NULL,
	[BO_CHILD_ID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_PARENT_ID] ASC,
	[BO_CHILD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[META_BO]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[META_BO](
	[META_BO_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BO_NAME] [varchar](100) NULL,
	[VERSION] [int] NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](50) NULL,
	[BO_DB_NAME] [varchar](50) NULL,
 CONSTRAINT [PK__META_BO__28ADAC72D031570B] PRIMARY KEY CLUSTERED 
(
	[META_BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[META_FIELD]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[META_FIELD](
	[META_FIELD_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[META_BO_ID] [bigint] NOT NULL,
	[DB_NAME] [varchar](100) NOT NULL,
	[DB_TYPE] [varchar](20) NOT NULL,
	[DB_NULL] [int] NOT NULL,
	[GRID_NAME] [varchar](100) NOT NULL,
	[GRID_FORMAT] [varchar](100) NULL,
	[GRID_SHOW] [int] NULL,
	[FORM_NAME] [varchar](100) NULL,
	[FORM_FORMAT] [varchar](100) NULL,
	[FORM_TYPE] [varchar](100) NOT NULL,
	[FORM_SOURCE] [varchar](1000) NULL,
	[FORM_SHOW] [int] NULL,
	[FORM_OPTIONAL] [int] NULL,
	[IS_FILTER] [int] NULL,
	[FORM_DEFAULT] [varchar](100) NULL,
	[CREATED_BY] [varchar](100) NOT NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](50) NULL,
	[VERSION] [int] NULL,
 CONSTRAINT [PK__META_FIE__604B6642F2AB887C] PRIMARY KEY CLUSTERED 
(
	[META_FIELD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VERSIONS]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VERSIONS](
	[VERSIONS_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[META_BO_ID] [bigint] NULL,
	[NUM] [int] NOT NULL,
	[SQLQUERY] [varchar](max) NOT NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[VERSIONS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BO] ADD  CONSTRAINT [DF__BO__CREATED_DATE__72C60C4A]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[BO] ADD  CONSTRAINT [DF__BO__UPDATED_DATE__73BA3083]  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_BO] ADD  CONSTRAINT [DF__META_BO__CREATED__239E4DCF]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[META_BO] ADD  CONSTRAINT [DF__META_BO__UPDATED__24927208]  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF__META_FIEL__DB_NU__2F10007B]  DEFAULT ((1)) FOR [DB_NULL]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF__META_FIEL__GRID___300424B4]  DEFAULT ((1)) FOR [GRID_SHOW]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF__META_FIEL__FORM___30F848ED]  DEFAULT ((1)) FOR [FORM_SHOW]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF_META_FIELD_FORM_OPTIONAL]  DEFAULT ((0)) FOR [FORM_OPTIONAL]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF_META_FIELD_IS_FILTER]  DEFAULT ((0)) FOR [IS_FILTER]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF__META_FIEL__CREAT__31EC6D26]  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF__META_FIEL__UPDAT__32E0915F]  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  CONSTRAINT [DF_META_FIELD_STATUS]  DEFAULT ('ACTIVE') FOR [STATUS]
GO
ALTER TABLE [dbo].[VERSIONS] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[VERSIONS] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD]  WITH CHECK ADD  CONSTRAINT [FK__META_FIEL__META___33D4B598] FOREIGN KEY([META_BO_ID])
REFERENCES [dbo].[META_BO] ([META_BO_ID])
GO
ALTER TABLE [dbo].[META_FIELD] CHECK CONSTRAINT [FK__META_FIEL__META___33D4B598]
GO
ALTER TABLE [dbo].[VERSIONS]  WITH CHECK ADD  CONSTRAINT [FK__VERSIONS__META_B__4BAC3F29] FOREIGN KEY([META_BO_ID])
REFERENCES [dbo].[META_BO] ([META_BO_ID])
GO
ALTER TABLE [dbo].[VERSIONS] CHECK CONSTRAINT [FK__VERSIONS__META_B__4BAC3F29]
GO
/****** Object:  StoredProcedure [dbo].[cleanMetaBo]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--/****** Script de la commande SelectTopNRows à partir de SSMS  ******/
--SELECT * from VERSIONS where META_BO_ID=11


--select * from META_FIELD where META_BO_ID=11

--select * from VIEW_KA_BO_





CREATE procedure [dbo].[cleanMetaBo]
as
delete from META_FIELD where META_BO_ID not in (select META_BO_ID from META_BO where STATUS ='-1')
delete from VERSIONS where META_BO_ID not in (select META_BO_ID from META_BO where STATUS ='-1')
delete from META_BO where STATUS <>'-1'
delete from bo
GO
/****** Object:  StoredProcedure [dbo].[InitMetaBo]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InitMetaBo]
AS
BEGIN

delete from META_FIELD;
delete from VERSIONS;
delete from META_BO;
delete from bo;
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;
SET IDENTITY_INSERT [dbo].[META_BO] ON;
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME])
VALUES (1, N'META_BO', 1, N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'-1', N'META_BO');
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] ON; 
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION])
VALUES (1, 1, N'META_BO_ID', N'bigint', 0, N'#', NULL, 0, N'', NULL, N'', NULL, 0, 0, NULL, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'PK', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION])
VALUES (2, 1, N'BO_NAME', N'varchar(100)', 1, N'Nom', NULL, 1, N'Nom', NULL, N'v-text', NULL, 1, 1, NULL, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'amdin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION])
VALUES (3, 1, N'STATUS', N'char(10)', 1, N'Nom', NULL, 1, N'STATUS', NULL, N'v-label', NULL, 1, 0, NULL, N'PENDING', N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;

END
GO
/****** Object:  StoredProcedure [dbo].[MoveBoToCurrentVersion]    Script Date: 02-Dec-18 4:15:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
CREATE PROCEDURE [dbo].[MoveBoToCurrentVersion]
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
			select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [VERSION] <= @boVersion;

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

exec [InitMetaBo]

GO