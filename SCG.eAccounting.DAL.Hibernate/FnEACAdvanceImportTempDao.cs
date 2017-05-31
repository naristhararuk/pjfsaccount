using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Standard.Data.NHibernate.Dao;
using SCG.eAccounting.DTO;
using NHibernate;

namespace SCG.eAccounting.DAL.Hibernate
{
    public partial class FnEACAdvanceImportTempDao : NHibernateDaoBase<FneacAdvanceImportTemp, long>, IFnEACAdvanceImportTempDao
    {
        public void ClearTempData()
        {

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("delete FnEACAdvanceImportTemp ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveRequesterID()
        {

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp  ");
            sqlBuilder.AppendLine("set requesteruserid = ( ");
            sqlBuilder.AppendLine("select userid from suuser where username = FnEACAdvanceImportTemp.requesterusercode and active = 1");
            sqlBuilder.AppendLine(") ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            RequesterLog();


        }

        private void RequesterLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("Message,CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,'','Fail','User Id not found ('+requesterusercode+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("where requesteruserid is null ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveReceiver()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp  ");
            sqlBuilder.AppendLine("set receiveruserid = ( ");
            sqlBuilder.AppendLine("select userid from suuser where username = FnEACAdvanceImportTemp.requesterusercode and active = 1");
            sqlBuilder.AppendLine(") ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            ReceiverLog();
        }

        private void ReceiverLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,'','Fail','User Id not found ('+requesterusercode+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("where requesteruserid is null ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveApproverID()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp  ");
            sqlBuilder.AppendLine("set ApproveUserID = ( ");
            sqlBuilder.AppendLine("select userid from suuser where username = FnEACAdvanceImportTemp.ApproveUserCode and active = 1");
            sqlBuilder.AppendLine(") ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            ApproverLog();
        }

        private void ApproverLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,'','Fail','User Id not found ('+ApproveUserCode+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("where ApproveUserID is null ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveCompany()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("set ExpenseComID = ( ");
            sqlBuilder.AppendLine("select companyid from dbcompany where companycode = FnEACAdvanceImportTemp.ExpenseComCode ");
            sqlBuilder.AppendLine(") ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            CompanyLog();
        }

        private void CompanyLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,'','Fail','Expense company id not found ('+ExpenseComCode+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("where ExpenseComID  is null ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveExpenseNo()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select eac.ExpenseNo,'','Fail','Duplicate Expense No ('+eac.ExpenseNo+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp eac ");
            sqlBuilder.AppendLine("where 0 <> (select count(*) from   ");
            sqlBuilder.AppendLine("document dm where  ");
            sqlBuilder.AppendLine("dm.referenceNo = eac.ExpenseNo and active = 1) ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void DeleteFailFromLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("delete FnEACAdvanceImportTemp where  ");
            sqlBuilder.AppendLine("ExpenseNo in (select EACRequestNo from FnEACAdvanceImportLog where active  = 1) ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void ResolveDocumentNo()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO DbDocumentRunning(  ");
            sqlBuilder.AppendLine("            DocumentTypeID,  ");
            sqlBuilder.AppendLine("            CompanyID,  ");
            sqlBuilder.AppendLine("            Year,  ");
            sqlBuilder.AppendLine("            RunningNo,  ");
            sqlBuilder.AppendLine("            Active,  ");
            sqlBuilder.AppendLine("            CreBy,  ");
            sqlBuilder.AppendLine("            CreDate,  ");
            sqlBuilder.AppendLine("            UpdBy,  ");
            sqlBuilder.AppendLine("            UpdDate,  ");
            sqlBuilder.AppendLine("            UpdPgm  ");
            sqlBuilder.AppendLine("            )  ");
            sqlBuilder.AppendLine("            SELECT DISTINCT 1,expenseComid,year(getdate()),0,1,1,getdate(),1,getdate(),'ImportAdvance'   ");
            sqlBuilder.AppendLine("            FROM FnEACAdvanceImportTemp f   ");
            sqlBuilder.AppendLine("            LEFT JOIN DbDocumentRunning d ON d.companyid = f.expenseComid AND d.documenttypeid = 1 AND d.year = year(getdate())   ");
            sqlBuilder.AppendLine("            WHERE d.runningid is null  ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            ResolveRunning();
        }

        private void ResolveRunning()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp  ");
            sqlBuilder.AppendLine("set EXPdocumentNo = t.docno ");
            sqlBuilder.AppendLine("            from  ");
            sqlBuilder.AppendLine("            FnEACAdvanceImportTemp a   ");
            sqlBuilder.AppendLine("            inner join   ");
            sqlBuilder.AppendLine("            (select   ");
            sqlBuilder.AppendLine("            (  ");
            sqlBuilder.AppendLine("            select documentNoPrefix from documenttype where documenttypeid = 1) + '-' +  ");
            sqlBuilder.AppendLine("            LTRIM(RTRIM(ExpenseComCode)) +   ");
            sqlBuilder.AppendLine("            substring(LTRIM(RTRIM(str(year(getdate())))),3,2) +  ");
            sqlBuilder.AppendLine("            (  ");
            sqlBuilder.AppendLine("            	select REPLICATE('0', 5 - LEN(CAST(rowno AS varchar(5)))) + CAST(rowno AS varchar(5))   ");
            sqlBuilder.AppendLine("            	from  ");
            sqlBuilder.AppendLine("            		 (  ");
            sqlBuilder.AppendLine("            			select t1.tempid, row_number() over (order by t1.tempid) +  ");
            sqlBuilder.AppendLine("            			(select sum(runningno) from dbdocumentrunning where documenttypeid = 1 and companyid = t1.expensecomid and dbdocumentrunning.year = year(getdate()))  ");
            sqlBuilder.AppendLine("            			as rowno  ");
            sqlBuilder.AppendLine("            			from FnEACAdvanceImportTemp t1   ");
            sqlBuilder.AppendLine("            			where t1.expensecomid = t2.expensecomid  ");
            sqlBuilder.AppendLine("            		 ) t3  ");
            sqlBuilder.AppendLine("            	where t3.tempid = t2.tempid  ");
            sqlBuilder.AppendLine("            ) as docno, t2.tempid  ");
            sqlBuilder.AppendLine("            from FnEACAdvanceImportTemp t2   ");
            sqlBuilder.AppendLine("            )  ");
            sqlBuilder.AppendLine("            t on t.tempid = a.tempid  ");
            sqlBuilder.AppendLine("update dbDocumentRunning  ");
            sqlBuilder.AppendLine("            set runningno = t.cnt + d.runningno  ");
            sqlBuilder.AppendLine("            from dbDocumentRunning  d  ");
            sqlBuilder.AppendLine("            inner join   ");
            sqlBuilder.AppendLine("            (  ");
            sqlBuilder.AppendLine("            select count(tempid) as cnt,expensecomid as comid  ");
            sqlBuilder.AppendLine("            from FnEACAdvanceImportTemp   ");
            sqlBuilder.AppendLine("            group by expensecomid   ");
            sqlBuilder.AppendLine("            ) t on t.comid = d.companyid  ");
            sqlBuilder.AppendLine("            where d.year = year(getdate()) and d.documenttypeid = 1   ");

            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }

        public void SaveToDataBase()
        {
            #region SaveToDocument
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into document( ");
            sqlBuilder.AppendLine("CompanyID,RequesterID,CreatorID,ReceiverID, ");
            sqlBuilder.AppendLine("DocumentDate,DocumentNo,DocumentTypeID,ApproverID, ");
            sqlBuilder.AppendLine("PostingStatus,Active,CreBy,CreDate,UpdBy,UpdDate,UpdPgm, ");
            sqlBuilder.AppendLine("ReferenceNo,subject,memo ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select expenseComID,RequesterUserID,RequesterUserID,ReceiverUserID, ");
            sqlBuilder.AppendLine("convert(datetime,p1.parametervalue,103), ");
            sqlBuilder.AppendLine("expdocumentno,1,ApproveUserID,'C',1, ");
            sqlBuilder.AppendLine("p2.parametervalue ");
            sqlBuilder.AppendLine(",getdate(), ");
            sqlBuilder.AppendLine("p2.parametervalue, ");
            sqlBuilder.AppendLine("getdate(),'ImportAdvance',ExpenseNo,subject,'' ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp,dbparameter p1,dbparameter p2 ");
            sqlBuilder.AppendLine("where p1.configurationname = 'AdvanceImportDocumentDate' and p2.configurationname = 'SystemUserID' ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region SaveToAdvance
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into avadvancedocument( ");
            sqlBuilder.AppendLine("DocumentID,ServiceTeamID,RequestDateOfAdvance,DueDateOfRemittance,RequestDateOfRemittance,AdvanceType, ");
            sqlBuilder.AppendLine("Amount,RemittanceAmount,ExpenseAmount,Active,CreBy,CreDate,UpdBy,UpdDate,UpdPgm,reason,PBID ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select t2.documentid,  ");
            sqlBuilder.AppendLine("(select sum(serviceteamid) from dbserviceteam where serviceteamcode =  ");
            sqlBuilder.AppendLine("(select parametervalue from dbparameter where configurationname = 'AdvanceImportServiceTeamCode') ");
            sqlBuilder.AppendLine(")  ");
            sqlBuilder.AppendLine(",t1.expensecreatedate,t1.clearadvanceduedate, ");
            sqlBuilder.AppendLine("t1.clearadvancedate,'DM',t1.netamount,0,0,1,1,getdate(),1,getdate(),'ImportAdvance','',t1.PBID ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp  t1  ");
            sqlBuilder.AppendLine("inner join [document] t2 on t2.documentno = t1.EXPdocumentNo  ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region SaveToAdvanceItem
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into avadvanceitem( ");
            sqlBuilder.AppendLine("AdvanceID,PaymentType,CurrencyID,Amount,ExchangeRate,AmountTHB,Active,CreBy,CreDate, ");
            sqlBuilder.AppendLine("UpdBy,UpdDate,UpdPgm ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select t3.advanceid,t1.paymenttype,(select currencyid from dbcurrency where symbol = 'THB'),t1.netamount,'1',t1.netamount,1,1,getdate(),1,getdate(),'ImportAdvance' ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp t1 ");
            sqlBuilder.AppendLine("inner join [document] t2 on t1.expdocumentno = t2.documentno  ");
            sqlBuilder.AppendLine("inner join avAdvanceDocument t3 on t2.documentid = t3.documentid ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
            #region SaveToWorkFlow
            sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into workflow( ");
            sqlBuilder.AppendLine("DocumentID,WorkFlowTypeID,Description,CurrentState,Active, ");
            sqlBuilder.AppendLine("CreBy,CreDate,UpdBy,UpdDate,UpdPgm) ");
            sqlBuilder.AppendLine("select t2.documentid,1,'',25,1,1,getdate(),1,getdate(),'ImportAdvance' ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp t1 ");
            sqlBuilder.AppendLine("inner join [document] t2 on t1.expdocumentno = t2.documentno  ");
            query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            #endregion
        }

        public void SaveSuccessToLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,EXPDocumentNo,'Success','Success', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();

        }

        public void ResolvePBCode()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("update FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("set pbid = ( ");
            sqlBuilder.AppendLine("select pbid from dbpb where pbcode = FnEACAdvanceImportTemp.pbcode ");
            sqlBuilder.AppendLine(") ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
            PBCodeLog();
        }

        private void PBCodeLog()
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("insert into FnEACAdvanceImportLog(EACRequestNo,EXPRequestNo,Status, ");
            sqlBuilder.AppendLine("[Message],CreBy,CreDate,UpdBy,UpdDate,UpdPgm,Active ");
            sqlBuilder.AppendLine(") ");
            sqlBuilder.AppendLine("select ExpenseNo,'','Fail','Expense pb Id not found ('+pbcode+')', ");
            sqlBuilder.AppendLine("1,getdate(),1,getdate(),'InterfaceImportAdvance',1 ");
            sqlBuilder.AppendLine("from FnEACAdvanceImportTemp ");
            sqlBuilder.AppendLine("where pbid  is null and pbcode <> '' ");
            ISQLQuery query = GetCurrentSession().CreateSQLQuery(sqlBuilder.ToString());
            query.AddScalar("Count", NHibernateUtil.Int32);
            query.UniqueResult();
        }
    }
}
