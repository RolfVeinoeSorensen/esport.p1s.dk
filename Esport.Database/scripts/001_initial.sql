ALTER DATABASE [Esport]
    COLLATE Latin1_General_100_CS_AS_SC;
GO
/****** Object:  Schema [auth]    Script Date: 25/09/2025 19.15.17 ******/
CREATE SCHEMA [auth]
GO
/****** Object:  Schema [club]    Script Date: 25/09/2025 19.15.17 ******/
CREATE SCHEMA [club]
GO
/****** Object:  Schema [cms]    Script Date: 25/09/2025 19.15.17 ******/
CREATE SCHEMA [cms]
GO
/****** Object:  Schema [log]    Script Date: 25/09/2025 19.15.17 ******/
CREATE SCHEMA [log]
GO
/****** Object:  Table [auth].[AuthRoles]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[AuthRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [auth].[AuthUsers]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[AuthUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[FirstName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Username] [nvarchar](255) NOT NULL,
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
/****** Object:  Table [auth].[AuthUsersRoles]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[AuthUsersRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UsersRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [club].[Events]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[Events](
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
/****** Object:  Table [club].[EventsTeams]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[EventsTeams](
	[EventId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
 CONSTRAINT [PK_EventsTeams] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC,
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [club].[EventsUsers]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[EventsUsers](
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
/****** Object:  Table [club].[Games]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[Games](
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
/****** Object:  Table [club].[GameServers]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[GameServers](
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
/****** Object:  Table [club].[Teams]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[Teams](
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
/****** Object:  Table [club].[UsersGames]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[UsersGames](
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
/****** Object:  Table [club].[UsersTeams]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [club].[UsersTeams](
	[TeamId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
 CONSTRAINT [PK_UsersTeams] PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC,
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [cms].[Files]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cms].[Files](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Filename] [nvarchar](255) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [cms].[News]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cms].[News](
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
/****** Object:  Table [cms].[NewsTags]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cms].[NewsTags](
	[NewsId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_NewsTags] PRIMARY KEY CLUSTERED 
(
	[NewsId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [cms].[Tags]    Script Date: 25/09/2025 19.15.17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [cms].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [log].[NLog]    Script Date: 25/09/2025 19.15.17 ******/
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
ALTER TABLE [auth].[AuthUsers] ADD  CONSTRAINT [DF_Users_ConsentShowImages]  DEFAULT ((0)) FOR [ConsentShowImages]
GO
ALTER TABLE [auth].[AuthUsers] ADD  CONSTRAINT [DF_Users_CanBringLaptop]  DEFAULT ((0)) FOR [CanBringLaptop]
GO
ALTER TABLE [auth].[AuthUsers] ADD  CONSTRAINT [DF_Users_CanBringStationaryPc]  DEFAULT ((0)) FOR [CanBringStationaryPc]
GO
ALTER TABLE [auth].[AuthUsers] ADD  CONSTRAINT [DF_Users_CanBringPlaystation]  DEFAULT ((0)) FOR [CanBringPlaystation]
GO
ALTER TABLE [cms].[News] ADD  CONSTRAINT [DF_News_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO
ALTER TABLE [auth].[AuthUsers]  WITH CHECK ADD  CONSTRAINT [FK_Users_Parent] FOREIGN KEY([ParentId])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [auth].[AuthUsers] CHECK CONSTRAINT [FK_Users_Parent]
GO
ALTER TABLE [auth].[AuthUsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_UserRoles] FOREIGN KEY([RoleId])
REFERENCES [auth].[AuthRoles] ([Id])
GO
ALTER TABLE [auth].[AuthUsersRoles] CHECK CONSTRAINT [FK_UsersRoles_UserRoles]
GO
ALTER TABLE [auth].[AuthUsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Users] FOREIGN KEY([UserId])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [auth].[AuthUsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Users]
GO
ALTER TABLE [club].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Users] FOREIGN KEY([CreatedBy])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [club].[Events] CHECK CONSTRAINT [FK_Events_Users]
GO
ALTER TABLE [club].[EventsTeams]  WITH CHECK ADD  CONSTRAINT [FK_EventsTeams_Events] FOREIGN KEY([EventId])
REFERENCES [club].[Events] ([id])
GO
ALTER TABLE [club].[EventsTeams] CHECK CONSTRAINT [FK_EventsTeams_Events]
GO
ALTER TABLE [club].[EventsTeams]  WITH CHECK ADD  CONSTRAINT [FK_EventsTeams_Teams] FOREIGN KEY([TeamId])
REFERENCES [club].[Teams] ([Id])
GO
ALTER TABLE [club].[EventsTeams] CHECK CONSTRAINT [FK_EventsTeams_Teams]
GO
ALTER TABLE [club].[EventsUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventsUsers_Events] FOREIGN KEY([EventId])
REFERENCES [club].[Events] ([id])
GO
ALTER TABLE [club].[EventsUsers] CHECK CONSTRAINT [FK_EventsUsers_Events]
GO
ALTER TABLE [club].[EventsUsers]  WITH CHECK ADD  CONSTRAINT [FK_EventsUsers_Users] FOREIGN KEY([UserId])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [club].[EventsUsers] CHECK CONSTRAINT [FK_EventsUsers_Users]
GO
ALTER TABLE [club].[GameServers]  WITH CHECK ADD  CONSTRAINT [FK_GameServers_Games] FOREIGN KEY([GameId])
REFERENCES [club].[Games] ([Id])
GO
ALTER TABLE [club].[GameServers] CHECK CONSTRAINT [FK_GameServers_Games]
GO
ALTER TABLE [club].[UsersGames]  WITH CHECK ADD  CONSTRAINT [FK_UsersGames_Games] FOREIGN KEY([GameId])
REFERENCES [club].[Games] ([Id])
GO
ALTER TABLE [club].[UsersGames] CHECK CONSTRAINT [FK_UsersGames_Games]
GO
ALTER TABLE [club].[UsersGames]  WITH CHECK ADD  CONSTRAINT [FK_UsersGames_Users] FOREIGN KEY([UserId])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [club].[UsersGames] CHECK CONSTRAINT [FK_UsersGames_Users]
GO
ALTER TABLE [club].[UsersTeams]  WITH CHECK ADD  CONSTRAINT [FK_UsersTeams_Teams] FOREIGN KEY([TeamId])
REFERENCES [club].[Teams] ([Id])
GO
ALTER TABLE [club].[UsersTeams] CHECK CONSTRAINT [FK_UsersTeams_Teams]
GO
ALTER TABLE [club].[UsersTeams]  WITH CHECK ADD  CONSTRAINT [FK_UsersTeams_Users] FOREIGN KEY([MemberId])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [club].[UsersTeams] CHECK CONSTRAINT [FK_UsersTeams_Users]
GO
ALTER TABLE [cms].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Files] FOREIGN KEY([ImageId])
REFERENCES [cms].[Files] ([Id])
GO
ALTER TABLE [cms].[News] CHECK CONSTRAINT [FK_News_Files]
GO
ALTER TABLE [cms].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users] FOREIGN KEY([CreatedBy])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [cms].[News] CHECK CONSTRAINT [FK_News_Users]
GO
ALTER TABLE [cms].[News]  WITH CHECK ADD  CONSTRAINT [FK_News_Users1] FOREIGN KEY([UpdatedBy])
REFERENCES [auth].[AuthUsers] ([Id])
GO
ALTER TABLE [cms].[News] CHECK CONSTRAINT [FK_News_Users1]
GO
ALTER TABLE [cms].[NewsTags]  WITH CHECK ADD  CONSTRAINT [FK_NewsTags_News] FOREIGN KEY([NewsId])
REFERENCES [cms].[News] ([Id])
GO
ALTER TABLE [cms].[NewsTags] CHECK CONSTRAINT [FK_NewsTags_News]
GO
ALTER TABLE [cms].[NewsTags]  WITH CHECK ADD  CONSTRAINT [FK_NewsTags_Tags] FOREIGN KEY([TagId])
REFERENCES [cms].[Tags] ([Id])
GO
ALTER TABLE [cms].[NewsTags] CHECK CONSTRAINT [FK_NewsTags_Tags]
GO
/****** Object:  StoredProcedure [log].[NLogAddEntry]    Script Date: 25/09/2025 19.15.17 ******/
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
