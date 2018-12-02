Begin tran tr_commit_version;

	DECLARE @vid int set @vid={0};
	DECLARE @meta_bo_id int set @meta_bo_id = {4};		
	DECLARE @vnum int set @vnum = {5};
	
	
	if @vnum > 1
	BEGIN
		DROP VIEW VIEW_{6};
	END;
	

	CREATE TABLE {1} (
	[BO_ID] bigint NOT NULL,
	{2}
	PRIMARY KEY ([BO_ID])
	);

	update versions set STATUS='OLD', UPDATED_BY='{3}', UPDATED_DATE=getdate() where META_BO_ID=@meta_bo_id and STATUS not in('OLD');
	update versions set STATUS='ACTIVE' , UPDATED_BY='{3}', UPDATED_DATE=getdate() where VERSIONS_ID = @vid;
	update META_FIELD set STATUS='COMMITED V' + convert(varchar,@vid), [VERSION]=@vnum where META_BO_ID = @meta_bo_id and STATUS = 'NEW';
	update META_BO set [VERSION] = @vnum, UPDATED_BY='{3}', UPDATED_DATE=getdate(), STATUS = 'ACTIVE v' + convert(varchar,@vnum) where META_BO_ID = @meta_bo_id;
	insert into versions(META_BO_ID, NUM, SQLQUERY, CREATED_BY, CREATED_DATE, STATUS) 
				values(@meta_bo_id, @vnum+1, '[SQLQUERY]', '{3}', getdate(),'PENDING');

commit tran tr_commit_version;