USE [FAIS]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BO]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BO](
	[BO_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [char](10) NULL,
	[BO_TYPE] [varchar](100) NULL,
	[VERSION] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BO_CHILDS]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BO_CHILDS](
	[BO_PARENT_ID] [bigint] NOT NULL,
	[BO_CHILD_ID] [bigint] NOT NULL,
	[RELATION] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_PARENT_ID] ASC,
	[BO_CHILD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BO_ROLE]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BO_ROLE](
	[BO_ROLE_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[META_BO_ID] [bigint] NOT NULL,
	[ROLE_ID] [nvarchar](128) NOT NULL,
	[CAN_READ] [bit] NULL,
	[CAN_WRITE] [bit] NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](10) NULL,
	[PAGE_ID] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_ROLE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[META_BO]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[META_BO](
	[META_BO_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BO_NAME] [varchar](100) NULL,
	[VERSION] [int] NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](50) NULL,
	[BO_DB_NAME] [varchar](50) NULL,
	[TYPE] [varchar](50) NULL,
	[JSON_DATA] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[META_BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[META_FIELD]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[META_FIELD](
	[META_FIELD_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[META_BO_ID] [bigint] NOT NULL,
	[DB_NAME] [varchar](100) NOT NULL,
	[DB_TYPE] [varchar](20) NOT NULL,
	[DB_NULL] [int] NOT NULL,
	[GRID_NAME] [varchar](100) NOT NULL,
	[GRID_FORMAT] [nvarchar](max) NULL,
	[GRID_SHOW] [int] NULL,
	[FORM_NAME] [varchar](100) NULL,
	[FORM_FORMAT] [nvarchar](max) NULL,
	[FORM_TYPE] [varchar](100) NOT NULL,
	[FORM_SOURCE] [nvarchar](max) NULL,
	[FORM_SHOW] [int] NULL,
	[FORM_OPTIONAL] [int] NULL,
	[IS_FILTER] [int] NULL,
	[FORM_DEFAULT] [varchar](100) NULL,
	[CREATED_BY] [varchar](100) NOT NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](50) NULL,
	[VERSION] [int] NULL,
	[JSON_DATA] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[META_FIELD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NOTIF]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NOTIF](
	[ID_NOTIF] [int] IDENTITY(1,1) NOT NULL,
	[VALIDATOR] [varchar](50) NULL,
	[ETAT] [int] NULL,
	[CREATED_DATE] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID_NOTIF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PAGE]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PAGE](
	[BO_ID] [bigint] NOT NULL,
	[TITLE] [varchar](50) NULL,
	[GROUPE] [varchar](50) NULL,
	[STATUS] [varchar](10) NULL,
	[LAYOUT] [nvarchar](max) NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlusSequence]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlusSequence](
	[SequenceID] [int] IDENTITY(1,1) NOT NULL,
	[cle] [varchar](500) NULL,
	[TableName] [varchar](50) NULL,
	[StartValue] [int] NULL,
	[StepBy] [int] NULL,
	[CurrentValue] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SequenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TASK]    Script Date: 18-Jul-19 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TASK](
	[TASK_ID] [int] IDENTITY(1,1) NOT NULL,
	[BO_ID] [int] NULL,
	[JSON_DATA] [nvarchar](max) NULL,
	[STATUS] [varchar](50) NULL,
	[ETAT] [int] NULL,
	[TASK_LEVEL] [int] NULL,
	[TASK_TYPE] [varchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[CREATED_BY] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TASK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VERSIONS]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VERSIONS](
	[VERSIONS_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[META_BO_ID] [bigint] NULL,
	[NUM] [int] NOT NULL,
	[SQLQUERY] [varchar](max) NOT NULL,
	[CREATED_BY] [varchar](100) NULL,
	[CREATED_DATE] [datetime] NULL,
	[UPDATED_BY] [varchar](100) NULL,
	[UPDATED_DATE] [datetime] NULL,
	[STATUS] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[VERSIONS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WORKFLOW]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFLOW](
	[BO_ID] [bigint] NOT NULL,
	[LIBELLE] [varchar](50) NULL,
	[ACTIVE] [int] NULL,
	[ITEMS] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[BO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'adminId', N'admin')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'46c09129-06b7-4fd6-b3cc-476684989390', N'adminId')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'fb0c8a78-571e-45e4-83fb-87701f197e42', N'adminId')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'46c09129-06b7-4fd6-b3cc-476684989390', N'noureddine.boudass@gmail.com', 0, N'AAQeH2AToBISNDcfoQKa9N4M8KPT2OG0oWrmuEpv94M93e9NlYC3+/kksZ5EREbpvQ==', N'78631ce5-b3d0-4b15-869e-a8e69ca8b172', NULL, 0, 0, NULL, 0, 0, N'noureddine.boudass@gmail.com')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'fb0c8a78-571e-45e4-83fb-87701f197e42', N'simo@simo.com', 0, N'ACDc65qNG4LZH85JSw2BAH4k02aDt4oWlfr39C4ZWgtiewLx6zSpoeiYA3qeVXbRVw==', N'12db9646-e34d-43ad-aec6-30e262337a42', NULL, 0, 0, NULL, 0, 0, N'simo@simo.com')
