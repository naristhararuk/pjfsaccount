using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.SU.DAL;
using Spring.Validation;
using SS.Standard.Utilities;
using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.DB.Query;
using System.Net.Sockets;

namespace SS.SU.BLL.Implement
{
    public class SuRoleServiceService :ServiceBase<SS.SU.DTO.SuRoleService,long>,  ISuRoleServiceService
    {

        public override IDao<SS.SU.DTO.SuRoleService, long> GetBaseDao()
        {
            return DaoProvider.SuRoleServiceDao;
        }

        #region ISuRoleServiceService Members

        public void AddRoleService(SS.SU.DTO.SuRoleService roleService)
        {
            roleService.CreDate = DateTime.Now.Date;
            roleService.UpdDate = DateTime.Now.Date;
            ValidationErrors errors = new ValidationErrors();
            if (roleService.ServiceTeamID.ServiceTeamID== null)
            {
                errors.AddError("RoleService.Error",new ErrorMessage("Requied ServiceID"));
            }
            if (roleService.RoleID.RoleID == null)
            {
                errors.AddError("RoleService.Error",new ErrorMessage("Requied RoleServiceID"));
            }
            if (DaoProvider.SuRoleServiceDao.IsExist(roleService))
            {
                errors.AddError("RoleService.Error", new ErrorMessage("Already Exist"));
            }

            if (!errors.IsEmpty)throw new ServiceValidationException(errors);
            DaoProvider.SuRoleServiceDao.Save(roleService);
            RefreshWorkflowPermission();



        }

        public void DeleteRoleService(SS.SU.DTO.SuRoleService roleService)
        {
            ValidationErrors errors = new ValidationErrors();
            if (roleService.RoleServiceID == null)
            {
                errors.AddError("RoleService.Error", new ErrorMessage("Requied RoleServiceID"));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            try
            {
                DaoProvider.SuRoleServiceDao.Delete(roleService);
            }
            catch (Exception)
            {
                roleService.Active = false;
                DaoProvider.SuRoleServiceDao.Update(roleService);
            }
            RefreshWorkflowPermission();

        }

        #endregion


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
