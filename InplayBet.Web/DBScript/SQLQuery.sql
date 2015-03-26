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
ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [DF_ContactUs_CreatedOn]
GO
ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [DF_ContactUs_CreatedBy]
GO
ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [DF_ContactUs_StatusId]
GO
/****** Object:  View [dbo].[UserRank]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP VIEW [dbo].[UserRank]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[Teams$]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Teams$]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Team]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Status]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Report]
GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Legue]
GO
/****** Object:  Table [dbo].[Follow]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Follow]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Currency]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Contact]
GO
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Challenge]
GO
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[BookMaker]
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP TABLE [dbo].[Bet]
GO
/****** Object:  StoredProcedure [dbo].[ResetUserAccount]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP PROCEDURE [dbo].[ResetUserAccount]
GO
/****** Object:  StoredProcedure [dbo].[GetCheatCountByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP PROCEDURE [dbo].[GetCheatCountByUser]
GO
/****** Object:  StoredProcedure [dbo].[CosecutiveBetByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP PROCEDURE [dbo].[CosecutiveBetByUser]
GO
/****** Object:  StoredProcedure [dbo].[ConsecutiveBetByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
DROP PROCEDURE [dbo].[ConsecutiveBetByUser]
GO
/****** Object:  StoredProcedure [dbo].[ConsecutiveBetByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ConsecutiveBetByUser]
(
	@userid int, 
	@status VARCHAR(20) = 'Won'
)
AS 
BEGIN
	DECLARE @irow int=1
	DECLARE @trow int
	DECLARE @cnt int=0
	DECLARE @mcnt int=0
	DECLARE @cstatus VARCHAR(10)

	SELECT C.UserKey, B.BetStatus, ROW_NUMBER() OVER(ORDER BY C.UserKey, B.CreatedOn DESC) AS rn
	INTO #tmptbl FROM Bet B INNER JOIN Challenge C ON
	B.ChallengeId = C.ChallengeId AND C.StatusId = 1 AND B.StatusId = 1
	WHERE ISNULL(B.BetStatus, '') <> '' AND C.UserKey = @userid

	SELECT @trow = COUNT(1) FROM #tmptbl

	WHILE @irow <= @trow
	BEGIN
		SELECT @cstatus = BetStatus FROM #tmptbl WHERE rn = @irow
		IF @cstatus=@status
		BEGIN
			SET @cnt += 1
			IF @mcnt < @cnt
				SET @mcnt = @cnt
		END
		ELSE
		BEGIN
			SET @cnt = 0
		END
 
		SET @irow += 1
	END

	SELECT @mcnt AS ConsicutiveCount
	DROP TABLE #tmptbl
END
GO
/****** Object:  StoredProcedure [dbo].[CosecutiveBetByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[CosecutiveBetByUser]
(
	@userid int, 
	@status VARCHAR(20) = 'Won'
)
AS 
BEGIN
	DECLARE @irow int=1
	DECLARE @trow int
	DECLARE @cnt int=0
	DECLARE @mcnt int=0
	DECLARE @cstatus VARCHAR(10)

	SELECT C.UserKey, B.BetStatus, ROW_NUMBER() OVER(ORDER BY C.UserKey, B.CreatedOn DESC) AS rn
	INTO #tmptbl FROM Bet B INNER JOIN Challenge C ON
	B.ChallengeId = C.ChallengeId AND C.StatusId = 1 AND B.StatusId = 1
	WHERE ISNULL(B.BetStatus, '') <> '' AND C.UserKey = @userid

	SELECT @trow = COUNT(1) FROM #tmptbl

	WHILE @irow <= @trow
	BEGIN
		SELECT @cstatus = BetStatus FROM #tmptbl WHERE rn = @irow
		IF @cstatus=@status
		BEGIN
			SET @cnt += 1
			IF @mcnt < @cnt
				SET @mcnt = @cnt
		END
		ELSE
		BEGIN
			SET @cnt = 0
		END
 
		SET @irow += 1
	END

	SELECT @mcnt AS ConsicutiveCount
	DROP TABLE #tmptbl
END
GO
/****** Object:  StoredProcedure [dbo].[GetCheatCountByUser]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetCheatCountByUser]
(@ReportedUserId INT)
AS
BEGIN
	SELECT U.UserId, U.UserName, COUNT(1) AS ReportCount
	FROM [Report] R INNER JOIN [UserRank] U ON R.ReportedBy = U.UserKey
	WHERE R.StatusId = 1 AND R.ReportedUserId = @ReportedUserId
	GROUP BY U.UserId, U.UserName 
	ORDER BY U.UserId, U.UserName
END

GO
/****** Object:  StoredProcedure [dbo].[ResetUserAccount]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[ResetUserAccount]
(
	@userid INT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @trancount int;
	SET @trancount = @@trancount;
	
	BEGIN TRY
		IF @trancount = 0
			BEGIN TRANSACTION
		ELSE
			SAVE TRANSACTION ResetUserAccount;	
		
	UPDATE B SET StatusId = 2 FROM Bet B INNER JOIN Challenge C 
	ON B.ChallengeId = C.ChallengeId
	WHERE C.UserKey = @userid

	UPDATE Challenge SET StatusId = 2 WHERE UserKey = @userid
	UPDATE [User] SET CreatedOn = GETDATE(), UpdatedBy=NULL,UpdatedOn=NULL WHERE UserKey = @userid
	
	LBEXIT:
		IF @trancount = 0	
			COMMIT;
			
	END TRY
	BEGIN CATCH
		DECLARE @error INT, @message VARCHAR(4000), @xstate INT;
		SELECT @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
		IF @xstate = -1
			ROLLBACK;
		IF @xstate = 1 and @trancount = 0
			ROLLBACK
		IF @xstate = 1 and @trancount > 0
			ROLLBACK TRANSACTION ResetUserAccount;

		RAISERROR ('ResetUserAccount: %d: %s', 16, 1, @error, @message) ;
	END CATCH
	
	SET NOCOUNT OFF

END
GO
/****** Object:  Table [dbo].[Bet]    Script Date: 3/27/2015 2:44:32 AM ******/
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
/****** Object:  Table [dbo].[BookMaker]    Script Date: 3/27/2015 2:44:32 AM ******/
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
/****** Object:  Table [dbo].[Challenge]    Script Date: 3/27/2015 2:44:32 AM ******/
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
/****** Object:  Table [dbo].[Contact]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[CotactUsId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[EmailId] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Massage] [nvarchar](max) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[CotactUsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Currency]    Script Date: 3/27/2015 2:44:32 AM ******/
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
/****** Object:  Table [dbo].[Follow]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Follow](
	[FollowId] [int] IDENTITY(1,1) NOT NULL,
	[FollowBy] [int] NOT NULL,
	[FollowTo] [int] NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Follow_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Follow_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Follow_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Follow] PRIMARY KEY CLUSTERED 
(
	[FollowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Legue]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Legue](
	[LegueId] [int] IDENTITY(1,1) NOT NULL,
	[LegueName] [nvarchar](200) NOT NULL,
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
/****** Object:  Table [dbo].[Report]    Script Date: 3/27/2015 2:44:32 AM ******/
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
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Report_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_Report_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Report_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 3/27/2015 2:44:32 AM ******/
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
/****** Object:  Table [dbo].[Team]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](300) NOT NULL,
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
/****** Object:  Table [dbo].[Teams$]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams$](
	[F1] [nvarchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserKey] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[CurrencyId] [int] NOT NULL CONSTRAINT [DF_User_CurrencyId]  DEFAULT ((2)),
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailId] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Sex] [nvarchar](1) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[BookMakerId] [int] NOT NULL,
	[AvatarPath] [nvarchar](500) NOT NULL,
	[IsAdmin] [bit] NOT NULL CONSTRAINT [DF_User_IsAdmin]  DEFAULT ((0)),
	[StatusId] [int] NOT NULL CONSTRAINT [DF_User_StatusId]  DEFAULT ((1)),
	[CreatedBy] [int] NOT NULL CONSTRAINT [DF_User_CreatedBy]  DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_User_CreatedOn]  DEFAULT (getdate()),
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[Address] [nvarchar](500) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[UserRank]    Script Date: 3/27/2015 2:44:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[UserRank]
AS
SELECT UserKey, UserId, UserName, [Address], AvatarPath, MemberSince, Sex, CurrencyId, CultureCode, 
CurrencySymbol, BookMakerId, BookMakerName, ISNULL(Followers, 0) AS Followers, 
ISNULL(BetWins,0) AS BetWins, ISNULL(TotalBets,0) AS TotalBets, ISNULL(TotalChallenges,0) AS TotalChallenges, 
ISNULL(Wins,0) AS Wins, ISNULL(Losses,0) AS Losses, ISNULL(Won, 0.00) AS Won, ISNULL(Placed, 0.00) AS Placed, 
ISNULL(Profit, 0.00) AS Profit, ISNULL(RANK() OVER(ORDER BY Won DESC),0) AS [Rank],
WinningBets = CASE WHEN Profit > 0 THEN 'Profit' WHEN Profit < 0 THEN 'Loose' ELSE '' END, ISNULL(TotalCheatReported,0) AS TotalCheatReported FROM
(SELECT UserKey, UserId, UserName, ISNULL([Address],'') AS [Address], AvatarPath, MemberSince, Sex, CurrencyId, CultureCode, CurrencySymbol, Followers,
BookMakerId, BookMakerName, BetWins, TotalBets,TotalChallenges, Wins, Losses, Won, CONVERT(decimal(11, 2),(Losses * 20)) AS Placed, 
CONVERT(decimal(11, 2),(Won - (Losses * 20))) AS Profit, TotalCheatReported FROM
(SELECT U.UserKey, U.UserId, (U.FirstName + ' ' + U.LastName) AS UserName, U.[Address], U.AvatarPath, U.CreatedOn AS MemberSince, U.Sex,
C.CurrencyId, C.CultureCode, C.CurrencySymbol, B.BookMakerId, B.BookMakerName,
(SELECT COUNT(1) FROM Follow F WHERE F.FollowTo = U.UserKey AND F.StatusId =1) AS Followers,
(SELECT COUNT(1) FROM Bet WHERE Bet.ChallengeId IN (SELECT ChallengeId FROM Challenge C1 WHERE C1.UserKey = U.UserKey AND C1.StatusId = 1) AND Bet.StatusId = 1 AND LOWER(Bet.BetStatus) = 'won') AS BetWins,
(SELECT COUNT(1) FROM Bet WHERE Bet.ChallengeId IN (SELECT ChallengeId FROM Challenge C1 WHERE C1.UserKey = U.UserKey AND C1.StatusId = 1) AND Bet.StatusId = 1) AS TotalBets,
(SELECT COUNT(1) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1) AS TotalChallenges,
(SELECT COUNT(1) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'won') AS Wins,
(SELECT COUNT(1) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'lost') AS Losses,
CONVERT(decimal(11, 2),ISNULL((SELECT SUM(WiningPrice) FROM Challenge C WHERE C.UserKey = U.UserKey AND C.StatusId = 1 AND LOWER(C.ChallengeStatus) = 'won'),0.00)) AS Won,
(SELECT COUNT(1) FROM Report R WHERE R.ReportedUserId = U.UserKey AND C.StatusId = 1) AS TotalCheatReported
FROM [User] U INNER JOIN BookMaker B ON B.BookMakerId = U.BookMakerId
INNER JOIN Currency C ON C.CurrencyId = U.CurrencyId
WHERE U.StatusId = 1 AND B.StatusId = 1 AND C.StatusId = 1)TBL)TBL

GO
ALTER TABLE [dbo].[Contact] ADD  CONSTRAINT [DF_ContactUs_StatusId]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Contact] ADD  CONSTRAINT [DF_ContactUs_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Contact] ADD  CONSTRAINT [DF_ContactUs_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
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
