﻿CREATE TABLE BO (
[BO_ID] bigint NOT NULL IDENTITY(1,1) ,
[CREATED_BY] varchar(100) NULL ,
[CREATED_DATE] datetime NULL DEFAULT getdate() ,
[UPDATED_BY] varchar(100) NULL ,
[UPDATED_DATE] datetime NULL DEFAULT getdate() ,
[STATUS] char(10) NULL ,
BO_TYPE varchar(100) NULL,
PRIMARY KEY ([BO_ID])
)

GO

CREATE INDEX [IDX_CREATED_DATE] ON BO
([CREATED_DATE] ASC) 
GO

CREATE INDEX [IDX_CREATED_BY] ON BO
([CREATED_BY] ASC) 
GO

CREATE INDEX [IDX_UPDATED_DATE] ON BO
([UPDATED_DATE] ASC) 
GO

CREATE INDEX [IDX_UPDATED_BY] ON BO
([UPDATED_BY] ASC) 
GO

CREATE TABLE BO_CHILDS (
[BO_PARENT_ID] bigint NOT NULL ,
[BO_CHILD_ID] bigint NOT NULL ,
PRIMARY KEY ([BO_PARENT_ID], [BO_CHILD_ID])
)

GO
