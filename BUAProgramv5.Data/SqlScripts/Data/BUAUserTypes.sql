USE [BuildUserAccountsV5]
GO
SET IDENTITY_INSERT [dbo].[UserTypes] ON 

GO
INSERT [dbo].[UserTypes] ([id], [TypeName]) VALUES (7, N'Doctor')
GO
INSERT [dbo].[UserTypes] ([id], [TypeName]) VALUES (8, N'MidLevel')
GO
INSERT [dbo].[UserTypes] ([id], [TypeName]) VALUES (9, N'NonProvider')
GO
INSERT [dbo].[UserTypes] ([id], [TypeName]) VALUES (10, N'SiteManager')
GO
SET IDENTITY_INSERT [dbo].[UserTypes] OFF
GO