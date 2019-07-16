USE [YODA]
GO
/****** Object:  StoredProcedure [dbo].[PlusSequenceNextID]    Script Date: 05-Apr-19 11:47:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SimoRML>
-- Create date: <08/02/2019>
-- Description:	<Identity by source (ex: BLABLA_513)>
-- =============================================
ALTER PROCEDURE [dbo].[PlusSequenceNextID]
	@cle varchar(500),
	@TableName varchar(500),
	@stepBy int,
	@presist int
AS
BEGIN
	SET NOCOUNT ON; 

	DECLARE @SequenceID int;
	DECLARE @StartValue int;
	DECLARE @CurrentValue int;
	-- INITIATE SEQUENCE
	select @SequenceID = SequenceID from PlusSequence where cle = @cle AND TableName = @TableName
    if @SequenceID is null
	Begin
		insert into PlusSequence (cle, TableName, StartValue, StepBy, CurrentValue)
		values (@cle, @TableName, 1, @stepBy, 0);
		SELECT @SequenceID = SCOPE_IDENTITY();
	END	

	-- CALCULATE NEXT ID
	select @StartValue=StartValue, @StepBy=StepBy, @CurrentValue=CurrentValue from PlusSequence where SequenceID = @SequenceID;

	if @CurrentValue < @StartValue 
	begin
		SET @CurrentValue = @StartValue;
	end
	else
	begin		
		SET @CurrentValue = @CurrentValue + @StepBy;
	end

	if @presist = 1
	begin
		update PlusSequence set CurrentValue = @CurrentValue where SequenceID = @SequenceID;
	end
	select convert(varchar,@CurrentValue);
END
