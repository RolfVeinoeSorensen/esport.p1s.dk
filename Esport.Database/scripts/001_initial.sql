/****** Object:  Schema [auth]    Script Date: 22/09/2025 20.24.04 ******/
CREATE SCHEMA [auth]
GO
/****** Object:  Schema [log]    Script Date: 22/09/2025 20.24.04 ******/
CREATE SCHEMA [log]
GO
/****** Object:  Table [auth].[Users]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NOT NULL,
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
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventsUsers]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventsUsers](
	[EventId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Invited] [datetime] NULL,
	[Accepted] [datetime] NULL,
	[Declined] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Games]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Games](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Games] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameServers]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameServers](
	[Id] [int] NOT NULL,
	[Server] [nvarchar](255) NOT NULL,
	[Port] [int] NOT NULL,
	[GameId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_GameServers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersGames]    Script Date: 22/09/2025 20.24.04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersGames](
	[GameId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[InGameName] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [log].[NLog]    Script Date: 22/09/2025 20.24.04 ******/
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
ALTER TABLE [auth].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Parent] FOREIGN KEY([ParentId])
REFERENCES [auth].[Users] ([Id])
GO
ALTER TABLE [auth].[Users] CHECK CONSTRAINT [FK_Users_Parent]
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
/****** Object:  StoredProcedure [log].[NLogAddEntry]    Script Date: 22/09/2025 20.24.04 ******/
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