/*
 * ER/Studio 8.0 SQL Code Generation
 * Company :      Vachira Phuket Hospital
 * Project :      PDFRequest.DM1
 * Author :       Developer
 *
 * Date Created : Saturday, March 21, 2009 15:19:10
 * Target DBMS : Microsoft SQL Server 2000
 */

/* 
 * TABLE: tbPDFRequest 
 */

CREATE TABLE tbPDFRequest(
    ID             uniqueidentifier    NOT NULL,
    Status         int                 NOT NULL,
    CreatedDate    datetime            NULL,
    LastUpdate     datetime            NULL,
    CONSTRAINT PK1 PRIMARY KEY NONCLUSTERED (ID)
)
go



IF OBJECT_ID('tbPDFRequest') IS NOT NULL
    PRINT '<<< CREATED TABLE tbPDFRequest >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE tbPDFRequest >>>'
go

