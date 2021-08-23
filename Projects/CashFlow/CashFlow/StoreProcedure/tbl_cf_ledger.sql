/****** Object:  Table [dbo].[CF_LEDGER]    Script Date: 22-11-2019 14:46:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = 'CF_LEDGER' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[CF_LEDGER](
	[led_id] [int] IDENTITY(1,1) NOT NULL,
	[pid] [int] NULL,
	[Particular] [varchar](100) NULL,
	[ac_id] [int] NULL,
	[ac_name] [varchar](50) NULL,
	[cf_type] [char](1) NULL,
 CONSTRAINT [PK_CF_LEDGER] PRIMARY KEY CLUSTERED 
(
	[led_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
End

GO

SET ANSI_PADDING OFF
GO


