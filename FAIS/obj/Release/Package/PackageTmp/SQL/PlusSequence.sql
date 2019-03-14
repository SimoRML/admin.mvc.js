/****** Object:  StoredProcedure [dbo].[NextID]    Script Date: 05-Mar-19 13:31:56 ******/
GO

/****** Object:  Table [dbo].[SourceSequence]    Script Date: 05-Mar-19 13:39:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlusSequence](
	[SequenceID] [int] IDENTITY(1,1) NOT NULL,
	[cle] [varchar](500) NULL,
	[TableName] [varchar](50) NULL,
	[StartValue] [int] NULL,
	[StepBy] [int] NULL,
	[CurrentValue] [int] NULL,
 CONSTRAINT [PK_SourceSequence] PRIMARY KEY CLUSTERED 
(
	[SequenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


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
	@stepBy int
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

	update PlusSequence set CurrentValue = @CurrentValue where SequenceID = @SequenceID;

	select convert(varchar,@CurrentValue);
END


GO