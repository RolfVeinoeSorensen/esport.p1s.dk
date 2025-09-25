INSERT INTO [auth].[AuthRoles]
           ([Role])
     VALUES
           (1)
GO
INSERT INTO [auth].[AuthRoles]
           ([Role])
     VALUES
           (2)
GO
INSERT INTO [auth].[AuthRoles]
           ([Role])
     VALUES
           (3)
GO
INSERT INTO [auth].[AuthRoles]
           ([Role])
     VALUES
           (4)
GO
INSERT INTO [auth].[AuthRoles]
           ([Role])
     VALUES
           (5)
GO
SET IDENTITY_INSERT [auth].[AuthUsers] ON 
GO
INSERT [auth].[AuthUsers] 
(
    [Id], 
    [FirstName], 
    [LastName], 
    [Username], 
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
    CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 
    N'$2a$11$uniuE9CTaTV/3dqbUHOzQ.ugjm5Yd7efdEPVYYgKQ1jRSHELwEmme', 
    N'$2a$11$DdhJ5lOi769tZO95PznW2u2obMYx/uOgoE1MjkEweqgrmA3OwC1Ba', 
    CAST(N'2024-07-16T00:07:28.9355702' AS DateTime2)
)
GO
SET IDENTITY_INSERT [auth].[AuthUsers] OFF
GO
INSERT INTO [auth].[AuthUsersRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           (1,1)
GO
INSERT INTO [auth].[AuthUsersRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           (1,4)
GO