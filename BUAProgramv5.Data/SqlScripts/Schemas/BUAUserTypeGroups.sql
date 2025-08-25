USE [BuildUserAccountsV5]
GO

/****** Object:  Table [dbo].[UserTypeGroups]    Script Date: 4/27/2021 2:20:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserTypeGroups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Type_ID] [int] NOT NULL,
	[Group_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserTypeGroups]  WITH CHECK ADD  CONSTRAINT [FK_UserTypeGroups_SecurityGroups] FOREIGN KEY([Group_ID])
REFERENCES [dbo].[SecurityGroups] ([id])
GO

ALTER TABLE [dbo].[UserTypeGroups] CHECK CONSTRAINT [FK_UserTypeGroups_SecurityGroups]
GO

ALTER TABLE [dbo].[UserTypeGroups]  WITH CHECK ADD  CONSTRAINT [FK_UserTypeGroups_UserTypes] FOREIGN KEY([Type_ID])
REFERENCES [dbo].[UserTypes] ([id])
GO

ALTER TABLE [dbo].[UserTypeGroups] CHECK CONSTRAINT [FK_UserTypeGroups_UserTypes]
GO