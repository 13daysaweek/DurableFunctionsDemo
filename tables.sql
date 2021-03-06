USE [SalesHistory]
GO
/****** Object:  Table [dbo].[SalesAnomalyResults]    Script Date: 1/22/2021 10:57:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesAnomalyResults](
	[SalesAnomalyId] [int] IDENTITY(1,1) NOT NULL,
	[RunIdentifier] [nvarchar](50) NOT NULL,
	[Region] [nvarchar](10) NOT NULL,
	[Division] [nvarchar](10) NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[AnomalyCalculationResult] [decimal](18, 4) NULL,
 CONSTRAINT [PK_SalesAnomalyResults] PRIMARY KEY CLUSTERED 
(
	[SalesAnomalyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesData]    Script Date: 1/22/2021 10:57:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesData](
	[SalesDataId] [int] IDENTITY(1,1) NOT NULL,
	[Region] [nvarchar](10) NOT NULL,
	[Division] [nvarchar](10) NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[TransactionAmount] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_SalesData] PRIMARY KEY CLUSTERED 
(
	[SalesDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
