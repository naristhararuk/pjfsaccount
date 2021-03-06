USE [eAccounting]
GO
/****** Object:  Table [dbo].[BAPIACAP09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACAP09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[VENDOR_NO] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[GL_ACCOUNT] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[REF_KEY_1] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[REF_KEY_2] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[REF_KEY_3] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[COMP_CODE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BUS_AREA] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PMNTTRMS] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BLINE_DATE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[DSCT_DAYS1] [decimal](2, 2) NULL,
	[DSCT_DAYS2] [decimal](2, 2) NULL,
	[NETTERMS] [decimal](2, 2) NULL,
	[DSCT_PCT1] [decimal](3, 3) NULL,
	[DSCT_PCT2] [decimal](3, 3) NULL,
	[PYMT_METH] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PMTMTHSUPL] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PMNT_BLOCK] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[SCBANK_IND] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[SUPCOUNTRY] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[SUPCOUNTRY_ISO] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BLLSRV_IND] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[ALLOC_NMBR] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[ITEM_TEXT] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PO_SUB_NO] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PO_CHECKDG] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PO_REF_NO] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[W_TAX_CODE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BUSINESSPLACE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[SECTIONCODE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[INSTR1] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[INSTR2] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[INSTR3] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[INSTR4] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BRANCH] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PYMT_CUR] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PYMT_AMT] [decimal](12, 4) NULL,
	[PYMT_CUR_ISO] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[SP_GL_IND] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[TAX_CODE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[TAX_DATE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[ALT_PAYEE] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[ALT_PAYEE_BANK] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PARTNER_BK] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[BANK_ID] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PARTNER_GUID] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACAP09_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACAR09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACAR09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CUSTOMER] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[GL_ACCOUNT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[REF_KEY_1] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[REF_KEY_2] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[REF_KEY_3] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[COMP_CODE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BUS_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[PMNTTRMS] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BLINE_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[DSCT_DAYS1] [decimal](2, 2) NULL,
	[DSCT_DAYS2] [decimal](2, 2) NULL,
	[NETTERMS] [decimal](2, 2) NULL,
	[DSCT_PCT1] [decimal](3, 3) NULL,
	[DSCT_PCT2] [decimal](3, 3) NULL,
	[PYMT_METH] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[PMTMTHSUPL] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[PAYMT_REF] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[DUNN_KEY] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[DUNN_BLOCK] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[PMNT_BLOCK] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[VAT_REG_NO] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[ALLOC_NMBR] [nvarchar](18) COLLATE Thai_CI_AS NULL,
	[ITEM_TEXT] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PARTNER_BK] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SCBANK_IND] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[BUSINESSPLACE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SECTIONCODE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BRANCH] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[PYMT_CUR] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[PYMT_CUR_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[PYMT_AMT] [decimal](12, 4) NULL,
	[C_CTR_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BANK_ID] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[SUPCOUNTRY] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[SUPCOUNTRY_ISO] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[TAX_CODE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[TAX_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[SP_GL_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[PARTNER_GUID] [nvarchar](32) COLLATE Thai_CI_AS NULL,
	[ALT_PAYEE] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ALT_PAYEE_BANK] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[DUNN_AREA] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACAR09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACCAHD]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACCAHD](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[DOC_TYPE_CA] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[RES_KEY] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[FIKEY] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[PAYMENT_FORM_REF] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACCAHD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACCAIT]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACCAIT](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CONT_ACCT] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[MAIN_TRANS] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SUB_TRANS] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[FUNC_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[FM_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[CMMT_ITEM] [nvarchar](14) COLLATE Thai_CI_AS NULL,
	[FUNDS_CTR] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[FUND] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[FUNC_AREA_LONG] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[CMMT_ITEM_LONG] [nvarchar](24) COLLATE Thai_CI_AS NULL,
	[GRANT_NBR] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACCAIT] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACCR09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACCR09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CURR_TYPE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[CURRENCY] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[CURRENCY_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[AMT_DOCCUR] [decimal](12, 4) NULL,
	[EXCH_RATE] [decimal](5, 5) NULL,
	[EXCH_RATE_V] [decimal](5, 5) NULL,
	[AMT_BASE] [decimal](12, 4) NULL,
	[DISC_BASE] [decimal](12, 4) NULL,
	[DISC_AMT] [decimal](12, 4) NULL,
	[TAX_AMT] [decimal](12, 4) NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACCR09_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACEXTC]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACEXTC](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[FIELD1] [nvarchar](250) COLLATE Thai_CI_AS NULL,
	[FIELD2] [nvarchar](250) COLLATE Thai_CI_AS NULL,
	[FIELD3] [nvarchar](250) COLLATE Thai_CI_AS NULL,
	[FIELD4] [nvarchar](250) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACEXTC_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACGL09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACGL09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[GL_ACCOUNT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ITEM_TEXT] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[STAT_CON] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[LOG_PROC] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[AC_DOC_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[REF_KEY_1] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[REF_KEY_2] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[REF_KEY_3] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[ACCT_KEY] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[ACCT_TYPE] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[DOC_TYPE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[COMP_CODE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BUS_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[FUNC_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[PLANT] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[FIS_PERIOD] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[FISC_YEAR] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[PSTNG_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[VALUE_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[FM_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[CUSTOMER] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CSHDIS_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[VENDOR_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ALLOC_NMBR] [nvarchar](18) COLLATE Thai_CI_AS NULL,
	[TAX_CODE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[EXT_OBJECT_ID] [nvarchar](34) COLLATE Thai_CI_AS NULL,
	[BUS_SCENARIO] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[COSTOBJECT] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[COSTCENTER] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ACTTYPE] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[PROFIT_CTR] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[PART_PRCTR] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[NETWORK] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[WBS_ELEMENT] [nvarchar](24) COLLATE Thai_CI_AS NULL,
	[ORDERID] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[ORDER_ITNO] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[ROUTING_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ACTIVITY] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[COND_TYPE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[COND_COUNT] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[COND_ST_NO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[FUND] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[FUNDS_CTR] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[CMMT_ITEM] [nvarchar](14) COLLATE Thai_CI_AS NULL,
	[CO_BUSPROC] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[ASSET_NO] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[SUB_NUMBER] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BILL_TYPE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SALES_ORD] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[S_ORD_ITEM] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[DISTR_CHAN] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[DIVISION] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[SALESORG] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SALES_GRP] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[SALES_OFF] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SOLD_TO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[DE_CRE_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[P_EL_PRCTR] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[XMFRW] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[QUANTITY] [decimal](7, 3) NULL,
	[BASE_UOM] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[BASE_UOM_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[INV_QTY] [decimal](7, 3) NULL,
	[INV_QTY_SU] [decimal](7, 3) NULL,
	[SALES_UNIT] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[SALES_UNIT_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[PO_PR_QNT] [decimal](7, 3) NULL,
	[PO_PR_UOM] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[PO_PR_UOM_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[ENTRY_QNT] [decimal](7, 3) NULL,
	[ENTRY_UOM] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[ENTRY_UOM_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[VOLUME] [decimal](8, 3) NULL,
	[VOLUMEUNIT] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[VOLUMEUNIT_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[GROSS_WT] [decimal](8, 3) NULL,
	[NET_WEIGHT] [decimal](8, 3) NULL,
	[UNIT_OF_WT] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[UNIT_OF_WT_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[ITEM_CAT] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[MATERIAL] [nvarchar](18) COLLATE Thai_CI_AS NULL,
	[MATL_TYPE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[MVT_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[REVAL_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[ORIG_GROUP] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[ORIG_MAT] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[SERIAL_NO] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[PART_ACCT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[TR_PART_BA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[TRADE_ID] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[VAL_AREA] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[VAL_TYPE] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[ASVAL_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[PO_NUMBER] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[PO_ITEM] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[ITM_NUMBER] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[COND_CATEGORY] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[FUNC_AREA_LONG] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[CMMT_ITEM_LONG] [nvarchar](24) COLLATE Thai_CI_AS NULL,
	[GRANT_NBR] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[CS_TRANS_T] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACGL09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACHE09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACHE09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[DOC_STATUS] [nvarchar](1) COLLATE Thai_CI_AS NULL CONSTRAINT [DF_BAPIACHE09_DOC_STATUS]  DEFAULT (N'N'),
	[DOC_TYPE_NAME] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[DOC_YEAR] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[REVERSE_DOC] [nchar](10) COLLATE Thai_CI_AS NULL,
	[FI_DOC] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[OBJ_TYPE] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[OBJ_KEY] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[OBJ_SYS] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[BUS_ACT] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[USERNAME] [nvarchar](12) COLLATE Thai_CI_AS NULL,
	[HEADER_TXT] [nvarchar](25) COLLATE Thai_CI_AS NULL,
	[COMP_CODE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[DOC_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[PSTNG_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[TRANS_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[FISC_YEAR] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[FIS_PERIOD] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[DOC_TYPE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[REF_DOC_NO] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[AC_DOC_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[OBJ_KEY_R] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[REASON_REV] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[COMPO_ACC] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[REF_DOC_NO_LONG] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[ACC_PRINCIPLE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[NEG_POSTNG] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[OBJ_KEY_INV] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACHE09_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACKEC9]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACKEC9](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[FIELDNAME] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[CHARACTERS] [nvarchar](18) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACKEC9] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACKEV9]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACKEV9](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[FIELDNAME] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[CURR_TYPE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[CURRENCY] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[CURRENCY_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[AMT_VALCOM] [decimal](12, 4) NULL,
	[BASE_UOM] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[BASE_UOM_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[QUA_VALCOM] [decimal](8, 3) NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACKEV9] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACPA09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACPA09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[NAME] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[NAME_2] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[NAME_3] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[NAME_4] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[POSTL_CODE] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CITY] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[COUNTRY] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[COUNTRY_ISO] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[STREET] [nvarchar](35) COLLATE Thai_CI_AS NULL,
	[PO_BOX] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[POBX_PCD] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[POBK_CURAC] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[BANK_ACCT] [nvarchar](18) COLLATE Thai_CI_AS NULL,
	[BANK_NO] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[BANK_CTRY] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[BANK_CTRY_ISO] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[TAX_NO_1] [nvarchar](16) COLLATE Thai_CI_AS NULL,
	[TAX_NO_2] [nvarchar](11) COLLATE Thai_CI_AS NULL,
	[TAX] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[EQUAL_TAX] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[REGION] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[CTRL_KEY] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[INSTR_KEY] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[DME_IND] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[LANGU_ISO] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[ANRED] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACPA09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACPC09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACPC09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CC_GLACCOUNT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CC_TYPE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[CC_NUMBER] [nvarchar](25) COLLATE Thai_CI_AS NULL,
	[CC_SEQ_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CC_VALID_F] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[CC_VALID_T] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[CC_NAME] [nvarchar](40) COLLATE Thai_CI_AS NULL,
	[DATAORIGIN] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[AUTHAMOUNT] [decimal](12, 4) NULL,
	[CURRENCY] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[CURRENCY_ISO] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[CC_AUTTH_NO] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[AUTH_REFNO] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[AUTH_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[AUTH_TIME] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[MERCHIDCL] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[POINT_OF_RECEIPT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[TERMINAL] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[CCTYP] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACPC09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACRE09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACRE09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[BUSINESS_ENTITY] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[BUILDING] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[PROPERTY] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[RENTAL_OBJECT] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[SERV_CHARGE_KEY] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SETTLEMENT_UNIT] [nvarchar](5) COLLATE Thai_CI_AS NULL,
	[CONTRACT_NO] [nvarchar](13) COLLATE Thai_CI_AS NULL,
	[FLOW_TYPE] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[CORR_ITEM] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[REF_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[OPTION_RATE] [decimal](5, 5) NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACRE09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIACTX09]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIACTX09](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[ITEMNO_ACC] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[GL_ACCOUNT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[COND_KEY] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[ACCT_KEY] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[TAX_CODE] [nvarchar](2) COLLATE Thai_CI_AS NULL,
	[TAX_RATE] [decimal](4, 3) NULL,
	[TAX_DATE] [nvarchar](8) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE_DEEP] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[TAXJURCODE_LEVEL] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[ITEMNO_TAX] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[DIRECT_TAX] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIACTX09] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIPAREX]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIPAREX](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[STRUCTURE] [nvarchar](30) COLLATE Thai_CI_AS NULL,
	[VALUEPART1] [nvarchar](240) COLLATE Thai_CI_AS NULL,
	[VALUEPART2] [nvarchar](240) COLLATE Thai_CI_AS NULL,
	[VALUEPART3] [nvarchar](240) COLLATE Thai_CI_AS NULL,
	[VALUEPART4] [nvarchar](240) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIPAREX] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIRET2]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIRET2](
	[ID1] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[TYPE] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[ID] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[NUMBER] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[MESSAGE] [nvarchar](220) COLLATE Thai_CI_AS NULL,
	[LOG_NO] [nvarchar](20) COLLATE Thai_CI_AS NULL,
	[LOG_MSG_NO] [nvarchar](6) COLLATE Thai_CI_AS NULL,
	[MESSAGE_V1] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[MESSAGE_V2] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[MESSAGE_V3] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[MESSAGE_V4] [nvarchar](50) COLLATE Thai_CI_AS NULL,
	[PARAMETER] [nvarchar](32) COLLATE Thai_CI_AS NULL,
	[ROW] [int] NULL,
	[FIELD] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[SYSTEM] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIRET2] PRIMARY KEY CLUSTERED 
(
	[ID1] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIRTAX1U15]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIRTAX1U15](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[WMWST] [decimal](7, 2) NULL,
	[MSATZ] [decimal](4, 3) NULL,
	[KTOSL] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[TXJCD] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[KNUMH] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[KBETR] [decimal](6, 2) NULL,
	[KAWRT] [decimal](8, 2) NULL,
	[HKONT] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[KSCHL] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[TXJCD_DEEP] [nvarchar](15) COLLATE Thai_CI_AS NULL,
	[TXJLV] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIRTAX1U15] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BAPIZACCKEY2]    Script Date: 03/26/2009 10:00:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BAPIZACCKEY2](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DOC_NO] [nvarchar](20) COLLATE Thai_CI_AS NOT NULL,
	[BUKRS] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BELNR] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[GJAHR] [nvarchar](4) COLLATE Thai_CI_AS NULL,
	[BUZEI] [nvarchar](3) COLLATE Thai_CI_AS NULL,
	[EFORMID] [nvarchar](10) COLLATE Thai_CI_AS NULL,
	[APP_TYPE] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[APPV_TYPE] [nvarchar](1) COLLATE Thai_CI_AS NULL,
	[Active] [bit] NOT NULL,
	[UpdBy] [bigint] NOT NULL,
	[UpdDate] [smalldatetime] NOT NULL,
	[CreBy] [bigint] NOT NULL,
	[CreDate] [smalldatetime] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[UpdPgm] [varchar](30) COLLATE Thai_CI_AS NOT NULL,
 CONSTRAINT [PK_BAPIZACCKEY2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF