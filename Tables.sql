USE [Webinar]
GO

CREATE TABLE [dbo].[CoffeeMachineSagas](
	[CorrelationId] [uniqueidentifier] NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[ToppingsRequested] [nvarchar](50) NULL,
	[CoffeeTypeRequested] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[State] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CoffeeMachineSagas] PRIMARY KEY CLUSTERED 
(
	[CorrelationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Coffees](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Toppings] [nvarchar](50) NULL,
	[Done] [bit] NOT NULL,
 CONSTRAINT [PK_Coffees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Coffees]  WITH CHECK ADD  CONSTRAINT [FK_Coffees_CoffeeMachineSagas] FOREIGN KEY([Id])
REFERENCES [dbo].[CoffeeMachineSagas] ([CorrelationId])
GO

ALTER TABLE [dbo].[Coffees] CHECK CONSTRAINT [FK_Coffees_CoffeeMachineSagas]
GO

CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[Amount] [money] NOT NULL,
	[IsValid] [bit] NOT NULL,
	[IsPaid] [bit] NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Payments] ADD  CONSTRAINT [DF_Payments_IsValid]  DEFAULT ((0)) FOR [IsValid]
GO

ALTER TABLE [dbo].[Payments] ADD  CONSTRAINT [DF_Payments_IsPaid]  DEFAULT ((0)) FOR [IsPaid]
GO

ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_CoffeeMachineSagas] FOREIGN KEY([OrderId])
REFERENCES [dbo].[CoffeeMachineSagas] ([CorrelationId])
GO

ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_CoffeeMachineSagas]
GO

