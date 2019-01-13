SELECT t.name as TABLE_NAME, 
		  column_name AS [Field],
			DATA_TYPE  AS [Type],
			IS_NULLABLE AS [Null],

			case when column_name='BO_ID' or exists(select * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
							WHERE  column_name = c.column_name and table_name=c.table_name
							and CONSTRAINT_NAME like 'PK_%')
			then 'PRI'
			else '' end as [Key],

			COLUMN_DEFAULT as [Default]
			,case when COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1
			then 'auto_increment' else '' end as [Extra]
			,
			(
				select top 1 FORM_SOURCE from META_FIELD join META_BO on META_FIELD.META_BO_ID = META_BO.META_BO_ID  
				where META_FIELD.DB_NAME = column_name AND META_BO.BO_DB_NAME = t.name
			) as FORM_SOURCE
		FROM
		  SYSOBJECTS t inner join INFORMATION_SCHEMA.COLUMNS c on t.name = c.TABLE_NAME
		WHERE xtype in ('U', 'V')
		AND (t.name like '%_BO_%' OR t.name like 'liste_%' )
order by TABLE_NAME, [Field]