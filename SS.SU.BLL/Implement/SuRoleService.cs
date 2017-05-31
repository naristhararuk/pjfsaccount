using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;
using SS.Standard.Utilities;
using SS.SU.DTO;
using SS.SU.BLL;
using SS.SU.DAL;
using SS.SU.DTO.ValueObject;
using SS.DB.Query;
using System.Net.Sockets;


namespace SS.SU.BLL.Implement
{
    public partial class SuRoleService : ServiceBase<SuRole, short>, ISuRoleService
    {



        public override IDao<SuRole, short> GetBaseDao()
        {
            return DaoProvider.SuRoleDao;
        }

        public void AddRole(SuRole role)
        {
            role.CreDate = DateTime.Now.Date;
            role.UpdDate = DateTime.Now.Date;
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (string.IsNullOrEmpty(role.RoleName))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RequiredRoleName"));
            }
            if (string.IsNullOrEmpty(role.RoleCode))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RoleCodeIsRequired"));
            }
            if (DaoProvider.SuRoleDao.IsDuplicateRoleCode(role))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RoleCodeIsDuplicated"));
            }
            if (role.ApproveVerifyMinLimit > role.ApproveVerifyMaxLimit)
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("Approve Verify min limit must be less then the Max limit."));
            }
            if (role.VerifyMaxLimit < role.VerifyMinLimit)
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("Verify min limit must be less then the Max limit."));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.SuRoleDao.Save(role);
            RefreshWorkflowPermission();

        }

        public void UpdateRole(SuRole role)
        {
            role.UpdDate = DateTime.Now.Date;
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            #region validation
            if (string.IsNullOrEmpty(role.RoleName))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RequiredRoleName"));
            }
            if (string.IsNullOrEmpty(role.RoleCode))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RoleCodeIsRequired"));
            }
            if (DaoProvider.SuRoleDao.IsDuplicateRoleCode(role))
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("RoleCodeIsDuplicated"));
            }
            if (role.ApproveVerifyMinLimit > role.ApproveVerifyMaxLimit)
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("Approve Verify min limit must be less then the Max limit."));
            }
            if (role.VerifyMaxLimit < role.VerifyMinLimit)
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("Verify min limit must be less then the Max limit."));
            }
            #endregion
            //throw error to UI for handle.
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);
            DaoProvider.SuRoleDao.Update(role);
            RefreshWorkflowPermission();



        }

        public void DeleteRole(SuRole role)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();
            if (ParameterServices.DefaultUserRoleID == (int)role.RoleID)
            {
                errors.AddError("Role.Error", new Spring.Validation.ErrorMessage("Unable to remove user from default role."));
            }
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            Delete(role);
            RefreshWorkflowPermission();

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

