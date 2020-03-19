USE [DirectoryStructure]
GO

/****** Object:  Table [dbo].[catalog]    Script Date: 3/19/2020 5:56:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[catalog](
	[id_catalog] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](max) NOT NULL,
	[id_parent_catalog] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

