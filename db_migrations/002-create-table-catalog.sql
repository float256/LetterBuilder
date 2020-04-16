IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'catalog')
BEGIN
	CREATE TABLE [dbo].[catalog](
		[id_catalog] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](max) NOT NULL,
		[id_parent_catalog] [int] NOT NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
