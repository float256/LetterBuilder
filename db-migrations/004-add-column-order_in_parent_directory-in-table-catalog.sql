IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'order_in_parent_directory' AND TABLE_NAME='catalog')
	ALTER TABLE [dbo].[catalog] ADD [order_in_parent_directory] [INT] NULL;
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'order_in_parent_directory' AND TABLE_NAME='catalog' AND IS_NULLABLE='NO')
BEGIN
	UPDATE [dbo].[catalog] SET [order_in_parent_directory] = [id_catalog];
	ALTER TABLE [dbo].[catalog] ALTER COLUMN [order_in_parent_directory] [INT] NOT NULL;
END
GO
