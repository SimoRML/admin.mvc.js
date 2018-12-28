Begin tran tr_commit_version;

	DECLARE @vid int set @vid={0};
	DECLARE @meta_bo_id int set @meta_bo_id={4};		
	DECLARE @vnum int set @vnum={5};
	
	
	if OBJECT_ID('{1}', 'U') IS NOT NULL
	BEGIN
		exec sp_rename '{1}', '{1}tmp';
	END;
	

	CREATE TABLE {1} (
	[BO_ID] bigint NOT NULL,
	{2}
	PRIMARY KEY ([BO_ID])
	);


	DECLARE @commit int set @commit = 1;
	if OBJECT_ID('{1}tmp', 'U') IS NOT NULL
	BEGIN
		exec MoveFromTmp @meta_bo_id;
		IF (select count(BO_ID) from {1}) = (select count(BO_ID) from {1}tmp) BEGIN
			DROP TABLE {1}tmp;
		END
		ELSE BEGIN
			set @commit = 0;
		END
	END
	
	update versions set STATUS='OLD', UPDATED_BY='{3}', UPDATED_DATE=getdate() where META_BO_ID=@meta_bo_id and STATUS not in('OLD');
	update versions set STATUS='ACTIVE' , UPDATED_BY='{3}', UPDATED_DATE=getdate() where VERSIONS_ID = @vid;
	update META_FIELD set STATUS='COMMITED V' + convert(varchar,@vid), [VERSION]=@vnum where META_BO_ID = @meta_bo_id and STATUS = 'NEW';
	update META_BO set [VERSION] = @vnum, UPDATED_BY='{3}', UPDATED_DATE=getdate(), STATUS = 'ACTIVE v' + convert(varchar,@vnum) where META_BO_ID = @meta_bo_id;
	insert into versions(META_BO_ID, NUM, SQLQUERY, CREATED_BY, CREATED_DATE, STATUS) 
				values(@meta_bo_id, @vnum+1, '[SQLQUERY]', '{3}', getdate(),'PENDING');

IF @commit = 1 
BEGIN
	commit tran tr_commit_version;
END
ELSE
BEGIN
	rollback tran tr_commit_version;
END