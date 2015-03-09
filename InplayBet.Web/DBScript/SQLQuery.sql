USE [InplayBet]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Status]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_BookMaker]
GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_Status]
GO
ALTER TABLE [dbo].[Legue] DROP CONSTRAINT [FK_Legue_Status]
GO
ALTER TABLE [dbo].[Challenge] DROP CONSTRAINT [FK_Challenge_User]
GO
ALTER TABLE [dbo].[Challenge] DROP CONSTRAINT [FK_Challenge_Status]
GO
ALTER TABLE [dbo].[BookMaker] DROP CONSTRAINT [FK_BookMaker_Status]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [FK_Bet_TeamB]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [FK_Bet_TeamA]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [FK_Bet_Status]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [FK_Bet_Legue]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [FK_Bet_Challenge]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_CreatedOn]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_CreatedBy]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [DF_User_StatusId]
GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [DF_Team_CreatedOn]
GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [DF_Team_CreatedBy]
GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [DF_Team_StatusId]
GO
ALTER TABLE [dbo].[Legue] DROP CONSTRAINT [DF_Legue_CreatedOn]
GO
ALTER TABLE [dbo].[Legue] DROP CONSTRAINT [DF_Legue_CreatedBy]
GO
ALTER TABLE [dbo].[Legue] DROP CONSTRAINT [DF_Legue_StatusId]
GO
ALTER TABLE [dbo].[Challenge] DROP CONSTRAINT [DF_Challenge_CreatedOn]
GO
ALTER TABLE [dbo].[Challenge] DROP CONSTRAINT [DF_Challenge_CreatedBy]
GO
ALTER TABLE [dbo].[Challenge] DROP CONSTRAINT [DF_Challenge_StatusId]
GO
ALTER TABLE [dbo].[BookMaker] DROP CONSTRAINT [DF_BookMaker_CreatedOn]
GO
ALTER TABLE [dbo].[BookMaker] DROP CONSTRAINT [DF_BookMaker_CreatedBy]
GO
ALTER TABLE [dbo].[BookMaker] DROP CONSTRAINT [DF_BookMaker_StatusId]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [DF_Bet_CreatedOn]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [DF_Bet_CreatedBy]
GO
ALTER TABLE [dbo].[Bet] DROP CONSTRAINT [DF_Bet_StatusId]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[Team]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[Status]
GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[Legue]
GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[Challenge]
GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[BookMaker]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/9/2015 6:42:04 PM ******/
DROP TABLE [dbo].[Bet]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/9/2015 6:42:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bet](
	[BetId] [int] IDENTITY(1,1) NOT NULL,
	[TeamAId] [int] NOT NULL,
	[TeamBId] [int] NOT NULL,
	[LegueId] [int] NOT NULL,
	[ChallengeId] [int] NOT NULL,
	[BetType] [nvarchar](200) NOT NULL,
	[Odds] [nvarchar](50) NOT NULL,
	[BetPlaced] [decimal](18, 2) NOT NULL,
	[WiningTotal] [decimal](18, 2) NOT NULL,
	[LoosingTotal] [decimal](18, 2) NOT NULL,
	[BetStatus] [nvarchar](50) NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Bet] PRIMARY KEY CLUSTERED 
