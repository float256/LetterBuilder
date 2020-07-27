IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'catalog' AND CONSTRAINT_TYPE='PRIMARY KEY')
	ALTER TABLE catalog ADD CONSTRAINT [PK_catalog] PRIMARY KEY CLUSTERED
	(
		[id_catalog] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO