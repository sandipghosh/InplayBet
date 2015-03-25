USE [InplayBet]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Status]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Currency]
GO
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_BookMaker]
GO
ALTER TABLE [dbo].[Team] DROP CONSTRAINT [FK_Team_Status]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [FK_Report_User]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [FK_Report_To_User]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [FK_Report_Status]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [FK_Report_Challenge]
GO
ALTER TABLE [dbo].[Legue] DROP CONSTRAINT [FK_Legue_Status]
GO
ALTER TABLE [dbo].[Currency] DROP CONSTRAINT [FK_Currency_Status]
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
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [DF_Report_CreatedOn]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [DF_Report_CreatedBy]
GO
ALTER TABLE [dbo].[Report] DROP CONSTRAINT [DF_Report_StatusId]
GO
/****** Object:  View [dbo].[UserRank]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP VIEW [dbo].[UserRank]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Team]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Status]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Report]
GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Legue]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Currency]
GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Challenge]
GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[BookMaker]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/14/2015 12:32:08 PM ******/
DROP TABLE [dbo].[Bet]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[Currency]    Script Date: 3/14/2015 12:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[CurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [nvarchar](50) NOT NULL,
	[CurrencySymbol] [nvarchar](50) NOT NULL,
	[CultureCode] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Currency_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Currency_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Currency_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[Report]    Script Date: 3/14/2015 12:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[ReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReportedBy] [int] NOT NULL,
	[ReportedUserId] [int] NOT NULL,
	[ReportedChallengeId] [int] NOT NULL,
	[Comment] [nvarchar](50) NULL,
	[ReportStatus] [nvarchar](50) NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 3/14/2015 12:32:08 PM ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 3/14/2015 12:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserKey] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](10) NOT NULL,
	[CurrencyId] [int] NOT NULL CONSTRAINT [DF_User_CurrencyId]  DEFAULT ((2)),
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
/****** Object:  View [dbo].[UserRank]    Script Date: 3/14/2015 12:32:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[UserRank]
AS
SELECT UserKey, UserId, UserName, AvatarPath, MemberSince, CurrencyId, CultureCode, 
CurrencySymbol, BookMakerId, BookMakerName, ISNULL(Wins,0) AS Wins, 
ISNULL(Losses,0) AS Losses, ISNULL(Won, 0.00) AS Won, ISNULL(Placed, 0.00) AS Placed, 
ISNULL(Profit, 0.00) AS Profit, ISNULL(RANK() OVER(ORDER BY Won DESC),0) AS [Rank],
WinningBets = CASE WHEN Profit > 0 THEN 'Profit' WHEN Profit < 0 THEN 'Loose' ELSE '' END FROM
(SELECT UserKey, UserId, UserName, AvatarPath, MemberSince, CurrencyId, CultureCode, CurrencySymbol, 
BookMakerId, BookMakerName, Wins, Losses, Won, CONVERT(decimal(11, 2),(Losses * 20)) AS Placed, 
CONVERT(decimal(11, 2),(Won - (Losses * 20))) AS Profit FROM
(SELECT U.UserKey, U.UserId, CONCAT(U.FirstName, ' ', U.LastName) AS UserName,U.AvatarPath, U.CreatedOn AS MemberSince,
C.CurrencyId, C.CultureCode, C.CurrencySymbol, B.BookMakerId, B.BookMakerName,
(SELECT COUNT(*) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'won') AS Wins,
(SELECT COUNT(*) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'lost') AS Losses,
CONVERT(decimal(11, 2),ISNULL((SELECT SUM(WiningPrice) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'won'),0.00)) AS Won
FROM [User] U INNER JOIN BookMaker B ON B.BookMakerId = U.BookMakerId
INNER JOIN Currency C ON C.CurrencyId = U.CurrencyId
WHERE U.StatusId = 1 AND B.StatusId = 1 AND C.StatusId = 1)TBL)TBL
GO
SET IDENTITY_INSERT [dbo].[Bet] ON 

GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1008, 1, 1, 2, 1, 1015, N'mndmn', N'sbsb', CAST(20.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:14:39.000' AS DateTime), 5, CAST(N'2015-03-13 04:15:52.883' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1009, 2, 1, 3, 3, 1015, N'dlfnbl', N'dlfbnl', CAST(30.00 AS Decimal(18, 2)), CAST(300.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:22:34.000' AS DateTime), 5, CAST(N'2015-03-13 04:23:22.740' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1010, 3, 4, 5, 3, 1015, N'lbflnsdfbl', N'lnbfln', CAST(300.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:23:27.000' AS DateTime), 5, CAST(N'2015-03-13 04:24:07.860' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1011, 4, 6, 1, 4, 1015, N'dlfnbl', N'ldnbl', CAST(500.00 AS Decimal(18, 2)), CAST(1050.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:24:16.000' AS DateTime), 5, CAST(N'2015-03-13 04:25:06.030' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1012, 1, 4, 6, 2, 1019, N'dkbnk', N'kdsfnbk', CAST(20.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:25:21.000' AS DateTime), 5, CAST(N'2015-03-13 04:26:12.380' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1013, 2, 7, 5, 4, 1019, N'snb', N'lnfb', CAST(100.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Lost', 1, 5, CAST(N'2015-03-13 04:26:16.000' AS DateTime), 5, CAST(N'2015-03-13 04:26:44.640' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2013, 1, 1, 2, 1, 1020, N'dfl', N'lsb', CAST(20.00 AS Decimal(18, 2)), CAST(35.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 12:58:31.000' AS DateTime), 5, CAST(N'2015-03-14 09:06:45.823' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2014, 1, 1, 2, 1, 1021, N'lnlbfldnl', N'ldnl', CAST(20.00 AS Decimal(18, 2)), CAST(30.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 7, CAST(N'2015-03-13 22:14:36.000' AS DateTime), 7, CAST(N'2015-03-13 22:17:13.723' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2015, 2, 1, 4, 3, 1021, N'sdndn', N'ldsnlnsd', CAST(30.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 7, CAST(N'2015-03-13 22:17:16.000' AS DateTime), 7, CAST(N'2015-03-13 22:19:38.600' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2016, 3, 4, 5, 10, 1021, N'fsgnf n ', N'shsdhsdh', CAST(50.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 7, CAST(N'2015-03-13 22:19:40.000' AS DateTime), 7, CAST(N'2015-03-13 22:27:34.587' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2017, 4, 4, 6, 4, 1021, N'dlbnldn', N'lndlbn', CAST(100.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 7, CAST(N'2015-03-13 22:29:06.000' AS DateTime), 7, CAST(N'2015-03-13 22:31:24.227' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2018, 5, 2, 4, 3, 1021, N'lbldnfbl', N'ldlbnldb', CAST(150.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 7, CAST(N'2015-03-13 22:35:14.000' AS DateTime), 7, CAST(N'2015-03-13 22:36:12.417' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2019, 1, 29, 30, 11, 1022, N'rlehlenhl', N'ehnlenh', CAST(20.00 AS Decimal(18, 2)), CAST(55.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:22:08.000' AS DateTime), 8, CAST(N'2015-03-14 00:35:38.127' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2020, 2, 3, 6, 3, 1022, N'dldn', N'ldnldn', CAST(55.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:35:38.000' AS DateTime), 8, CAST(N'2015-03-14 00:36:49.890' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2021, 3, 3, 5, 4, 1022, N'sdknlk', N'lkndldn', CAST(100.00 AS Decimal(18, 2)), CAST(1030.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:36:50.000' AS DateTime), 8, CAST(N'2015-03-14 00:38:01.233' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2022, 1, 3, 9, 10, 1023, N'ldnlnb', N'dlldnf', CAST(20.00 AS Decimal(18, 2)), CAST(600.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Lost', 1, 8, CAST(N'2015-03-14 00:38:01.000' AS DateTime), 8, CAST(N'2015-03-14 00:39:11.333' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2023, 1, 1, 5, 5, 1024, N'gfggg', N'fdfsfs', CAST(20.00 AS Decimal(18, 2)), CAST(56.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:41:23.000' AS DateTime), 8, CAST(N'2015-03-14 00:42:30.490' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2024, 2, 4, 8, 3, 1024, N'ghfgh', N'njghn', CAST(56.00 AS Decimal(18, 2)), CAST(85.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:42:30.000' AS DateTime), 8, CAST(N'2015-03-14 00:43:22.123' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2025, 3, 6, 10, 4, 1024, N'fhfgfg', N'gfgdfg', CAST(85.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Lost', 1, 8, CAST(N'2015-03-14 00:43:22.000' AS DateTime), 8, CAST(N'2015-03-14 00:44:18.560' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2026, 2, 2, 4, 5, 1020, N'kskjsb', N'ksv', CAST(35.00 AS Decimal(18, 2)), CAST(50.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-14 09:06:46.000' AS DateTime), 5, CAST(N'2015-03-14 09:07:20.500' AS DateTime))
GO
INSERT [dbo].[Bet] ([BetId], [BetNumber], [TeamAId], [TeamBId], [LegueId], [ChallengeId], [BetType], [Odds], [BetPlaced], [WiningTotal], [LoosingTotal], [BetStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2027, 1, 1, 2, 2, 1025, N'lsdfbl', N'slbnlsb', CAST(20.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Won', 1, 9, CAST(N'2015-03-14 12:30:42.000' AS DateTime), 9, CAST(N'2015-03-14 12:31:11.063' AS DateTime))
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
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1015, 1, 5, CAST(1050.00 AS Decimal(18, 2)), N'Won', 1, 5, CAST(N'2015-03-13 04:14:39.000' AS DateTime), 5, CAST(N'2015-03-13 04:25:06.057' AS DateTime))
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1019, 2, 5, CAST(-20.00 AS Decimal(18, 2)), N'Lost', 1, 5, CAST(N'2015-03-13 04:25:21.000' AS DateTime), 5, CAST(N'2015-03-13 04:26:44.673' AS DateTime))
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1020, 3, 5, CAST(0.00 AS Decimal(18, 2)), N'', 1, 5, CAST(N'2015-03-13 12:58:31.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1021, 1, 7, CAST(0.00 AS Decimal(18, 2)), N'', 1, 7, CAST(N'2015-03-13 22:14:36.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1022, 1, 8, CAST(1030.00 AS Decimal(18, 2)), N'Won', 1, 8, CAST(N'2015-03-14 00:22:08.000' AS DateTime), 8, CAST(N'2015-03-14 00:38:01.260' AS DateTime))
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1023, 2, 8, CAST(-20.00 AS Decimal(18, 2)), N'Lost', 1, 8, CAST(N'2015-03-14 00:38:01.000' AS DateTime), 8, CAST(N'2015-03-14 00:39:11.360' AS DateTime))
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1024, 3, 8, CAST(-20.00 AS Decimal(18, 2)), N'Lost', 1, 8, CAST(N'2015-03-14 00:41:23.000' AS DateTime), 8, CAST(N'2015-03-14 00:44:18.590' AS DateTime))
GO
INSERT [dbo].[Challenge] ([ChallengeId], [ChallengeNumber], [UserKey], [WiningPrice], [ChallengeStatus], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1025, 1, 9, CAST(0.00 AS Decimal(18, 2)), N'', 1, 9, CAST(N'2015-03-14 12:30:42.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Challenge] OFF
GO
SET IDENTITY_INSERT [dbo].[Currency] ON 

GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyName], [CurrencySymbol], [CultureCode], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'US Dollar', N'$', N'en-US', 1, 1, CAST(N'2015-03-13 15:34:04.563' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyName], [CurrencySymbol], [CultureCode], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2, N'Pound Sterling', N'£', N'en-GB', 1, 1, CAST(N'2015-03-13 15:35:37.557' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyName], [CurrencySymbol], [CultureCode], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Euro', N'€', N'it-IT', 1, 1, CAST(N'2015-03-13 15:37:21.167' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Currency] OFF
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
INSERT [dbo].[Legue] ([LegueId], [LegueName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (11, N'World Cup', 1, 8, CAST(N'2015-03-14 00:34:45.327' AS DateTime), NULL, NULL)
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
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (28, N'www', 1, 5, CAST(N'2015-03-13 02:21:23.923' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (29, N'India', 1, 8, CAST(N'2015-03-14 00:33:45.370' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Team] ([TeamId], [TeamName], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (30, N'England', 1, 8, CAST(N'2015-03-14 00:34:13.763' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Team] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'Admin', 1, N'Admin', N'Admin', N'sandipghosh.dev@hotmail.com', N'admin', N'M', CAST(N'1900-01-01 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-08 21:17:25.357' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'JVX3HZMM2Y', 2, N'Sandip', N'Ghosh', N'sandipghosh.dev@gmail.com', N'sandev1984', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (7, N'User1', 3, N'User1', N'User1', N'User1@gmail.com', N'User1', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (8, N'User2', 1, N'User2', N'User2', N'User2@gmail.com', N'User2', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (9, N'User3', 2, N'User3', N'User3', N'User3@gmail.com', N'User3', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (10, N'User4', 3, N'User4', N'User4', N'User4@gmail.com', N'User4', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (11, N'User5', 1, N'User5', N'User5', N'User5@gmail.com', N'User5', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[User] ([UserKey], [UserId], [CurrencyId], [FirstName], [LastName], [EmailId], [Password], [Sex], [DateOfBirth], [BookMakerId], [AvatarPath], [StatusId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (12, N'User6', 2, N'User6', N'User6', N'User6@gmail.com', N'User6', N'M', CAST(N'1984-12-22 00:00:00.000' AS DateTime), 1, N'~/Images/Users/Default.jpg', 1, 1, CAST(N'2015-03-09 02:04:44.000' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
ALTER TABLE [dbo].[Report] ADD  CONSTRAINT [DF_Report_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Report] ADD  CONSTRAINT [DF_Report_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Report] ADD  CONSTRAINT [DF_Report_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
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
ALTER TABLE [dbo].[Currency]  WITH CHECK ADD  CONSTRAINT [FK_Currency_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Currency] CHECK CONSTRAINT [FK_Currency_Status]
GO
ALTER TABLE [dbo].[Legue]  WITH CHECK ADD  CONSTRAINT [FK_Legue_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Legue] CHECK CONSTRAINT [FK_Legue_Status]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_Challenge] FOREIGN KEY([ReportedChallengeId])
REFERENCES [dbo].[Challenge] ([ChallengeId])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_Challenge]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_Status]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_To_User] FOREIGN KEY([ReportedUserId])
REFERENCES [dbo].[User] ([UserKey])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_To_User]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_User] FOREIGN KEY([ReportedBy])
REFERENCES [dbo].[User] ([UserKey])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_User]
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
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Currency] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([CurrencyId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Currency]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Status]
GO
