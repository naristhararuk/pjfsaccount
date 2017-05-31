using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.SU.DTO;
using SS.SU.DAL;
using SS.Standard.Utilities;
using SS.DB.Query;
using System.Net.Sockets;



namespace SS.SU.BLL.Implement
{
    public partial class SuRolepbService : ServiceBase<SuRolepb,long>,ISuRolepbService
    {

        #region ISuRolepbService Members
        
        public void AddRolepb(SuRolepb suRolepb)
        {
            suRolepb.CreDate = DateTime.Now.Date;
            suRolepb.UpdDate = DateTime.Now.Date;
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (suRolepb.PBID.Pbid == null)
            {
                errors.AddError("SuRolepb.Error",new Spring.Validation.ErrorMessage("RequiredPBID"));
            }
            if(suRolepb.RoleID.RoleID == null){
                errors.AddError("SuRolepb.Error", new Spring.Validation.ErrorMessage("RequiredRoleID"));
            }
            if (DaoProvider.SuRolepbDao.IsExist(suRolepb.RoleID.RoleID, suRolepb.PBID.Pbid))
            {
                errors.AddError("SuRolepb.Error", new Spring.Validation.ErrorMessage("Already exist"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            
            DaoProvider.SuRolepbDao.Save(suRolepb);
            RefreshWorkflowPermission();
        }

        public void DeleteRolepb(SuRolepb suRolepb)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            
            if(suRolepb.RolePBID == null){
                errors.AddError("SuRolepb.Error", new Spring.Validation.ErrorMessage("RequiedRolePBID"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            try
            {
                DaoProvider.SuRolepbDao.Delete(suRolepb);
            }
            catch (Exception)
            {
                suRolepb.Active = false;
                DaoProvider.SuRolepbDao.Update(suRolepb);
            }
            RefreshWorkflowPermission();
        }

        #endregion

        public override IDao<SuRolepb, long> GetBaseDao()
        {
            return DaoProvider.SuRolepbDao;
        }

        private void RefreshWorkflowPermission()
        {
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
        }
    }
}
