USE [DirectoryStructure]
GO

/****** Object:  Table [dbo].[text_block]    Script Date: 3/19/2020 6:19:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[text_block](
	[id_text_block] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](max) NOT NULL,
	[text] [varchar](max) NOT NULL,
	[id_parent_catalog] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

