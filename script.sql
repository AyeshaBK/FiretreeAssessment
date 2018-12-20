USE [master]
GO
/****** Object:  Database [PlacesDB]    Script Date: 2018/12/20 11:55:58 AM ******/
CREATE DATABASE [PlacesDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PlacesDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\PlacesDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PlacesDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\PlacesDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PlacesDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PlacesDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PlacesDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PlacesDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PlacesDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PlacesDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PlacesDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PlacesDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PlacesDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PlacesDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PlacesDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PlacesDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PlacesDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PlacesDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PlacesDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PlacesDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PlacesDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PlacesDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PlacesDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PlacesDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PlacesDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PlacesDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PlacesDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PlacesDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PlacesDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PlacesDB] SET  MULTI_USER 
GO
ALTER DATABASE [PlacesDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PlacesDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PlacesDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PlacesDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PlacesDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PlacesDB] SET QUERY_STORE = OFF
GO
USE [PlacesDB]
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 2018/12/20 11:55:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agent](
	[AgentId] [int] IDENTITY(1,1) NOT NULL,
	[ImagePath] [varchar](max) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Cellphone] [int] NOT NULL,
	[Email] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[AgentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lead]    Script Date: 2018/12/20 11:56:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lead](
	[LeadId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Message] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Lead] PRIMARY KEY CLUSTERED 
(
	[LeadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listing]    Script Date: 2018/12/20 11:56:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listing](
	[ListingId] [int] IDENTITY(1,1) NOT NULL,
	[ImagePath] [varchar](max) NOT NULL,
	[Price] [float] NOT NULL,
	[Bedrooms] [int] NOT NULL,
	[RefNumber] [int] NOT NULL,
	[MarketingHeading] [varchar](max) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Suburb] [varchar](50) NOT NULL,
	[AgentId] [int] NULL,
 CONSTRAINT [PK_Listing] PRIMARY KEY CLUSTERED 
(
	[ListingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Listing]  WITH CHECK ADD  CONSTRAINT [FK_Listing_Agent] FOREIGN KEY([AgentId])
REFERENCES [dbo].[Agent] ([AgentId])
GO
ALTER TABLE [dbo].[Listing] CHECK CONSTRAINT [FK_Listing_Agent]
GO
USE [master]
GO
ALTER DATABASE [PlacesDB] SET  READ_WRITE 
GO
