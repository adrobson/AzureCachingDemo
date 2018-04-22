﻿USE [master]
GO
/****** Object:  Database [CachingDemo]    Script Date: 22/04/2018 17:50:49 ******/
CREATE DATABASE [CachingDemo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CachingDemo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CachingDemo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CachingDemo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CachingDemo_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CachingDemo] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CachingDemo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CachingDemo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CachingDemo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CachingDemo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CachingDemo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CachingDemo] SET ARITHABORT OFF 
GO
ALTER DATABASE [CachingDemo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CachingDemo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CachingDemo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CachingDemo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CachingDemo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CachingDemo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CachingDemo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CachingDemo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CachingDemo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CachingDemo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CachingDemo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CachingDemo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CachingDemo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CachingDemo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CachingDemo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CachingDemo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CachingDemo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CachingDemo] SET RECOVERY FULL 
GO
ALTER DATABASE [CachingDemo] SET  MULTI_USER 
GO
ALTER DATABASE [CachingDemo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CachingDemo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CachingDemo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CachingDemo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CachingDemo] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CachingDemo', N'ON'
GO
ALTER DATABASE [CachingDemo] SET QUERY_STORE = OFF
GO
USE [CachingDemo]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [CachingDemo]
GO
/****** Object:  Table [dbo].[Model]    Script Date: 22/04/2018 17:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Model](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Model] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ModelData]    Script Date: 22/04/2018 17:50:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModelId] [int] NOT NULL,
	[PreviousId] [int] NULL,
	[Amount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ModelData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ModelData]  WITH CHECK ADD  CONSTRAINT [FK_ModelData_Model] FOREIGN KEY([ModelId])
REFERENCES [dbo].[Model] ([Id])
GO
ALTER TABLE [dbo].[ModelData] CHECK CONSTRAINT [FK_ModelData_Model]
GO
USE [master]
GO
ALTER DATABASE [CachingDemo] SET  READ_WRITE 
GO