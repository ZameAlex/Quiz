USE [Quiz]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 09.05.2017 17:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nchar](30) NOT NULL,
	[Type] [int] NOT NULL,
	[IDQuestion] [int] NOT NULL,
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NextQuestion]    Script Date: 09.05.2017 17:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NextQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Answers] [nchar](20) NULL,
	[IDQuestion] [int] NOT NULL,
	[IDNextRule] [int] NULL,
	[IDNextQuestion] [int] NOT NULL,
 CONSTRAINT [PK_NextQuestion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Question]    Script Date: 09.05.2017 17:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nchar](10) NOT NULL,
	[Type] [int] NOT NULL,
	[IDQuestionare] [int] NOT NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Questionare]    Script Date: 09.05.2017 17:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questionare](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[IDUser] [int] NOT NULL,
	[URL] [nchar](30) NULL,
	[Type] [bit] NULL,
 CONSTRAINT [PK_Questionare] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 09.05.2017 17:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nchar](10) NOT NULL,
	[Password] [nchar](10) NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_Question] FOREIGN KEY([IDQuestion])
REFERENCES [dbo].[Question] ([ID])
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_Question]
GO
ALTER TABLE [dbo].[NextQuestion]  WITH CHECK ADD  CONSTRAINT [FK_NextQuestion_NextQuestion] FOREIGN KEY([IDNextRule])
REFERENCES [dbo].[NextQuestion] ([ID])
GO
ALTER TABLE [dbo].[NextQuestion] CHECK CONSTRAINT [FK_NextQuestion_NextQuestion]
GO
ALTER TABLE [dbo].[NextQuestion]  WITH CHECK ADD  CONSTRAINT [FK_NextQuestion_Question] FOREIGN KEY([IDQuestion])
REFERENCES [dbo].[Question] ([ID])
GO
ALTER TABLE [dbo].[NextQuestion] CHECK CONSTRAINT [FK_NextQuestion_Question]
GO
ALTER TABLE [dbo].[NextQuestion]  WITH CHECK ADD  CONSTRAINT [FK_NextQuestion_Question1] FOREIGN KEY([IDNextQuestion])
REFERENCES [dbo].[Question] ([ID])
GO
ALTER TABLE [dbo].[NextQuestion] CHECK CONSTRAINT [FK_NextQuestion_Question1]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Questionare] FOREIGN KEY([IDQuestionare])
REFERENCES [dbo].[Questionare] ([ID])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Questionare]
GO
ALTER TABLE [dbo].[Questionare]  WITH CHECK ADD  CONSTRAINT [FK_Questionare_Users] FOREIGN KEY([IDUser])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Questionare] CHECK CONSTRAINT [FK_Questionare_Users]
GO
