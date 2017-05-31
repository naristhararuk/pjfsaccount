using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.Query.NHibernate.Implement;
using SCG.eAccounting.DTO;
using SS.Standard.Data.NHibernate.QueryDao;
using NHibernate;
using SS.DB.Query;
using System.Net.Sockets;
using Spring.Transaction.Interceptor;

namespace SCG.eAccounting.Query.Hibernate
{
    public class FnEHRexpenseTempQuery : NHibernateQueryBase<FnehRexpenseTemp, long>, IFnEHRexpenseTempQuery
    {

        public void ClearTemporary()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("DELETE FROM FnEHRexpenseTemp where (status is null or status = '')");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();

            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = '' ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveID()
        {
            #region resolveID
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE fnehrexpensetemp ");
            sqlBuilder.Append(" SET ");
            sqlBuilder.Append(" fnehrexpensetemp.ReceiverUserID = receiver.UserID,");
            sqlBuilder.Append(" fnehrexpensetemp.CreateUserID =creater.UserID,");
            sqlBuilder.Append(" fnehrexpensetemp.ApproveUserID =approver.UserID,");
            sqlBuilder.Append(" fnehrexpensetemp.CostCenterID = cost.CostCenterID,");
            sqlBuilder.Append(" fnehrexpensetemp.AccountID = acc.AccountID,");
            sqlBuilder.Append(" fnehrexpensetemp.expenseComID = comp.companyID ");
            sqlBuilder.Append(" fnehrexpensetemp.IOID = io.IOID ");
            sqlBuilder.Append(" FROM fnehrexpensetemp ehr ");
            sqlBuilder.Append(" LEFT JOIN SuUser receiver");
            sqlBuilder.Append(" ON receiver.UserName = ehr.ReceiverUserCode and receiver.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN SuUser approver ");
            sqlBuilder.Append(" ON approver.UserName = ehr.ApproverUserCode and approver.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN SuUser creater ");
            sqlBuilder.Append(" ON creater.UserName = ehr.createUserCode and creater.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN DbCostCenter cost");
            sqlBuilder.Append(" ON cost.CostCenterCode = ehr.CostCenterCode and cost.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN DbAccount acc");
            sqlBuilder.Append(" ON acc.AccountCode = ehr.AccountCode and acc.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN DbCompany comp");
            sqlBuilder.Append(" ON comp.CompanyCode = ehr.ExpenseComCode and comp.Active = 1 ");
            sqlBuilder.Append(" LEFT JOIN DbInternalOrder io");
            sqlBuilder.Append(" ON io.IONumber = ehr.IONumber and io.Active = 1 ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region fail userid
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),'',temp.EHRExpenseID,'','Fail' ");
            sqlBuilder.AppendLine(",'Could not resolve userid.',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("WHERE createuserid is null or createuserid = ''  ");
            sqlBuilder.AppendLine("or ApproveUserID is null or ApproveUserID = ''  ");
            sqlBuilder.AppendLine("or ReceiverUserID is null or ReceiverUserID = ''  ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region delete user id null
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine("where createuserid is null or createuserid = '' ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();

            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine("where ApproveUserID is null or ApproveUserID = '' ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();

            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine("where ReceiverUserID is null or ReceiverUserID = '' ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

            #region fail company id
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),'',temp.EHRExpenseID,'','Fail' ");
            sqlBuilder.AppendLine(",'Could not resolve companyid.',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("WHERE expenseComID is null or expenseComID = ''  ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

            #region delete company id null
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine("where expenseComID is null or expenseComID = '' ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

            #region fail IOid
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),'',temp.EHRExpenseID,'','Fail' ");
            sqlBuilder.AppendLine(",'Could not resolve companyid.',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("WHERE (IOID is null or IOID = '') AND (ISNULL(IONumber,'') <> '') ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

            #region delete IOid null
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine(" (IOID is null or IOID = '') AND (ISNULL(IONumber,'') <> '') ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
        }



        public void ResolvePBSERVICEID()
        {
            #region resolve service team
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("UPDATE FnEHRexpenseTemp   ");
            sqlBuilder.AppendLine("SET   ");
            sqlBuilder.AppendLine("FnEHRexpenseTemp.PBID = pb.pbid,  ");
            sqlBuilder.AppendLine("FnEHRexpenseTemp.ServiceTeamID =  serviceteam.ServiceTeamID ");
            sqlBuilder.AppendLine("FROM FnEHRexpenseTemp temp   ");
            sqlBuilder.AppendLine("LEFT JOIN SuUser receiver   ");
            sqlBuilder.AppendLine("ON receiver.UserID = temp.ReceiverUserID   ");
            sqlBuilder.AppendLine("LEFT JOIN DBPB pb   ");
            sqlBuilder.AppendLine("ON pb.CompanyID = receiver.CompanyID and pb.Active = 1 ");
            sqlBuilder.AppendLine("LEFT JOIN DbServiceTeamLocation serviceteam    ");
            sqlBuilder.AppendLine("ON serviceteam.locationid = receiver.locationid and serviceteam.Active = 1 ");
            sqlBuilder.AppendLine("LEFT JOIN DbLocation loc   ");
            sqlBuilder.AppendLine("ON loc.LocationID = receiver.locationid and loc.Active = 1 ");
            sqlBuilder.AppendLine("WHERE loc.IsAllowImportExpense = 1 ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("tCOUNT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region fail
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),'',temp.EHRExpenseID,'','Fail' ");
            sqlBuilder.AppendLine(",'Could not find serviceteam or serviceteam location is not allow import expense.',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("where temp.ServiceTeamID IS NULL OR temp.ServiceTeamID = ''");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region delete
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp set status = 'F' ");
            sqlBuilder.AppendLine("FROM  ");
            sqlBuilder.AppendLine("FnEHRexpenseTemp  ");
            sqlBuilder.AppendLine("LEFT JOIN SuUser receiver ");
            sqlBuilder.AppendLine("ON receiver.UserID = ReceiverUserID  ");
            sqlBuilder.AppendLine("LEFT JOIN DBPB pb  ");
            sqlBuilder.AppendLine("ON pb.CompanyID = receiver.CompanyID  ");
            sqlBuilder.AppendLine("LEFT JOIN DbServiceTeamLocation serviceteam  ");
            sqlBuilder.AppendLine("ON serviceteam.locationid = receiver.locationid  ");
            sqlBuilder.AppendLine("LEFT JOIN DbLocation loc   ");
            sqlBuilder.AppendLine("ON loc.locationID = receiver.locationid ");
            sqlBuilder.AppendLine("WHERE  ");
            sqlBuilder.AppendLine("loc.IsAllowImportExpense IS NULL  ");
            sqlBuilder.AppendLine("OR loc.IsAllowImportExpense <> 1 ");
            sqlBuilder.AppendLine("OR FnEHRexpenseTemp.ServiceTeamID IS NULL ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("tCOUNT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

            #region Resolve duplicate
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),'',temp.EHRExpenseID,'','Fail' ");
            sqlBuilder.AppendLine(",'Duplicate EHR No.',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("where  exists  ");
            sqlBuilder.AppendLine("(select 1 from document where document.referenceno = temp.ehrexpenseid) ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region Delete duplicate
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("delete fnehrexpensetemp ");
            sqlBuilder.AppendLine("where  exists  ");
            sqlBuilder.AppendLine("(select 1 from document where document.referenceno = fnehrexpensetemp.ehrexpenseid) ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("tCOUNT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion

        }

        [Transaction]
        public void CommitNewExpense()
        {
            ISQLQuery query;
            StringBuilder sqlBuilder = new StringBuilder();

            #region create running
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO DbDocumentRunning( ");
            sqlBuilder.AppendLine("DocumentTypeID, ");
            sqlBuilder.AppendLine("CompanyID, ");
            sqlBuilder.AppendLine("Year, ");
            sqlBuilder.AppendLine("RunningNo, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT DISTINCT 9,expenseComid,year(getdate()),0,1,1,getdate(),1,getdate(),'AutoPayment'  ");
            sqlBuilder.AppendLine("FROM fnehrexpensetemp f  ");
            sqlBuilder.AppendLine("LEFT JOIN DbDocumentRunning d ON d.companyid = f.expenseComid AND d.documenttypeid = 9 AND d.year = year(getdate())  ");
            sqlBuilder.AppendLine("WHERE d.runningid is null and (f.status is null or f.status = '') ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion resolve doc number
            #region resolve documnet number
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update fnehrexpensetemp ");
            sqlBuilder.AppendLine("set documentno = t.docno ");
            sqlBuilder.AppendLine("from ");
            sqlBuilder.AppendLine("fnehrexpensetemp a  ");
            sqlBuilder.AppendLine("inner join  ");
            sqlBuilder.AppendLine("(select  ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("select documentNoPrefix from documenttype where documenttypeid = 9) + '-' + ");
            sqlBuilder.AppendLine("LTRIM(RTRIM(expensecomcode)) +  ");
            sqlBuilder.AppendLine("substring(LTRIM(RTRIM(str(year(getdate())))),3,2) + ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("	select REPLICATE('0', 5 - LEN(CAST(rowno AS varchar(5)))) + CAST(rowno AS varchar(5))  ");
            sqlBuilder.AppendLine("	from ");
            sqlBuilder.AppendLine("		 ( ");
            sqlBuilder.AppendLine("			select t1.ehrexpensetempid, row_number() over (order by t1.ehrexpensetempid) + ");
            sqlBuilder.AppendLine("			(select sum(runningno) from dbdocumentrunning where documenttypeid = 9 and companyid = t1.expensecomid and dbdocumentrunning.year = year(getdate())) ");
            sqlBuilder.AppendLine("			as rowno ");
            sqlBuilder.AppendLine("			from fnehrexpensetemp t1  ");
            //to do : where cluase with only normal case.
            sqlBuilder.AppendLine("			where t1.expensecomid = t2.expensecomid and (status is null or status = '')");
            sqlBuilder.AppendLine("		 ) t3 ");
            sqlBuilder.AppendLine("	where t3.ehrexpensetempid = t2.ehrexpensetempid ");
            sqlBuilder.AppendLine("    ");
            sqlBuilder.AppendLine(") as docno, t2.ehrexpensetempid ");
            sqlBuilder.AppendLine("from fnehrexpensetemp t2  ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("t on t.ehrexpensetempid = a.ehrexpensetempid ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region Success Log
            sqlBuilder.AppendLine("INSERT INTO SuEhrExpenseLog( ");
            sqlBuilder.AppendLine("LastDate, ");
            sqlBuilder.AppendLine("EHrExpenseID, ");
            sqlBuilder.AppendLine("ExpenseRequestNo, ");
            sqlBuilder.AppendLine("ReplaceID, ");
            sqlBuilder.AppendLine("Status, ");
            sqlBuilder.AppendLine("[Message], ");
            sqlBuilder.AppendLine("ExpenseDate, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT getdate(),temp.documentno,temp.ehrexpenseid,'','Success' ");
            sqlBuilder.AppendLine(",'Success',temp.ExpenseDate,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("LEFT JOIN [Document] Doc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentNo = temp.EHRExpenseID  ");
            sqlBuilder.AppendLine("LEFT JOIN FnExpenseDocument expDoc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentID = expDoc.DocumentID  ");
            sqlBuilder.AppendLine("WHERE ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("SELECT COUNT(1)  ");
            sqlBuilder.AppendLine("FROM [Document] d  ");
            sqlBuilder.AppendLine("WHERE d.DocumentNo = temp.ehrexpenseid  ");
            sqlBuilder.AppendLine("AND ACTIVE = 1  ");
            sqlBuilder.AppendLine(") = 0 AND (status is null or status = '')");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region update document running
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update dbDocumentRunning  ");
            sqlBuilder.AppendLine("set runningno = t.cnt + d.runningno ");
            sqlBuilder.AppendLine("from dbDocumentRunning  d ");
            sqlBuilder.AppendLine("inner join  ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("select count(ehrexpenseid) as cnt,expensecomid as comid ");
            sqlBuilder.AppendLine("from fnehrexpensetemp  where (status is null or status = '') ");
            sqlBuilder.AppendLine("group by expensecomid  ");
            sqlBuilder.AppendLine(") t on t.comid = d.companyid ");
            sqlBuilder.AppendLine("where d.year = year(getdate()) and d.documenttypeid = 9  ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region insert document
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into [document](Documentdate,CompanyID,CreatorID,ReceiverID,DocumentNo,DocumentTypeID,ApproverID,Subject,Active,CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Memo,RequesterID,ReferenceNo ) ");
            sqlBuilder.AppendLine("SELECT expensedate,expensecomid,createuserid,receiveruserid,documentno,3,approveuserid,invoicedescription,1,1,getdate(),1,getdate(),'EHRImport','',receiveruserid,t.EHRExpenseID ");
            sqlBuilder.AppendLine("FROM FnehrExpenseTemp t  ");
            sqlBuilder.AppendLine("WHERE   ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("SELECT COUNT(1)  ");
            sqlBuilder.AppendLine("FROM [Document] d ");
            sqlBuilder.AppendLine("WHERE d.DocumentNo = t.DocumentNo  ");
            sqlBuilder.AppendLine("AND ACTIVE = 1  ");
            sqlBuilder.AppendLine(") = 0 and (status is null or status = '') ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO FnExpenseDocument( ");
            sqlBuilder.AppendLine("DocumentID, ");
            sqlBuilder.AppendLine("ServiceTeamID, ");
            sqlBuilder.AppendLine("PBID, ");
            sqlBuilder.AppendLine("ExpenseType, ");
            sqlBuilder.AppendLine("PaymentType, ");
            sqlBuilder.AppendLine("TotalExpense, ");
            sqlBuilder.AppendLine("TotalAdvance, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate,		 ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm,DifferenceAmount ) ");
            sqlBuilder.AppendLine("SELECT doc.documentID, expense.ServiceTeamID, ");
            sqlBuilder.AppendLine("expense.PBID,'DM',expense.PaymentType,expense.TotalExpense,expense.TotalAdvance,  ");
            sqlBuilder.AppendLine("1,1,getdate(),1,getdate(),'Interface',expense.TotalExpense  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp expense  ");
            sqlBuilder.AppendLine("LEFT JOIN [Document] doc ");
            sqlBuilder.AppendLine("ON expense.DocumentNo = doc.DocumentNo   ");
            sqlBuilder.AppendLine("WHERE   ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("SELECT COUNT(1)  ");
            sqlBuilder.AppendLine("FROM FnExpenseDocument  ");
            sqlBuilder.AppendLine("WHERE FnExpenseDocument.DocumentID = doc.DocumentID   ");
            sqlBuilder.AppendLine("AND FnExpenseDocument.Active = 1  ");
            sqlBuilder.AppendLine(") = 0 AND (status is null or status = '') ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region insert workflow
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO workflow( ");
            sqlBuilder.AppendLine("DocumentID , ");
            sqlBuilder.AppendLine("WorkFlowTypeID , ");
            sqlBuilder.AppendLine("Description , ");
            sqlBuilder.AppendLine("CurrentState , ");
            sqlBuilder.AppendLine("Active , ");
            sqlBuilder.AppendLine("CreBy , ");
            sqlBuilder.AppendLine("CreDate , ");
            sqlBuilder.AppendLine("UpdBy , ");
            sqlBuilder.AppendLine("UpdDate , ");
            sqlBuilder.AppendLine("UpdPgm  ");
            sqlBuilder.AppendLine(")  ");
            sqlBuilder.AppendLine("SELECT doc.documentID,7,'',37,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnehrExpenseTemp expense  ");
            sqlBuilder.AppendLine("LEFT JOIN [Document] doc  ");
            sqlBuilder.AppendLine("ON expense.DocumentNo = doc.DocumentNo     ");
            sqlBuilder.AppendLine("WHERE ");
            sqlBuilder.AppendLine("(  ");
            sqlBuilder.AppendLine("SELECT COUNT(1)   ");
            sqlBuilder.AppendLine("FROM WorkFlow   ");
            sqlBuilder.AppendLine("WHERE DocumentID = doc.documentID   ");
            sqlBuilder.AppendLine("AND ACTIVE = 1   ");
            sqlBuilder.AppendLine(") = 0  AND (status is null or status = '')");

            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region insert expense invoice
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO FnExpenseInvoice( ");
            sqlBuilder.AppendLine("ExpenseID, ");
            sqlBuilder.AppendLine("InvoiceDocumentType, ");
            sqlBuilder.AppendLine("InvoiceDate, ");
            sqlBuilder.AppendLine("TotalAmount, ");
            sqlBuilder.AppendLine("NetAmount, ");
            sqlBuilder.AppendLine("Description, ");
            sqlBuilder.AppendLine("isVAT, ");
            sqlBuilder.AppendLine("isWHT, ");
            sqlBuilder.AppendLine("TotalBaseAmount, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT expDoc.ExpenseID,'G',temp.expenseDate, ");
            sqlBuilder.AppendLine("expDoc.TotalExpense,temp.InvoiceBaseAmount,temp.InvoiceBaseAmount, ");
            sqlBuilder.AppendLine("0,0,temp.InvoiceBaseAmount,1,1,getdate(),1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("LEFT JOIN [Document] Doc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentNo = temp.DocumentNo  ");
            sqlBuilder.AppendLine("LEFT JOIN FnExpenseDocument expDoc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentID = expDoc.DocumentID  ");
            sqlBuilder.AppendLine("WHERE   ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("SELECT COUNT(1)  ");
            sqlBuilder.AppendLine("FROM FnExpenseInvoice  ");
            sqlBuilder.AppendLine("WHERE FnExpenseInvoice.ExpenseID = expDoc.ExpenseID  ");
            sqlBuilder.AppendLine(") = 0 AND (status is null or status = '')");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region expense invoice item
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO FnExpenseInvoiceItem ( ");
            sqlBuilder.AppendLine("InvoiceID, ");
            sqlBuilder.AppendLine("CostCenterID, ");
            sqlBuilder.AppendLine("AccountID, ");
            sqlBuilder.AppendLine("Description, ");
            sqlBuilder.AppendLine("Amount, ");
            sqlBuilder.AppendLine("Active, ");
            sqlBuilder.AppendLine("CreBy, ");
            sqlBuilder.AppendLine("CreDate, ");
            sqlBuilder.AppendLine("UpdBy, ");
            sqlBuilder.AppendLine("UpdDate, ");
            sqlBuilder.AppendLine("UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("SELECT expInv.InvoiceID,temp.CostCenterID,temp.AccountID, ");
            sqlBuilder.AppendLine("temp.InvoiceDescription,temp.InvoiceBaseAmount,1,1,getdate(), ");
            sqlBuilder.AppendLine("1,getdate(),'Interface'  ");
            sqlBuilder.AppendLine("FROM FnEHRExpenseTemp temp  ");
            sqlBuilder.AppendLine("LEFT JOIN [Document] Doc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentNo = temp.DocumentNo  ");
            sqlBuilder.AppendLine("LEFT JOIN FnExpenseDocument expDoc  ");
            sqlBuilder.AppendLine("ON Doc.DocumentID = expDoc.DocumentID  ");
            sqlBuilder.AppendLine("LEFT JOIN FnExpenseInvoice expInv ");
            sqlBuilder.AppendLine("ON expDoc.ExpenseID = expInv.ExpenseID  ");
            sqlBuilder.AppendLine("WHERE  ");
            sqlBuilder.AppendLine("( ");
            sqlBuilder.AppendLine("SELECT COUNT(1)  ");
            sqlBuilder.AppendLine("FROM FnExpenseInvoiceItem ");
            sqlBuilder.AppendLine("WHERE FnExpenseInvoiceItem.InvoiceID = expInv.InvoiceID   ");
            sqlBuilder.AppendLine(") = 0 AND (status is null or status = '')");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("CNTT", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region refreshworkflow
            try
            {
                int port = ParameterServices.RefreshWorkFlowPermissionListernerPort;
                TcpClient client = new TcpClient("127.0.0.1", port);
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("scgrefreshpermission");
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                data = new Byte[256];
                // String to store the response ASCII representation.
                String responseData = String.Empty;
                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //Response.Write(string.Format("Received: {0}", responseData));
                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (Exception)
            {
                return;
            }
            #endregion

        }

        public void ImportExpense()
        {
            ISQLQuery query = GetCurrentSession().CreateSQLQuery("exec dbo.ImportExpense");
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }
    }
}
