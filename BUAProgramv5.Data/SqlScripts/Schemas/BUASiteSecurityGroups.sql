USE [BuildUserAccountsV5]
GO

/****** Object:  Table [dbo].[Site_SecurityGroups]    Script Date: 4/27/2021 2:17:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Site_SecurityGroups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[site_id] [int] NOT NULL,
	[GroupName] [varchar](250) NOT NULL,
	[DistinguishedName] [varchar](250) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Site_SecurityGroups]  WITH CHECK ADD  CONSTRAINT [FK_Site_SecurityGroups_Site] FOREIGN KEY([site_id])
REFERENCES [dbo].[Site] ([id])
GO

ALTER TABLE [dbo].[Site_SecurityGroups] CHECK CONSTRAINT [FK_Site_SecurityGroups_Site]
GO

