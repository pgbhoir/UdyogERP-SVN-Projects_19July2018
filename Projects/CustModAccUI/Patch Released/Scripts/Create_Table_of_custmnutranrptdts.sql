IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE [NAME]='custmnutranrptdts' AND XTYPE='U')
BEGIN
CREATE TABLE [dbo].[custmnutranrptdts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[fk_id] [varchar](5) NULL,
	[ccomp] [varchar](100) NULL,
	[optiontype] [varchar](10) NULL,
	[desc1] [varchar](150) NULL,
	[desc2] [varchar](150) NULL,
	[desc3] [varchar](150) NULL
) ON [PRIMARY]
END