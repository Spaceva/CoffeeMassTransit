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

CREATE TABLE [dbo].[CoffeeStates](
	[CorrelationId] [uniqueidentifier] NOT NULL,
	[CurrentState] [nvarchar](50) NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[ToppingsRequested] [nvarchar](50) NULL,
	[CoffeeTypeRequested] [int] NOT NULL,
	[Amount] [money] NOT NULL,
 CONSTRAINT [PK_CoffeeStates] PRIMARY KEY CLUSTERED 
(
	[CorrelationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Coffees](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Toppings] [nvarchar](50) NULL,
	[Done] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Coffees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[Amount] [money] NOT NULL,
	[IsValid] [bit] NOT NULL DEFAULT ((0)),
	[IsPaid] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
