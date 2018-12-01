Begin tran tr_commit_version;

	DECLARE @vid int set @vid={0};
	DECLARE @meta_bo_id int set @meta_bo_id = {4};		
	DECLARE @vnum int; set @vnum = (select max(num) from VERSIONS where META_BO_ID = @meta_bo_id);
	DECLARE @table_name_version varchar(100) set @table_name_version = '{1}' + convert(varchar,(@vnum-1));
	
	if EXISTS(SELECT TABLE_NAME FROM FAIS.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME='{1}') 
	BEGIN
		exec sp_rename '{1}', @table_name_version;
		DROP VIEW VIEW_{1};
	END;

	CREATE TABLE {1} (
	[BO_ID] bigint NOT NULL,
	{2}
	[CREATED_BY] varchar(100) NULL ,
	[CREATED_DATE] datetime NULL DEFAULT getdate() ,
	[UPDATED_BY] varchar(100) NULL ,
	[UPDATED_DATE] datetime NULL DEFAULT getdate() ,
	[STATUS] varchar(10) NULL ,
	PRIMARY KEY ([BO_ID])
	);

	CREATE INDEX [IDX_CREATED_DATE] ON {1} ([CREATED_DATE] ASC);
	CREATE INDEX [IDX_CREATED_BY] ON {1} ([CREATED_BY] ASC);
	CREATE INDEX [IDX_UPDATED_DATE] ON {1} ([UPDATED_DATE] ASC);
	CREATE INDEX [IDX_UPDATED_BY] ON {1} ([UPDATED_BY] ASC);	

	update versions set STATUS='OLD', UPDATED_BY='{3}', UPDATED_DATE=getdate() where META_BO_ID=@meta_bo_id and STATUS not in('OLD');
	update versions set STATUS='ACTIVE' , UPDATED_BY='{3}', UPDATED_DATE=getdate() where VERSIONS_ID = @vid;
	update META_FIELD set STATUS='COMMITED V' + convert(varchar,@vid) where META_BO_ID = @meta_bo_id and STATUS = 'NEW';
	insert into versions(META_BO_ID, NUM, SQLQUERY, CREATED_BY, CREATED_DATE, STATUS) 
				values(@meta_bo_id, @vnum+1, '[SQLQUERY]', '{3}', getdate(),'PENDING');

commit tran tr_commit_version;