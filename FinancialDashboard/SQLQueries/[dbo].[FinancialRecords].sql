USE [Zorvyn]
GO

/****** Object:  Table [dbo].[FinancialRecords]    Script Date: 4/5/2026 2:03:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FinancialRecords](
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[RecordDate] [datetime2](7) NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FinancialRecords] ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[FinancialRecords] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[FinancialRecords] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[FinancialRecords] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO


