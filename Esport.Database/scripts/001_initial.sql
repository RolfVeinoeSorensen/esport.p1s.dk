ALTER DATABASE [Esport]
    COLLATE Latin1_General_100_CS_AS_SC;
GO
/****** Object:  Schema [auth]    Script Date: 24/09/2025 22.49.22 ******/
CREATE SCHEMA [auth]
GO
/****** Object:  Schema [log]    Script Date: 24/09/2025 22.49.22 ******/
CREATE SCHEMA [log]
GO
/****** Object:  Table [auth].[Users]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Username] [nvarchar](255) NOT NULL,
	[Role] [int] NOT NULL,
	[CreatedUtc] [datetime2](7) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[PasswordResetToken] [nvarchar](max) NULL,
	[PasswordResetTokenExpiration] [datetime2](7) NOT NULL,
	[AddressStreet] [nvarchar](255) NULL,
	[AddressStreetNumber] [int] NULL,
	[AddressFloor] [nvarchar](50) NULL,
	[AddressSide] [nvarchar](50) NULL,
	[AddressPostalCode] [nvarchar](10) NULL,
	[AddressCity] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[ConsentShowImages] [bit] NOT NULL,
	[CanBringLaptop] [bit] NOT NULL,
	[CanBringStationaryPc] [bit] NOT NULL,
	[CanBringPlaystation] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventsUsers]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventsUsers](
	[EventId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Invited] [datetime] NULL,
	[Accepted] [datetime] NULL,
	[Declined] [datetime] NULL,
 CONSTRAINT [PK_EventsUsers] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Filename] [nchar](10) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Games]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Games](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Logo] [nvarchar](255) NULL,
 CONSTRAINT [PK_Games] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameServers]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameServers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Server] [nvarchar](255) NOT NULL,
	[Port] [int] NOT NULL,
	[GameId] [int] NOT NULL,
 CONSTRAINT [PK_GameServers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[MetaDescription] [nvarchar](160) NULL,
	[UrlSlug] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[ImageId] [int] NULL,
	[CategoryId] [int] NULL,
	[CreatedBy] [int] NOT NULL,
	[UpdatedBy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
	[IsPublished] [bit] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsTags]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsTags](
	[NewsId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_NewsTags] PRIMARY KEY CLUSTERED 
(
	[NewsId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersGames]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersGames](
	[GameId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[InGameName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_UsersGames] PRIMARY KEY CLUSTERED 
(
	[GameId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersTeams]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersTeams](
	[TeamId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
 CONSTRAINT [PK_UsersTeams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [log].[NLog]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [log].[NLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MachineName] [nvarchar](200) NULL,
	[Logged] [datetime] NOT NULL,
	[Level] [varchar](5) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Logger] [nvarchar](300) NULL,
	[Properties] [nvarchar](max) NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [auth].[Users] ADD  CONSTRAINT [DF_Users_ConsentShowImages]  DEFAULT ((0)) FOR [ConsentShowImages]
GO
ALTER TABLE [auth].[Users] ADD  CONSTRAINT [DF_Users_CanBringLaptop]  DEFAULT ((0)) FOR [CanBringLaptop]
GO
ALTER TABLE [auth].[Users] ADD  CONSTRAINT [DF_Users_CanBringStationaryPc]  DEFAULT ((0)) FOR [CanBringStationaryPc]
GO
ALTER TABLE [auth].[Users] ADD  CONSTRAINT [DF_Users_CanBringPlaystation]  DEFAULT ((0)) FOR [CanBringPlaystation]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO
ALTER TABLE [auth].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Parent] FOREIGN KEY([ParentId])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [auth].[Users] CHECK CONSTRAINT [FK_Users_Parent]
GO
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Users] FOREIGN KEY([CreatedBy])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Users]
GO
ALTER TABLE [dbo].[EventsUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventsUsers_Events] FOREIGN KEY([EventId])
REFERENCES [dbo].[Events] ([id])
GO
ALTER TABLE [dbo].[EventsUsers] CHECK CONSTRAINT [FK_EventsUsers_Events]
GO
ALTER TABLE [dbo].[EventsUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventsUsers_Users] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[EventsUsers] CHECK CONSTRAINT [FK_EventsUsers_Users]
GO
ALTER TABLE [dbo].[GameServers]  WITH CHECK ADD  CONSTRAINT [FK_GameServers_Games] FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[GameServers] CHECK CONSTRAINT [FK_GameServers_Games]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users] FOREIGN KEY([CreatedBy])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_Users]
GO
ALTER TABLE [dbo].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[News] CHECK CONSTRAINT [FK_News_Users1]
GO
ALTER TABLE [dbo].[NewsTags]  WITH CHECK ADD  CONSTRAINT [FK_NewsTags_News] FOREIGN KEY([NewsId])
REFERENCES [dbo].[News] ([Id])
GO
ALTER TABLE [dbo].[NewsTags] CHECK CONSTRAINT [FK_NewsTags_News]
GO
ALTER TABLE [dbo].[NewsTags]  WITH CHECK ADD  CONSTRAINT [FK_NewsTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[NewsTags] CHECK CONSTRAINT [FK_NewsTags_Tags]
GO
ALTER TABLE [dbo].[UsersGames]  WITH CHECK ADD  CONSTRAINT [FK_UsersGames_Games] FOREIGN KEY([GameId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[UsersGames] CHECK CONSTRAINT [FK_UsersGames_Games]
GO
ALTER TABLE [dbo].[UsersGames]  WITH CHECK ADD  CONSTRAINT [FK_UsersGames_Users] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[UsersGames] CHECK CONSTRAINT [FK_UsersGames_Users]
GO
ALTER TABLE [dbo].[UsersTeams]  WITH CHECK ADD  CONSTRAINT [FK_UsersTeams_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[UsersTeams] CHECK CONSTRAINT [FK_UsersTeams_Teams]
GO
ALTER TABLE [dbo].[UsersTeams]  WITH CHECK ADD  CONSTRAINT [FK_UsersTeams_Users] FOREIGN KEY([MemberId])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [dbo].[UsersTeams] CHECK CONSTRAINT [FK_UsersTeams_Users]
GO
/****** Object:  StoredProcedure [log].[NLogAddEntry]    Script Date: 24/09/2025 22.49.22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [log].[NLogAddEntry] (
  @machineName nvarchar(200),
  @logged datetime,
  @level varchar(5),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @exception nvarchar(max)
) AS
BEGIN
  INSERT INTO [log].[NLog] (
    [MachineName],
    [Logged],
    [Level],
    [Message],
    [Logger],
    [Properties],
    [Exception]
  ) VALUES (
    @machineName,
    @logged,
    @level,
    @message,
    @logger,
    @properties,
    @exception
  );
END
GO