IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'picture')
BEGIN
	CREATE TABLE [dbo].[picture](
		[id_picture] [int] IDENTITY(1,1) NOT NULL,
		[binary_data] [varbinary](max) NOT NULL,
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