(
	[BetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/9/2015 6:42:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookMaker](
	[BookMakerId] [int] IDENTITY(1,1) NOT NULL,
	[BookMakerName] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_BookMaker] PRIMARY KEY CLUSTERED 
(
	[BookMakerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/9/2015 6:42:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Challenge](
	[ChallengeId] [int] IDENTITY(1,1) NOT NULL,
	[ChallengeNumber] [int] NOT NULL,
	[UserKey] [int] NOT NULL,
	[ChallengeStatus] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Challenge] PRIMARY KEY CLUSTERED 
(
	[ChallengeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/9/2015 6:42:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Legue](
	[LegueId] [int] IDENTITY(1,1) NOT NULL,
	[LegueName] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Legue] PRIMARY KEY CLUSTERED 
(
	[LegueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/9/2015 6:42:04 PM ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 3/9/2015 6:42:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](100) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 3/9/2015 6:42:04 PM ******/
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
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
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
INSERT [dbo].[BookMaker] ([BookMakerId], [BookMakerName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Bet 365', 1, 1, CAST(0x0000A455015ECFAE AS DateTime), NULL, NULL)
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
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'1', N'Admin', N'Admin', N'sandipghosh.dev@hotmail.com', N'admin', N'M', CAST(0x0000000000000000 AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(0x0000A455015EDAC7 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'JVX3HZMM2Y', N'Sandip', N'Ghosh', N'sandipghosh.dev@gmail.com', N'sandev1984', N'M', CAST(0x0000793C00000000 AS DateTime), 1, N'~/Images/Users/Default.jpg', 2, 1, CAST(0x0000A45600224250 AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[Bet] ADD  CONSTRAINT [DF_Bet_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Bet] ADD  CONSTRAINT [DF_Bet_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Bet] ADD  CONSTRAINT [DF_Bet_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[BookMaker] ADD  CONSTRAINT [DF_BookMaker_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[BookMaker] ADD  CONSTRAINT [DF_BookMaker_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[BookMaker] ADD  CONSTRAINT [DF_BookMaker_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Challenge] ADD  CONSTRAINT [DF_Challenge_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Challenge] ADD  CONSTRAINT [DF_Challenge_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Challenge] ADD  CONSTRAINT [DF_Challenge_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Legue] ADD  CONSTRAINT [DF_Legue_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Legue] ADD  CONSTRAINT [DF_Legue_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Legue] ADD  CONSTRAINT [DF_Legue_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Team] ADD  CONSTRAINT [DF_Team_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Team] ADD  CONSTRAINT [DF_Team_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Team] ADD  CONSTRAINT [DF_Team_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Bet]  WITH CHECK ADD  CONSTRAINT [FK_Bet_Challenge] FOREIGN KEY([ChallengeId])
REFERENCES [dbo].[Challenge] ([ChallengeId])
GO
ALTER TABLE [dbo].[Bet] CHECK CONSTRAINT [FK_Bet_Challenge]
GO
ALTER TABLE [dbo].[Bet]  WITH CHECK ADD  CONSTRAINT [FK_Bet_Legue] FOREIGN KEY([LegueId])
REFERENCES [dbo].[Legue] ([LegueId])
GO
ALTER TABLE [dbo].[Bet] CHECK CONSTRAINT [FK_Bet_Legue]
GO
ALTER TABLE [dbo].[Bet]  WITH CHECK ADD  CONSTRAINT [FK_Bet_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Bet] CHECK CONSTRAINT [FK_Bet_Status]
GO
ALTER TABLE [dbo].[Bet]  WITH CHECK ADD  CONSTRAINT [FK_Bet_TeamA] FOREIGN KEY([TeamAId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Bet] CHECK CONSTRAINT [FK_Bet_TeamA]
GO
ALTER TABLE [dbo].[Bet]  WITH CHECK ADD  CONSTRAINT [FK_Bet_TeamB] FOREIGN KEY([TeamBId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[Bet] CHECK CONSTRAINT [FK_Bet_TeamB]
GO
ALTER TABLE [dbo].[BookMaker]  WITH CHECK ADD  CONSTRAINT [FK_BookMaker_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[BookMaker] CHECK CONSTRAINT [FK_BookMaker_Status]
GO
ALTER TABLE [dbo].[Challenge]  WITH CHECK ADD  CONSTRAINT [FK_Challenge_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Challenge] CHECK CONSTRAINT [FK_Challenge_Status]
GO
ALTER TABLE [dbo].[Challenge]  WITH CHECK ADD  CONSTRAINT [FK_Challenge_User] FOREIGN KEY([UserKey])
REFERENCES [dbo].[User] ([UserKey])
GO
ALTER TABLE [dbo].[Challenge] CHECK CONSTRAINT [FK_Challenge_User]
GO
ALTER TABLE [dbo].[Legue]  WITH CHECK ADD  CONSTRAINT [FK_Legue_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Legue] CHECK CONSTRAINT [FK_Legue_Status]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FK_Team_Status]
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
