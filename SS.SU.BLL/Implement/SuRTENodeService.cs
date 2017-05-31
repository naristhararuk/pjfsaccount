using System;
using System.Web;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SS.Standard.Data.NHibernate.Dao;
using SS.Standard.Data.NHibernate.Service.Implement;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;
using SS.SU.DAL;
using SS.SU.BLL;
using SS.Standard.Utilities;

namespace SS.SU.BLL.Implement
{
    public class SuRTENodeService : ServiceBase<SuRTENode, short>, ISuRTENodeService
    {

        #region Constant
        private const int maxFileSize = 51200;
        private const int maxImageWidth = 30;
        private const int maxImageHeight = 30;
        #endregion

        #region Override Method
        public override IDao<SuRTENode, short> GetBaseDao()
        {
            return DaoProvider.SuRTENodeDao;
        }
        #endregion
        #region ISuRTENodeService Members

        public IList<SuRTENodeSearchResult> GetRTENodeList(short languageId, string nodetype)
        {
            return DaoProvider.SuRTENodeDao.GetRTENodeList(languageId, nodetype);
        }
        public IList<SuRTENodeSearchResult> GetRTEContentList(short languageId, string nodetype,short nodeid)
        {
            return DaoProvider.SuRTENodeDao.GetRTEContentList(languageId, nodetype, nodeid);
        }
        public IList<SuRTENodeSearchResult> GetRTEContent(short languageId, string nodetype, short nodeid)
        {
            return DaoProvider.SuRTENodeDao.GetRTEContent(languageId, nodetype, nodeid);
        }
        public SuRTENodeSearchResult GetWelcome(short languageId, string nodeType)
        {
            return DaoProvider.SuRTENodeDao.GetWelcome(languageId, nodeType);
        }
        public short AddNode(SuRTENode node)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (node.NodeHeaderid == null)
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("RequeiredNodeHeaderId"));
            }
            
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            return DaoProvider.SuRTENodeDao.Save(node);
        }
        public short AddNode(SuRTENode node, HttpPostedFile imageFile)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (node.NodeHeaderid == null)
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("RequeiredNodeHeaderId"));
            }
            // Check file Type of imageFile.
            if ((!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) && (!imageFile.ContentType.Equals("image/jpg")) && (!imageFile.ContentType.Equals("image/x-png")))
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (imageFile.ContentLength > maxFileSize)
            {
                // File size exceed 50KB.
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            //// Check file Dimension of imageFile
            //Image uploadImage = Image.FromStream(imageFile.InputStream);
            //if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            //{
            //    errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            return DaoProvider.SuRTENodeDao.Save(node);
        }
        public void UpdateNode(SuRTENode node)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (node.NodeHeaderid == null)
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("RequeiredNodeHeaderId"));
            }

            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.SuRTENodeDao.SaveOrUpdate(node);
        }
        public void UpdateNode(SuRTENode node, HttpPostedFile imageFile)
        {
            Spring.Validation.ValidationErrors errors = new Spring.Validation.ValidationErrors();

            if (node.NodeHeaderid == null)
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("RequeiredNodeHeaderId"));
            }
            // Check file Type of imageFile.
            if ((!imageFile.ContentType.Equals("image/gif")) && (!imageFile.ContentType.Equals("image/jpeg")) &&
                (!imageFile.ContentType.Equals("image/jpg")) && (!imageFile.ContentType.Equals("image/x-png")))
            {
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("FileTypeError"));
            }
            // Check file Size of imageFile.
            if (imageFile.ContentLength > maxFileSize)
            {
                // File size exceed 50KB.
                errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("FileSizeError"));
            }
            //// Check file Dimension of imageFile
            //Image uploadImage = Image.FromStream(imageFile.InputStream);
            //if ((uploadImage.Width > maxImageWidth) || (uploadImage.Height > maxImageHeight))
            //{
            //    errors.AddError("Node.Error", new Spring.Validation.ErrorMessage("DimensionError"));
            //}
            if (!errors.IsEmpty) throw new ServiceValidationException(errors);

            DaoProvider.SuRTENodeDao.SaveOrUpdate(node);
        }
        #endregion
    }
}
