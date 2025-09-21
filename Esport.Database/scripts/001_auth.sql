CREATE SCHEMA [auth]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Role] [int] NOT NULL,
	[CreatedUtc] [datetime2](7) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[PasswordResetToken] [nvarchar](max) NULL,
	[PasswordResetTokenExpiration] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [auth].[Users] ON 
GO
INSERT [auth].[Users] 
(
    [Id], 
    [FirstName], 
    [LastName], 
    [Username], 
    [Role], 
    [CreatedUtc], 
    [PasswordHash], 
    [PasswordResetToken], 
    [PasswordResetTokenExpiration]
) 
VALUES 
(
    1, 
    N'Rolf', 
    N'Veinø Sørensen', 
    N'rvs@easymodules.net', 
    1, 
    CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 
    N'$2a$11$uniuE9CTaTV/3dqbUHOzQ.ugjm5Yd7efdEPVYYgKQ1jRSHELwEmme', 
    N'$2a$11$DdhJ5lOi769tZO95PznW2u2obMYx/uOgoE1MjkEweqgrmA3OwC1Ba', 
    CAST(N'2024-07-16T00:07:28.9355702' AS DateTime2)
)
GO
SET IDENTITY_INSERT [auth].[Users] OFF
GO