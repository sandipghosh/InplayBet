USE [InplayBet]
GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/9/2015 2:14:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookMaker](
	[BookMakerId] [int] IDENTITY(1,1) NOT NULL,
	[BookMakerName] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_BookMaker_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_BookMaker_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_BookMaker_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_BookMaker] PRIMARY KEY CLUSTERED 
(
	[BookMakerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/9/2015 2:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 3/9/2015 2:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserKey] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](10) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailId] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Sex] [nvarchar](1) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[BookMakerId] [int] NOT NULL,
	[AvatarPath] [nvarchar](500) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_User_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_User_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_User_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[BookMaker] ON 

GO
INSERT [dbo].[BookMaker] ([BookMakerId], [BookMakerName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Bet 365', 1, 1, CAST(N'2015-03-08 21:17:15.887' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[BookMaker] OFF
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (1, N'Active')
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (2, N'Inative')
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (3, N'Discontinue')
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'1', N'Admin', N'Admin', N'sandipghosh.dev@hotmail.com', N'admin', N'M', CAST(N'1900-01-01 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-08 21:17:25.357' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'JVX3HZMM2Y', N'Sandip', N'Ghosh', N'sandipghosh.dev@gmail.com', N'sandev1984', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 2, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[BookMaker]  WITH CHECK ADD  CONSTRAINT [FK_BookMaker_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[BookMaker] CHECK CONSTRAINT [FK_BookMaker_Status]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_BookMaker] FOREIGN KEY([BookMakerId])
REFERENCES [dbo].[BookMaker] ([BookMakerId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_BookMaker]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Status]
GO
