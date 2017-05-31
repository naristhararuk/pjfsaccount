using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.eAccounting.Web.UserControls.DocumentEditor
{
    public delegate void DsNullHandler();
    public interface IDocumentEditor
    {
        void Initialize(string initFlag, long? documentID);
        long Save();
        void RollBackTransaction();
        bool IsContainEditableFields(object editableFieldEnum);
        bool IsContainVisibleFields(object visibleFieldEnum);
        void Copy(long wfid);

        void EnabledViewPostButton(bool IsLock);
        void EnabledPostRemittanceButton(bool IsLock);

        event DsNullHandler DsNull;
        bool RequireDocumentAttachment();
    }
}
