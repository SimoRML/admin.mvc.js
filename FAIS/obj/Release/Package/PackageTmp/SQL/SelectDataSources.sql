SELECT
  t.name as TABLE_NAME, 
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
FROM
  SYSOBJECTS t inner join INFORMATION_SCHEMA.COLUMNS c on t.name = c.TABLE_NAME
WHERE xtype in ( 'U', 'V')
AND t.name like '%_BO_%' 
OR t.name like '__' 
order by TABLE_NAME, [Field]