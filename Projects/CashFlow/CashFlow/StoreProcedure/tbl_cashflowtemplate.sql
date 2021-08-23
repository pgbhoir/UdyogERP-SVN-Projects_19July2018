/****** Object:  Table [dbo].[CASHFLOWTemplate]    Script Date: 22-11-2019 14:43:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = 'CASHFLOWTemplate' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[CASHFLOWTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[idno] [int] NULL,
	[srno] [int] NULL,
	[pID] [int] NULL,
	[particular] [varchar](100) NULL,
	[CFcur] [decimal](18, 2) NULL,
	[cfprev] [decimal](18, 2) NULL,
	[IsHead] [bit] NULL,
	[istotal] [bit] NULL,
 CONSTRAINT [PK_CASHFLOWTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
ELSE
BEGIN
	IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME = 'CASHFLOWTemplate'
                 AND COLUMN_NAME = 'idno') 
	BEGIN
		ALTER TABLE CASHFLOWTemplate ADD idno numeric(3)
	END


END


GO

SET ANSI_PADDING OFF
GO


