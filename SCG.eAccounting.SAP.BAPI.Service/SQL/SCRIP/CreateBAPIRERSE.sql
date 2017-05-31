CREATE TABLE [dbo].[BAPIREVERSE](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_KIND] [varchar](50) COLLATE Thai_CI_AS NULL,
	[DOC_ID] [bigint] NOT NULL,
	[DOC_SEQ] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[DOC_YEAR] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[DOC_APP_FLAG] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[FI_DOC] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[OBJ_TYPE] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[OBJ_KEY] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[OBJ_SYS] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[REVERSE_DOC] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[ReverseDocYear] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ReverseDocFlag] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ReverseDocMsg] [nvarchar](200) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIREVERSE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
