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
/****** Object:  Table [dbo].[User]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[Team]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[Status]
GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[Legue]
GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[Challenge]
GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[BookMaker]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/12/2015 7:56:59 AM ******/
DROP TABLE [dbo].[Bet]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/12/2015 7:56:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bet](
	[BetId] [int] IDENTITY(1,1) NOT NULL,
	[BetNumber] [int] NOT NULL,
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
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Bet_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Bet_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Bet_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Bet] PRIMARY KEY CLUSTERED 
(
	[BetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/12/2015 7:56:59 AM ******/
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
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/12/2015 7:56:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Challenge](
	[ChallengeId] [int] IDENTITY(1,1) NOT NULL,
	[ChallengeNumber] [int] NOT NULL,
	[UserKey] [int] NOT NULL,
	[WiningPrice] [decimal](18, 2) NOT NULL CONSTRAINT [DF_Challenge_WiningPrice]  DEFAULT ((0)),
	[ChallengeStatus] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Challenge_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Challenge_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Challenge_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Challenge] PRIMARY KEY CLUSTERED 
(
	[ChallengeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/12/2015 7:56:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Legue](
	[LegueId] [int] IDENTITY(1,1) NOT NULL,
	[LegueName] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Legue_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Legue_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Legue_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Legue] PRIMARY KEY CLUSTERED 
(
	[LegueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/12/2015 7:56:59 AM ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 3/12/2015 7:56:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](100) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Team_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Team_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Team_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 3/12/2015 7:56:59 AM ******/
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
SET IDENTITY_INSERT [dbo].[Bet] ON 

GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2, 1, 1, 2, 1, 1, N'test1', N'1', CAST(20.00 AS Decimal(18, 2)), CAST(25.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 1, CAST(N'2015-03-12 02:05:05.500' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (3, 2, 3, 4, 1, 1, N'test1', N'1', CAST(25.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 1, CAST(N'2015-03-12 02:05:05.500' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, 3, 5, 6, 1, 1, N'test1', N'1', CAST(30.00 AS Decimal(18, 2)), CAST(35.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Lost', 1, 1, CAST(N'2015-03-12 02:05:05.500' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, 1, 7, 8, 1, 2, N'test1', N'1', CAST(20.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 1, CAST(N'2015-03-12 02:05:05.500' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (6, 2, 9, 10, 1, 2, N'test1', N'1', CAST(500.00 AS Decimal(18, 2)), CAST(1050.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 1, CAST(N'2015-03-12 02:05:05.500' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Bet] OFF
GO
SET IDENTITY_INSERT [dbo].[BookMaker] ON 

GO
INSERT [dbo].[BookMaker] ([BookMakerId], [BookMakerName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Bet 365', 1, 1, CAST(N'2015-03-08 21:17:15.887' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[BookMaker] OFF
GO
SET IDENTITY_INSERT [dbo].[Challenge] ON 

GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, 1, 5, CAST(-20.00 AS Decimal(18, 2)), N'Lost', 1, 1, CAST(N'2015-03-12 02:03:50.153' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2, 2, 5, CAST(500.00 AS Decimal(18, 2)), N'Won', 1, 1, CAST(N'2015-03-12 02:03:50.153' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Challenge] OFF
GO
SET IDENTITY_INSERT [dbo].[Legue] ON 

GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Legue 1', 1, 1, CAST(N'2015-03-11 22:54:45.960' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2, N'Legue 2', 1, 1, CAST(N'2015-03-11 22:54:47.910' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Legue 3', 1, 1, CAST(N'2015-03-11 22:54:49.603' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'Legue 4', 1, 1, CAST(N'2015-03-11 22:54:51.420' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'Legue 5', 1, 1, CAST(N'2015-03-11 22:54:53.203' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (6, N'Legue 6', 1, 1, CAST(N'2015-03-11 22:54:54.997' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (7, N'Legue 7', 1, 1, CAST(N'2015-03-11 22:54:56.640' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (8, N'Legue 8', 1, 1, CAST(N'2015-03-11 22:54:58.580' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (9, N'Legue 9', 1, 1, CAST(N'2015-03-11 22:55:00.320' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (10, N'Legue 10', 1, 1, CAST(N'2015-03-11 22:55:03.160' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Legue] OFF
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (1, N'Active')
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (2, N'Inative')
GO
INSERT [dbo].[Status] ([StatusId], [StatusName]) VALUES (3, N'Discontinue')
GO
SET IDENTITY_INSERT [dbo].[Team] ON 

GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Team A', 1, 1, CAST(N'2015-03-11 22:52:54.510' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2, N'Team B', 1, 1, CAST(N'2015-03-11 22:53:01.580' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Team C', 1, 1, CAST(N'2015-03-11 22:53:05.370' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'Team D', 1, 1, CAST(N'2015-03-11 22:53:11.680' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'Team E', 1, 1, CAST(N'2015-03-11 22:53:14.323' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (6, N'Team F', 1, 1, CAST(N'2015-03-11 22:53:17.800' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (7, N'Team G', 1, 1, CAST(N'2015-03-11 22:53:20.763' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (8, N'Team H', 1, 1, CAST(N'2015-03-11 22:53:25.243' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (9, N'Team I', 1, 1, CAST(N'2015-03-11 22:53:27.427' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (10, N'Team J', 1, 1, CAST(N'2015-03-11 22:53:29.487' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (11, N'Team K', 1, 1, CAST(N'2015-03-11 22:53:31.373' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (12, N'Team L', 1, 1, CAST(N'2015-03-11 22:53:33.507' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (13, N'Team M', 1, 1, CAST(N'2015-03-11 22:53:36.163' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (14, N'Team N', 1, 1, CAST(N'2015-03-11 22:53:38.010' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (15, N'Team P', 1, 1, CAST(N'2015-03-11 22:53:40.117' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (16, N'Team Q', 1, 1, CAST(N'2015-03-11 22:53:42.023' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (17, N'Team R', 1, 1, CAST(N'2015-03-11 22:54:04.670' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (18, N'Team S', 1, 1, CAST(N'2015-03-11 22:54:06.587' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (19, N'Team T', 1, 1, CAST(N'2015-03-11 22:54:08.957' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (20, N'Team U', 1, 1, CAST(N'2015-03-11 22:54:10.960' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (21, N'Team V', 1, 1, CAST(N'2015-03-11 22:54:14.760' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (22, N'Team W', 1, 1, CAST(N'2015-03-11 22:54:17.010' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (23, N'Team X', 1, 1, CAST(N'2015-03-11 22:54:18.970' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (24, N'Team Y', 1, 1, CAST(N'2015-03-11 22:54:20.907' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (25, N'Team Z', 1, 1, CAST(N'2015-03-11 22:54:22.853' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Team] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'1', N'Admin', N'Admin', N'sandipghosh.dev@hotmail.com', N'admin', N'M', CAST(N'1900-01-01 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-08 21:17:25.357' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'JVX3HZMM2Y', N'Sandip', N'Ghosh', N'sandipghosh.dev@gmail.com', N'sandev1984', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
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
