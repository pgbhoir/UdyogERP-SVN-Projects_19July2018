IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='custfeature' AND XTYPE='U')
BEGIN
CREATE TABLE [dbo].[custfeature](
	[id] [varchar](5) NULL,
	[date] [smalldatetime] NULL,
	[rcomp] [varchar](100) NULL,
	[prodname] [varchar](50) NULL,
	[prodver] [varchar](15) NULL,
	[rmacid] [varchar](100) NULL,
	[bug] [varchar](50) NULL,
	[pono] [varchar](20) NULL,
	[podate] [smalldatetime] NULL,
	[poamt] [numeric](12, 2) NULL,
	[apprby] [varchar](30) NULL,
	[remarks] [varchar](100) NULL,
	[user_name] [varchar](30) NULL
) ON [PRIMARY]
END