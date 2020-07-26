IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'order_in_parent_directory' AND TABLE_NAME='text_block')
	ALTER TABLE [dbo].[text_block] ADD [order_in_parent_directory] [INT] NULL;
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'order_in_parent_directory' AND TABLE_NAME='text_block' AND IS_NULLABLE='NO')
BEGIN
	UPDATE [dbo].[text_block] SET [order_in_parent_directory] = [id_text_block];
	ALTER TABLE [dbo].[text_block] ALTER COLUMN [order_in_parent_directory] [INT] NOT NULL;
END
GO
