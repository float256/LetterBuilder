IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'text_block')
BEGIN
	CREATE TABLE [dbo].[text_block](
		[id_text_block] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](max) NOT NULL,
		[text] [nvarchar](max) NOT NULL,
		[id_parent_catalog] [int] NOT NULL,
		[order_in_parent_directory] [int] NOT NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
