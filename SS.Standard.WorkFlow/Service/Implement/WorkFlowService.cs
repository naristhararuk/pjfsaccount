using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Spring.Context.Support;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.Standard.WorkFlow.DAL;
using SS.Standard.WorkFlow.Query;
using SS.Standard.WorkFlow.DTO;
using SS.Standard.WorkFlow.Service;
using SS.Standard.WorkFlow.DTO.ValueObject;
using SS.Standard.Utilities;
using SS.Standard.Security;
using log4net;


namespace SS.Standard.WorkFlow.Service.Implement
{
    public class WorkFlowService : ServiceBase<DTO.WorkFlow, long>, IWorkFlowService
    {
        private static ILog logger = log4net.LogManager.GetLogger(typeof(WorkFlowService));

        public IWorkFlowStateEventPermissionService WorkFlowStateEventPermissionService { get; set; }
        public IWorkFlowResponseTokenService WorkFlowResponseTokenService { get; set; }
        public IWorkFlowResponseService WorkFlowResponseService { get; set; }
        public IUserAccount UserAccount { get; set; }

        public override IDao<DTO.WorkFlow, long> GetBaseDao()
        {

            return WorkFlowDaoProvider.WorkFlowDao;
        }

        #region IWorkFlowService Members

        public bool CanCopyDocument(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("CanCopy{0}", stateName));
            bool canCopyEvent = false;
            if (methodInfo != null)
                canCopyEvent = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });

            return canCopyEvent;
        }

        public bool CanEditDocument(long workFlowID)
        {

            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("CanEdit{0}", stateName));
            bool canEditEvent = false;
            if (methodInfo != null)
                canEditEvent = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });

            return canEditEvent;
        }

        public bool CanDeleteDocument(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("CanDelete{0}", stateName));
            bool canDelete = false;
            if (methodInfo != null)
                canDelete = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });

            return canDelete;
        }

        public bool CanPrintPayInDocument(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("CanPrintPayIn{0}", stateName));
            bool canPrintPayInEvent = false;
            if (methodInfo != null)
                canPrintPayInEvent = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });

            return canPrintPayInEvent;
        }

        public void OnDeleteDocument(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            MethodInfo methodInfo = finder.GetType().GetMethod("OnDeleteDocument");
            if (methodInfo != null)
                methodInfo.Invoke(finder, new object[] { workFlowID });
        }

        public IList<AuthorizedEvent> RetrieveAuthorizedEvents(long workFlowID, short languageID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);


            int workFlowStateID = workFlow.CurrentState.WorkFlowStateID;
            IList<WorkFlowStateEventWithLang> workFlowStateEvents = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByWorkFlowStateEventByStateID(workFlowStateID, languageID);

            IList<AuthorizedEvent> authorizedEvents = new List<AuthorizedEvent>();
            MethodInfo methodInfo;
            bool canDoStateEvent;
            foreach (WorkFlowStateEventWithLang workFlowStateEventWithLang in workFlowStateEvents)
            {
                //Call method Can{EventName}{StateName} ex. CanSubmitDraft , CanApproveWaitAR ..
                methodInfo = finder.GetType().GetMethod(string.Format("Can{0}{1}", workFlowStateEventWithLang.Name, stateName));
                if (methodInfo != null)
                {
                    canDoStateEvent = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });
                    if (canDoStateEvent)
                    {
                        authorizedEvents.Add(new AuthorizedEvent(workFlowStateEventWithLang.EventID, workFlowStateEventWithLang.Name, workFlowStateEventWithLang.DisplayName, workFlowStateEventWithLang.UserControlPath));
                    }
                }
            }

            return authorizedEvents;
        }

        public void NotifyEvent(long workFlowID, string eventName, object eventData)
        {
            string stateName = string.Empty;
            try
            {
                DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentityWithUpdateLock(workFlowID);
                WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

                string serviceName = workFlowTypeDocumentType.ServiceName;
                stateName = workFlow.CurrentState.Name;
                object finder = ContextRegistry.GetContext().GetObject(serviceName);

                //Call method Can{EventName}{StateName} ex. CanSubmitDraft , CanApproveWaitAR ..
                MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("Can{0}{1}", eventName, stateName));
                if (methodInfo != null)
                {
                    bool canDoStateEvent = (bool)methodInfo.Invoke(finder, new object[] { workFlowID });
                    if (canDoStateEvent)
                    {
                        //Call method On{EventName}{StateName} ex. OnSubmitDraft , OnApproveWaitAR ..
                        methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", eventName, stateName));
                        string signal = (string)methodInfo.Invoke(finder, new object[] { workFlowID, eventData });

                        //Call method OnExit{StateName}
                        methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", "Exit", stateName));
                        if (methodInfo != null)
                            methodInfo.Invoke(finder, new object[] { workFlowID });

                        //Get NextState in Transition
                        WorkFlowState nextState = WorkFlowQueryProvider.WorkFlowStateTransitionQuery.GetNextState(workFlow.CurrentState.WorkFlowStateID, signal);
                        if (nextState != null)
                        {

                            if (workFlow.CurrentState.Ordinal > nextState.Ordinal)
                            {
                                this.ResetActiveResponse(workFlowID, nextState.Ordinal);
                            }

                            this.UpdateState(workFlowID, nextState);

                            this.ReCalculateWorkFlowPermission(workFlowID);

                            //Call method OnEnter{NextStateName}
                            methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", "Enter", nextState.Name));
                            if (methodInfo != null)
                                methodInfo.Invoke(finder, new object[] { workFlowID });
                        }
                    }
                    else
                    {
                        Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                        errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_DoesnotHavePermission_On_Document")); //ไม่มีสิทธิใช้เอกสารฉบับนี้
                        throw new ServiceValidationException(errors);
                    }
                }
                else
                {
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_Invalid_Document_State")); //สถานะของเอกสารไม่ถูกต้อง เอกสารนี้อาจกำลังถูกปรับปรุงโดยผู้ใช้งานท่านอื่น
                    throw new ServiceValidationException(errors);
                }
            }
            catch (ServiceValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType().Equals(typeof(ServiceValidationException)))
                    throw ex;
                else
                {
                    if (logger != null)
                    {
                        logger.Error(string.Format("WorkflowID: {0}, EventName: {1}, StateName: {2}", workFlowID, eventName, stateName));
                        logger.Error(string.Format("WorkFlowService.NotifyEvent(workflowID) : {0}", workFlowID));
                        logger.Error(string.Format("Error Message StackTrace: {0}", ex.StackTrace));
                    }
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors(); 
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_Problem_Occurred")); //ระบบมีปัญหาในการประมวลผล กรุณาลองอีกครั้งหนึ่ง
                    throw new ServiceValidationException(errors);
                }
            }
        }

        public void NotifyEventFromInternal(long workFlowID, string eventName, object eventData)
        {
            string stateName = string.Empty;
            try
            {
                DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
                WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

                string serviceName = workFlowTypeDocumentType.ServiceName;
                stateName = workFlow.CurrentState.Name;
                object finder = ContextRegistry.GetContext().GetObject(serviceName);

                //Call method On{EventName}{StateName} ex. OnSubmitDraft , OnApproveWaitAR ..
                MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", eventName, stateName));
                if (methodInfo != null)
                {
                    string signal = (string)methodInfo.Invoke(finder, new object[] { workFlowID, eventData });

                    //Call method OnExit{StateName}
                    methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", "Exit", stateName));
                    if (methodInfo != null)
                        methodInfo.Invoke(finder, new object[] { workFlowID });

                    //Get NextState in Transition
                    WorkFlowState nextState = WorkFlowQueryProvider.WorkFlowStateTransitionQuery.GetNextState(workFlow.CurrentState.WorkFlowStateID, signal);
                    if (nextState != null)
                    {

                        if (workFlow.CurrentState.Ordinal > nextState.Ordinal)
                        {
                            this.ResetActiveResponse(workFlowID, nextState.Ordinal);
                        }

                        this.UpdateState(workFlowID, nextState);

                        this.ReCalculateWorkFlowPermission(workFlowID);

                        //Call method OnEnter{NextStateName}
                        methodInfo = finder.GetType().GetMethod(string.Format("On{0}{1}", "Enter", nextState.Name));
                        if (methodInfo != null)
                            methodInfo.Invoke(finder, new object[] { workFlowID });
                    }
                }
                else
                {
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_Invalid_Document_State")); //สถานะของเอกสารไม่ถูกต้อง เอกสารนี้อาจกำลังถูกปรับปรุงโดยผู้ใช้งานท่านอื่น
                    throw new ServiceValidationException(errors);
                }
            }
            catch (ServiceValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType().Equals(typeof(ServiceValidationException)))
                    throw ex;
                else
                {
                    if (logger != null)
                    {
                        logger.Error(string.Format("WorkflowID: {0}, EventName: {1}, StateName: {2}", workFlowID, eventName, stateName));
                        logger.Error(string.Format("WorkFlowService.NotifyEventFromInternal(workflowID) : {0}", workFlowID, ex.StackTrace));
                    }
                    Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
                    errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlow_Error_Problem_Occurred")); //ระบบมีปัญหาในการประมวลผล กรุณาลองอีกครั้งหนึ่ง
                    throw new ServiceValidationException(errors);
                }
            }
        }

        public void UpdateState(long workFlowID, DTO.WorkFlowState newState)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            workFlow.CurrentState = newState;
            workFlow.UpdBy = UserAccount.UserID;
            workFlow.UpdDate = DateTime.Now;
            workFlow.UpdPgm = UserAccount.CurrentProgramCode;
            this.Update(workFlow);
        }

        public long CheckExistAndAddNew(SS.Standard.WorkFlow.DTO.WorkFlow domain)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (domain.Document == null)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("DocumentRequired"));
            }
            if (domain.WorkFlowType == null)
            {
                errors.AddError("WorkFlow.Error", new Spring.Validation.ErrorMessage("WorkFlowTypeRequired"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            WorkFlow.DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.GetWorkFlowByDocumentID(domain.Document.DocumentID);
            if (workFlow == null)
            {
                domain.Active = true;
                domain.CreBy = UserAccount.UserID;
                domain.CreDate = DateTime.Now;
                domain.UpdBy = UserAccount.UserID;
                domain.UpdDate = DateTime.Now;
                domain.UpdPgm = UserAccount.CurrentProgramCode;
                long workFlowID = WorkFlowDaoProvider.WorkFlowDao.Save(domain);

                return workFlowID;
            }
            else
            {
                return workFlow.WorkFlowID;
            }
        }

        private void ResetActiveResponse(long workFlowID, int nextOrdinal)
        {
            #region Old Code
            //IList<WorkFlowResponse> workFlowResponses = WorkFlowQueryProvider.WorkFlowResponseQuery.FindByWorkFlowID(workFlowID);
            //foreach (WorkFlowResponse response in workFlowResponses)
            //{
            //    if (response.WorkFlowStateEvent.WorkFlowState.Ordinal >= nextOrdinal)
            //    {
            //        response.Active = false;
            //        WorkFlowResponseService.Update(response);
            //    }
            //}
            #endregion
            // comment by meaw 09/09/2009
            WorkFlowResponseService.ResetActiveResponse(workFlowID, nextOrdinal);
        }

        #endregion

        #region IWorkFlowService Members (GetEditableFields)
        public IList<object> GetEditableFields(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            int stateID = workFlow.CurrentState.WorkFlowStateID;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            IList<object> editableFileds = new List<object>();
            MethodInfo methodInfo;

            methodInfo = finder.GetType().GetMethod(string.Format("GetEditableFieldsFor{0}", stateName));
            if (methodInfo != null)
            {
                editableFileds = (IList<object>)methodInfo.Invoke(finder, new object[] { workFlowID });
            }

            return editableFileds;
        }
        #endregion

        #region IWorkFlowService Members (GetVisibleFields)
        public IList<object> GetVisibleFields(long workFlowID)
        {
            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;
            int stateID = workFlow.CurrentState.WorkFlowStateID;
            object finder = ContextRegistry.GetContext().GetObject(serviceName);

            IList<object> visibleFileds = new List<object>();
            MethodInfo methodInfo;

            methodInfo = finder.GetType().GetMethod(string.Format("GetVisibleFieldsFor{0}", stateName));
            if (methodInfo != null)
            {
                visibleFileds = (IList<object>)methodInfo.Invoke(finder, new object[] { workFlowID });
            }

            return visibleFileds;
        }
        #endregion




        #region IWorkFlowService Members (ReCalculateWorkFlowPermission)


        public void ReCalculateWorkFlowPermission(long workFlowID)
        {
            WorkFlowStateEventPermissionService.ClearOldPermission(workFlowID);

            DTO.WorkFlow workFlow = WorkFlowQueryProvider.WorkFlowQuery.FindByIdentity(workFlowID);
            if (workFlow == null) return;
            if (workFlow.CurrentState == null) return;
            if (workFlow.WorkFlowType == null) return;
            if (workFlow.Document == null) return;
            if (workFlow.Document.DocumentType == null) return;

            WorkFlowTypeDocumentType workFlowTypeDocumentType = WorkFlowQueryProvider.WorkFlowTypeDocumentTypeQuery.GetByWorkFlowTypeID_DocumentTypeID(workFlow.WorkFlowType.WorkFlowTypeID, workFlow.Document.DocumentType.DocumentTypeID);
            if (workFlowTypeDocumentType == null) return;

            string serviceName = workFlowTypeDocumentType.ServiceName;
            string stateName = workFlow.CurrentState.Name;

            object finder = ContextRegistry.GetContext().GetObject(serviceName);
            if (finder == null) return;

            IList<WorkFlowStateEvent> stateEvents = WorkFlowQueryProvider.WorkFlowStateEventQuery.FindByWorkFlowStateEventByStateID(workFlow.CurrentState.WorkFlowStateID);

            foreach (WorkFlowStateEvent stateEvent in stateEvents)
            {
                MethodInfo methodInfo = finder.GetType().GetMethod(string.Format("ReCalculatePermissionFor{0}{1}", stateEvent.Name, stateName));
                if (methodInfo != null)
                    methodInfo.Invoke(finder, new object[] { workFlowID, stateEvent.WorkFlowStateEventID });
            }
        }

        public void ReCalculateWorkFlowPermission(short roleID)
        {
            throw new NotImplementedException();
        }

        public void ReCalculateWorkFlowPermission()
        {
            // Query document that its status does not 'Complete' or 'Cancel'
            IList<DTO.WorkFlow> workFlows = WorkFlowQueryProvider.WorkFlowQuery.GetAllActiveWorkFlow();
            foreach (DTO.WorkFlow workFlow in workFlows)
            {
                ReCalculateWorkFlowPermission(workFlow.WorkFlowID);
            }
        }

        #endregion

        #region NotifyEventFromSMS , Email


        public void NotifyEventFromToken(string tokenCode, long userID, int workFlowStateEventID, TokenType tokenType)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            WorkFlowResponseToken responseToken = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.GetByTokenCode_WorkFlowStateEventID(tokenCode, workFlowStateEventID);

            if (responseToken == null)
                errors.AddError("NotifyEventFromToken", new Spring.Validation.ErrorMessage("NotifyEventFromToken_Mismatch_ResponseToken"));
            else if (userID != responseToken.UserID)
                errors.AddError("NotifyEventFromToken", new Spring.Validation.ErrorMessage("NotifyEventFromToken_Mismatch_UserID"));

            if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

            SubmitResponse eventData = new SubmitResponse(workFlowStateEventID);
            if (tokenType == TokenType.Email)
            {
                eventData.ResponseMethod = ResponseMethod.Email;
            }
            else if (tokenType == TokenType.SMS)
            {
                eventData.ResponseMethod = ResponseMethod.SMS;
            }
            NotifyEvent(responseToken.WorkFlow.WorkFlowID, responseToken.WorkFlowStateEvent.Name, eventData);

            WorkFlowResponseTokenService.ClearResponseToken(tokenCode);
        }
        public void NotifyEventFromSMSToken(string tokenCode, long userID, int workFlowStateEventID, TokenType tokenType)
        {
            try
            {
                Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

                WorkFlowResponseToken responseToken = WorkFlowQueryProvider.WorkFlowResponseTokenQuery.GetByTokenCode_WorkFlowStateEventID(tokenCode, workFlowStateEventID);

                if (responseToken == null)
                    errors.AddError("NotifyEventFromToken", new Spring.Validation.ErrorMessage("NotifyEventFromToken_Mismatch_ResponseToken"));
                else if (userID != responseToken.UserID)
                    errors.AddError("NotifyEventFromToken", new Spring.Validation.ErrorMessage("NotifyEventFromToken_Mismatch_UserID"));

                if (!errors.IsEmpty) throw new SS.Standard.Utilities.ServiceValidationException(errors);

                SubmitResponse eventData = new SubmitResponse(workFlowStateEventID);

                eventData.ResponseMethod = ResponseMethod.SMS;

                NotifyEvent(responseToken.WorkFlow.WorkFlowID, responseToken.WorkFlowStateEvent.Name, eventData);

                WorkFlowResponseTokenService.ClearResponseToken(tokenCode);
            }
            catch (ServiceValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                if (ex != null && ex.InnerException != null)
                {
                    throw ex;
                }
                else
                {
                    throw new Exception("WorkFlowHasChanged");
                }
            }

        }
        #endregion
    }
}