GO
SET IDENTITY_INSERT [dbo].[META_BO] ON 
GO
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE], [JSON_DATA]) VALUES (1, N'META_BO', 1, N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'-1', N'META_BO', NULL, NULL)
GO
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE], [JSON_DATA]) VALUES (2, N'WORKFLOW', 1, N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'-1', N'WORKFLOW', NULL, NULL)
GO
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE], [JSON_DATA]) VALUES (3, N'Page', 1, N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'-1', N'PAGE', N'form', N'{"TITLE":"","GROUPE":null}')
GO
SET IDENTITY_INSERT [dbo].[META_BO] OFF
GO
SET IDENTITY_INSERT [dbo].[META_FIELD] ON 
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (1, 1, N'META_BO_ID', N'bigint', 0, N'#', NULL, 0, N'', NULL, N'', NULL, 0, 0, NULL, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'PK', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (2, 1, N'BO_NAME', N'varchar(100)', 1, N'Nom', NULL, 1, N'Nom', NULL, N'v-text', NULL, 1, 0, 0, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'amdin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (3, 1, N'STATUS', N'varchar(50)', 1, N'Nom', NULL, 1, N'STATUS', NULL, N'v-label', NULL, 1, 0, NULL, N'PENDING', N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (4, 1, N'TYPE', N'varchar(50)', 1, N'Type', N'{"fct":"Display","source":"TYPE"}', 1, N'Type', NULL, N'v-select', N'[{ "Value": "form", "Display": "FORM" }, { "Value": "subform", "Display": "SUB FORM" }]', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (5, 2, N'LIBELLE', N'varchar(50)', 1, N'Libelle', N'', 1, N'Libelle', NULL, N'v-text', N'', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (6, 2, N'ACTIVE', N'int', 1, N'Active', N'', 1, N'Active', NULL, N'v-checkbox', N'', 1, 1, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (7, 2, N'additional.column', N'', 1, N'', N'{"type":"button", "color":"btn-success","icon":"add","action":"redirect", "data":"#workflow.home"}', 1, N'', NULL, N'', N'', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (8, 3, N'TITLE', N'varchar(100)', 1, N'TITLE', NULL, 1, N'TITLE', NULL, N'v-text', N'', 1, 0, 0, NULL, N'admin', NULL, N'admin', NULL, N'LOCKED', NULL, N'')
GO
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) VALUES (9, 3, N'additional.column', N'', 1, N'', N'{"type":"button", "color":"btn-success","icon":"settings","action":"redirect", "data":"#admin.page"}', 1, N'', NULL, N'', N'', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[META_FIELD] OFF
GO
ALTER TABLE [dbo].[BO] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[BO] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[BO_ROLE] ADD  DEFAULT ((0)) FOR [CAN_READ]
GO
ALTER TABLE [dbo].[BO_ROLE] ADD  DEFAULT ((0)) FOR [CAN_WRITE]
GO
ALTER TABLE [dbo].[BO_ROLE] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[BO_ROLE] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_BO] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[META_BO] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ((1)) FOR [DB_NULL]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ((1)) FOR [GRID_SHOW]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ((1)) FOR [FORM_SHOW]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ((0)) FOR [FORM_OPTIONAL]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ((0)) FOR [IS_FILTER]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[META_FIELD] ADD  DEFAULT ('ACTIVE') FOR [STATUS]
GO
ALTER TABLE [dbo].[VERSIONS] ADD  DEFAULT (getdate()) FOR [CREATED_DATE]
GO
ALTER TABLE [dbo].[VERSIONS] ADD  DEFAULT (getdate()) FOR [UPDATED_DATE]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[META_FIELD]  WITH CHECK ADD FOREIGN KEY([META_BO_ID])
REFERENCES [dbo].[META_BO] ([META_BO_ID])
GO
ALTER TABLE [dbo].[VERSIONS]  WITH CHECK ADD FOREIGN KEY([META_BO_ID])
REFERENCES [dbo].[META_BO] ([META_BO_ID])
GO
/****** Object:  StoredProcedure [dbo].[cleanMetaBo]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--/****** Script de la commande SelectTopNRows à partir de SSMS  ******/
--SELECT * from VERSIONS where META_BO_ID=11


--select * from META_FIELD where META_BO_ID=11

--select * from VIEW_KA_BO_





CREATE procedure [dbo].[cleanMetaBo]
as
delete from META_FIELD where META_BO_ID not in (select META_BO_ID from META_BO where STATUS ='-1')
delete from VERSIONS where META_BO_ID not in (select META_BO_ID from META_BO where STATUS ='-1')
delete from META_BO where STATUS <>'-1'
delete from bo

GO
/****** Object:  StoredProcedure [dbo].[GetSubForm]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SimoRML>
-- Create date: <25/03/2019>
-- =============================================
CREATE PROCEDURE [dbo].[GetSubForm]
	@meta_bo_id int
AS
BEGIN

	select replace(FORM_TYPE,'subform-', '') as subform from META_FIELD 
	where META_BO_ID = @meta_bo_id AND FORM_TYPE like 'subform-%'

END

GO
/****** Object:  StoredProcedure [dbo].[GetSubFormId]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSubFormId]
	@meta_bo_id int
AS
BEGIN
	DECLARE @sub varchar(50);
	select @sub = replace(FORM_TYPE,'subform-', '') from META_FIELD 
	where META_BO_ID = @meta_bo_id AND FORM_TYPE like 'subform-%'

	SELECT META_BO_ID FROM META_BO WHERE BO_DB_NAME = @sub;
END


GO
/****** Object:  StoredProcedure [dbo].[InitMetaBo]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[InitMetaBo]
AS
BEGIN

IF not exists( SELECT object_id 
FROM sys.indexes 
WHERE name='UNIQUE_META_BO_BO_DB_NAME' AND object_id = OBJECT_ID('dbo.META_BO')
)
BEGIN
       CREATE UNIQUE INDEX UNIQUE_META_BO_BO_DB_NAME   
   ON dbo.META_BO (BO_DB_NAME);   
END

IF not exists( SELECT object_id 
FROM sys.indexes 
WHERE name='UNIQUE_META_FIELD_DB_NAME' AND object_id = OBJECT_ID('dbo.META_FIELD')
)
BEGIN
       CREATE UNIQUE INDEX UNIQUE_META_FIELD_DB_NAME
       ON dbo.META_FIELD (META_BO_ID, DB_NAME);   
END

delete from META_FIELD;
delete from VERSIONS;
delete from META_BO;
delete from bo;
delete from BO_CHILDS;
delete from PlusSequence;
delete from [PAGE];
delete from WORKFLOW;
delete from TASK;
delete from NOTIF;
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;

SET IDENTITY_INSERT [dbo].[META_BO] ON ;
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE]) 
VALUES (1, N'META_BO', 1, N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-11-11T13:06:41.347' AS DateTime), N'-1', N'META_BO', NULL);

INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE]) 
VALUES (2, N'WORKFLOW', 1, N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'admin', CAST(N'2018-12-18T13:06:41.347' AS DateTime), N'-1', N'WORKFLOW', NULL);
/*PAGE */
INSERT [dbo].[META_BO] ([META_BO_ID], [BO_NAME], [VERSION], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [BO_DB_NAME], [TYPE], [JSON_DATA]) VALUES (3, N'Page', 1, N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'admin', CAST(N'2019-03-16T17:53:01.400' AS DateTime), N'-1', N'PAGE', N'form', N'{"TITLE":"","GROUPE":null}')
SET IDENTITY_INSERT [dbo].[META_BO] OFF;
SET IDENTITY_INSERT [dbo].[META_FIELD] ON ;
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (1, 1, N'META_BO_ID', N'bigint', 0, N'#', NULL, 0, N'', NULL, N'', NULL, 0, 0, NULL, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'PK', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (2, 1, N'BO_NAME', N'varchar(100)', 1, N'Nom', NULL, 1, N'Nom', NULL, N'v-text', NULL, 1, 0, 0, NULL, N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'amdin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (3, 1, N'STATUS', N'varchar(50)', 1, N'Nom', NULL, 1, N'STATUS', NULL, N'v-label', NULL, 1, 0, NULL, N'PENDING', N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-11-11T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) VALUES (4, 1, N'TYPE', N'varchar(50)', 1, N'Type', N'{"fct":"Display","source":"TYPE"}', 1, N'Type', NULL, N'v-select', N'[{ "Value": "form", "Display": "FORM" }, { "Value": "subform", "Display": "SUB FORM" }]', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-03T13:10:28.673' AS DateTime), N'LOCKED', NULL);

INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (5, 2, N'LIBELLE', N'varchar(50)', 1, N'Libelle', '', 1, N'Libelle', NULL, N'v-text', '', 1, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (6, 2, N'ACTIVE', N'int', 1, N'Active', '', 1, N'Active', NULL, N'v-checkbox', '', 1, 1, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (7, 2, N'additional.column', '', 1, N'', '{"type":"button", "color":"btn-success","icon":"add","action":"redirect", "data":"#workflow.home"}', 1, N'', NULL, N'', '', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);
/*PAGE FIELD*/
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION], [JSON_DATA]) 
VALUES (8, 3, N'TITLE', N'varchar(100)', 1, N'TITLE', NULL, 1, N'TITLE', NULL, N'v-text', N'', 1, 0, 0, NULL, N'admin', NULL, N'admin', NULL, N'LOCKED', NULL, N'')
INSERT [dbo].[META_FIELD] ([META_FIELD_ID], [META_BO_ID], [DB_NAME], [DB_TYPE], [DB_NULL], [GRID_NAME], [GRID_FORMAT], [GRID_SHOW], [FORM_NAME], [FORM_FORMAT], [FORM_TYPE], [FORM_SOURCE], [FORM_SHOW], [FORM_OPTIONAL], [IS_FILTER], [FORM_DEFAULT], [CREATED_BY], [CREATED_DATE], [UPDATED_BY], [UPDATED_DATE], [STATUS], [VERSION]) 
VALUES (9, 3, N'additional.column', '', 1, N'', '{"type":"button", "color":"btn-success","icon":"settings","action":"redirect", "data":"#admin.page"}', 1, N'', NULL, N'', '', 0, 0, NULL, N'form', N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'admin', CAST(N'2018-12-18T13:10:28.673' AS DateTime), N'LOCKED', NULL);

SET IDENTITY_INSERT [dbo].[META_FIELD] OFF;

DECLARE @tname varchar(100);
DECLARE @sql nvarchar(100);
DECLARE _bo_cursor CURSOR FOR 
             SELECT t.name FROM SYSOBJECTS t WHERE xtype in ( 'U' ) AND (t.name like '%_BO_%' );

OPEN _bo_cursor  
FETCH NEXT FROM _bo_cursor INTO @tname

WHILE @@FETCH_STATUS = 0  
BEGIN  
       SET @sql = 'DROP TABLE ' + @tname;
       EXEC sp_executesql @sql;
       FETCH NEXT FROM _bo_cursor INTO @tname
END 

CLOSE _bo_cursor  
DEALLOCATE _bo_cursor 

END


GO
/****** Object:  StoredProcedure [dbo].[INSERT_BO_LIGNES]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_BO_LIGNES]
	@nbrLignes int,
	@BO_NAME varchar(100)
AS

	Declare @req nvarchar(MAX);
	DECLARE @cnt INT = 0;
	DECLARE @BO_ID INT = 0;
	DECLARE @META_BO_ID int = 0;
		select @META_BO_ID = META_BO_ID from META_BO where META_BO.BO_DB_NAME = @BO_NAME;

	if @META_BO_ID = 0 BEGIN	
		set @nbrLignes = 0;
	END
	DECLARE @currentVersion int;
		select @currentVersion=[VERSION] from META_BO where META_BO_ID= @META_BO_ID;
		

	WHILE @cnt < @nbrLignes
	BEGIN

	   INSERT INTO [dbo].[BO]
			   ([CREATED_BY]
			   ,[CREATED_DATE]
			   ,[UPDATED_BY]
			   ,[UPDATED_DATE]
			   ,[STATUS]
			   ,[BO_TYPE]
			   ,[VERSION])
		 VALUES
			   ('sys'
			   ,getdate()
			   ,'sys'
			   ,getdate()
			   ,'1'
			   ,@META_BO_ID
			   ,@currentVersion
			   )
		
		SELECT @BO_ID = SCOPE_IDENTITY();  
		set @req = N'insert into ' + @BO_NAME + N'(BO_ID) values('+ convert(varchar,@BO_ID)+N') ';
		exec sp_executesql @req;
			   
	   SET @cnt = @cnt + 1;
END;

GO
/****** Object:  StoredProcedure [dbo].[MoveBoToCurrentVersion]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
CREATE PROCEDURE [dbo].[MoveBoToCurrentVersion]
	@BO_ID bigint
AS
BEGIN
	print '-----MoveBoToCurrentVersion-----';
	print '@BO_ID : ' + convert(varchar,@BO_ID);
	DECLARE @metBoId int;
	DECLARE @boVersion int;
		select @metBoId=BO_TYPE, @boVersion=[VERSION] from BO where BO_ID = @BO_ID;
	
	DECLARE @currentVersion int;
	DECLARE @boDbName varchar(100);
		select @boDbName = BO_DB_NAME, @currentVersion=[VERSION] from META_BO where META_BO_ID= @metBoId;
	DECLARE @currentTable varchar(100) set @currentTable = @boDbName + convert(varchar, @currentVersion);
	DECLARE @boTable varchar(100) set @boTable = @boDbName + convert(varchar, @boVersion);

	print '@metBoId : ' + convert(varchar,@metBoId);
	print '@boDbName : ' + @boDbName;
	print '@boVersion : ' + convert(varchar,@boVersion);
	print '@boTable : ' + @boTable;
	print '@currentVersion : ' + convert(varchar,@currentVersion);
	print '@currentTable : ' + @currentTable;

	IF @boVersion != @currentVersion BEGIN
		
		DECLARE @oneField varchar(100);
		DECLARE @fields varchar(MAX) set @fields = '[BO_ID]';
		
		
		DECLARE fields_cursor CURSOR FOR 
			select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [VERSION] <= @boVersion AND FORM_TYPE not like 'subform-%';

		OPEN fields_cursor  
		FETCH NEXT FROM fields_cursor INTO @oneField

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			set @fields += ', [' + @onefield + ']';
			FETCH NEXT FROM fields_cursor INTO @oneField
		END 

		CLOSE fields_cursor  
		DEALLOCATE fields_cursor 

		print '@fields : ' + @fields;
		
		Declare @insertStatement nvarchar(MAX);
			set @insertStatement = 'insert into ' + @currentTable + '('+@fields+') select * from '+@boTable+' where BO_ID=' + convert(varchar,@BO_ID);
		print '@@insertStatement : ' + @insertStatement;

		Declare @deleteStatement nvarchar(MAX);
			set @deleteStatement = 'delete from '+@boTable+' where BO_ID=' + convert(varchar,@BO_ID);
		print '@@deleteStatement  : ' + @deleteStatement ;

		BEGIN TRAN TR_SWITCH;
			exec sp_executesql @insertStatement;
			exec sp_executesql @deleteStatement;
			update BO set [VERSION]=@currentVersion where BO_ID = @BO_ID;
		COMMIT TRAN TR_SWITCH;
	END
	print '--------------------------------';
END


GO
/****** Object:  StoredProcedure [dbo].[MoveFromTmp]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<SimoRML>
-- Create date: <02-12-2018>
-- Description:	<Move BO from old table version to the current version (FOR UPDATE) >
-- =============================================
CREATE PROCEDURE [dbo].[MoveFromTmp]
	@metBoId int
AS
BEGIN
	print '-----[MoveFromTmp]-----';
	
	DECLARE @currentVersion int;
	DECLARE @table varchar(100);
		select @table = BO_DB_NAME, @currentVersion=[VERSION] from META_BO where META_BO_ID= @metBoId;
	DECLARE @table_tmp varchar(100); set @table_tmp = @table + 'tmp';
	
	print '@currentVersion : ' + convert(varchar,@table);
	print '@table : ' + convert(varchar,@table);
	print '@table_tmp : ' + convert(varchar,@table_tmp);

		
	DECLARE @oneField varchar(100);
	DECLARE @fields varchar(MAX) set @fields = '[BO_ID]';
			
	DECLARE fields_cursor CURSOR FOR 
		select DB_NAME from META_FIELD where META_BO_ID = @metBoId AND [STATUS] <> 'NEW' AND FORM_TYPE not like 'subform-%';

	OPEN fields_cursor  
	FETCH NEXT FROM fields_cursor INTO @oneField

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @fields += ', [' + @onefield + ']';
		FETCH NEXT FROM fields_cursor INTO @oneField
	END 

	CLOSE fields_cursor  
	DEALLOCATE fields_cursor 

	print '@fields : ' + @fields;
		
	Declare @insertStatement nvarchar(MAX);
		set @insertStatement = 'insert into ' + @table + '('+@fields+') select * from '+@table_tmp;
	print '@@insertStatement : ' + @insertStatement;

	exec sp_executesql @insertStatement;

	print '--------------------------------';
END


GO
/****** Object:  StoredProcedure [dbo].[PlusSequenceNextID]    Script Date: 18-Jul-19 19:17:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<SimoRML>
-- Create date: <08/02/2019>
-- Description:	<Identity by source (ex: BLABLA_513)>
-- =============================================
CREATE PROCEDURE [dbo].[PlusSequenceNextID]
	@cle varchar(500),
	@TableName varchar(500),
	@stepBy int,
	@presist int
AS
BEGIN
	SET NOCOUNT ON; 

	DECLARE @SequenceID int;
	DECLARE @StartValue int;
	DECLARE @CurrentValue int;
	-- INITIATE SEQUENCE
	select @SequenceID = SequenceID from PlusSequence where cle = @cle AND TableName = @TableName
    if @SequenceID is null
	Begin
		insert into PlusSequence (cle, TableName, StartValue, StepBy, CurrentValue)
		values (@cle, @TableName, 1, @stepBy, 0);
		SELECT @SequenceID = SCOPE_IDENTITY();
	END	

	-- CALCULATE NEXT ID
	select @StartValue=StartValue, @StepBy=StepBy, @CurrentValue=CurrentValue from PlusSequence where SequenceID = @SequenceID;

	if @CurrentValue < @StartValue 
	begin
		SET @CurrentValue = @StartValue;
	end
	else
	begin		
		SET @CurrentValue = @CurrentValue + @StepBy;
	end

	if @presist = 1
	begin
		update PlusSequence set CurrentValue = @CurrentValue where SequenceID = @SequenceID;
	end
	select convert(varchar,@CurrentValue);
END

GO
