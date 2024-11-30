USE [master]
GO

CREATE DATABASE [DBThoiTrang]
CONTAINMENT = NONE
ON PRIMARY 
(
    NAME = N'DBNoiThat',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DBNoiThat.mdf',
    SIZE = 4096KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 1024KB
)
LOG ON 
(
    NAME = N'DBNoiThat_log',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DBNoiThat_log.ldf',
    SIZE = 2560KB,
    MAXSIZE = 2048GB,
    FILEGROWTH = 10%
)
GO

USE [DBThoiTrang]
GO

CREATE TABLE [dbo].[Card](
    [CardId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [NumberCard] [int] NULL,
    [UserNumber] [int] NULL,
    [UserId] [int] NULL,
    [Identification] [int] NULL
)
GO

CREATE TABLE [dbo].[Category](
    [CategoryId] [int] NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL,
    [MetaTitle] [nvarchar](50) NULL,
    [ParId] [int] NULL
)
GO

CREATE TABLE [dbo].[Contact](
    [ContactId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Content] [nvarchar](max) NULL,
    [Status] [bit] NULL,
    [EmailCC] [nvarchar](50) NULL
)
GO

CREATE TABLE [dbo].[Credentials](
    [CredenId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserGroupId] [nvarchar](50) NULL,
    [RoleId] [nvarchar](50) NULL
)
GO

CREATE TABLE [dbo].[News](
    [NewsId] [int] NOT NULL PRIMARY KEY,
    [Title] [nvarchar](max) NULL,
    [Detail] [nvarchar](max) NULL,
    [Photo] [nvarchar](max) NULL,
    [DateUpdate] [date] NULL
)
GO

CREATE TABLE [dbo].[Order](
    [OrderId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ShipName] [nvarchar](50) NULL,
    [UserId] [int] NULL,
    [ShipPhone] [int] NULL,
    [ShipEmail] [nvarchar](max) NULL,
    [UpdateDate] [datetime] NULL,
    [ShipAddress] [nvarchar](max) NULL,
    [StatusId] [int] NULL
)
GO

CREATE TABLE [dbo].[OrderDetail](
    [OrderDetailId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [OrderId] [int] NULL,
    [ProductId] [int] NULL,
    [Price] [int] NULL,
    [Quantity] [int] NULL
)
GO

CREATE TABLE [dbo].[Product](
    [ProductId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL,
    [Description] [nvarchar](max) NULL,
    [Price] [int] NULL,
    [Quantity] [int] NULL,
    [ProviderId] [int] NULL,
    [CateId] [int] NULL,
    [Photo] [nvarchar](max) NULL,
    [StartDate] [date] NULL,
    [EndDate] [date] NULL,
    [Discount] [int] NULL,
    [IsHidden] bit NULL
)
GO

CREATE TABLE [dbo].[Provider](
    [ProviderId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL,
    [Address] [nvarchar](max) NULL,
    [Phone] [int] NULL
)
GO

CREATE TABLE [dbo].[Role](
    [RoleId] [nvarchar](50) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL
)
GO

CREATE TABLE [dbo].[Status](
    [StatusId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL
)
GO

CREATE TABLE [dbo].[User](
    [UserId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL,
    [Address] [nvarchar](50) NULL,
    [Phone] [int] NULL,
    [Username] [nvarchar](50) NULL,
    [Password] [nvarchar](50) NULL,
    [Email] [nvarchar](max) NULL,
    [GroupId] [nvarchar](50) NULL,
    [Status] [bit] NOT NULL
)
GO

CREATE TABLE [dbo].[UserGroups](
    [GroupId] [nvarchar](50) NOT NULL PRIMARY KEY,
    [Name] [nvarchar](50) NULL
)
GO

-- Foreign Key Constraints
ALTER TABLE [dbo].[Card] ADD CONSTRAINT [FK_Card_User] 
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]

ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_User] 
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [FK_Order_Status] 
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].Status

ALTER TABLE [dbo].[OrderDetail] ADD CONSTRAINT [FK_OrderDetail_Order] 
    FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order]
ALTER TABLE [dbo].[OrderDetail] ADD CONSTRAINT [FK_OrderDetail_Product] 
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]

ALTER TABLE [dbo].[Product] ADD CONSTRAINT [FK_Product_Provider] 
    FOREIGN KEY ([ProviderId]) REFERENCES [dbo].Provider
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [FK_Product_Category] 
    FOREIGN KEY ([CateId]) REFERENCES [dbo].Category

ALTER TABLE [dbo].[Credentials] ADD CONSTRAINT [FK_Credentials_UserGroups] 
    FOREIGN KEY ([UserGroupId]) REFERENCES [dbo].UserGroups
ALTER TABLE [dbo].[Credentials] ADD CONSTRAINT [FK_Credentials_Role] 
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].Role

ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_GroupId] 
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].UserGroups

ALTER TABLE [dbo].[Card]
ADD CONSTRAINT UQ_Card_UserId UNIQUE (UserId);