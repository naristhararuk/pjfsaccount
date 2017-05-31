using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

using SS.SU.BLL;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.SU.Query;
using SCG.eAccounting.Web.Helper;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls
{
    public partial class ServiceShowCase : BaseUserControl
    {
        public ISuRTENodeService SuRTENodeService { get; set; }
        short languageId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                languageId = UserAccount.CurrentLanguageID;

                IList<SuRTENodeSearchResult> list = SuRTENodeService.GetRTENodeList(languageId, "Service");
                if(list!=null && list.Count>0)
                {
                    ctlNode1.Text = list[0].Header;
                    ctlContent1.Text = list[0].Content;

                    IList<SuRTENodeSearchResult> listItem = SuRTENodeService.GetRTEContentList( UserAccount.CurrentLanguageID, "Service", list[0].NodeId);
                    rptContent.DataSource = listItem;
                    rptContent.DataBind();
                }
            }
        }

        protected void rptContent_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton ctlContent = (LinkButton)item.FindControl("ctlContent");
                Label ctlContentID = (Label)item.FindControl("ctlContentID");
                Image ctlImage = (Image)item.FindControl("ctlImage");
                SuRTENodeSearchResult suRTENode = (SuRTENodeSearchResult)item.DataItem;
                short nodeId = suRTENode.NodeId;
                short languageId = suRTENode.LanguageId;
                SuRTENode rteNode = SuRTENodeService.FindByIdentity(nodeId);
                string filePath = ParameterServices.FilePathService;
                string imageName = rteNode.ImagePath;
                ctlImage.ImageUrl = filePath + imageName;
            }
        }

        protected void rptContent_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            RepeaterItem item = e.Item;
            LinkButton ctlContent = (LinkButton)item.FindControl("ctlContent");
            Label ctlContentID = (Label)item.FindControl("ctlContentID");

            IList<SuRTENodeSearchResult> listItem = SuRTENodeService.GetRTEContent(UserAccount.CurrentLanguageID, "Service", UIHelper.ParseShort(ctlContentID.Text));
            if (listItem != null && listItem.Count > 0)
            {
                ctlNode1.Text = listItem[0].Header;
                ctlContent1.Text = listItem[0].Content;
                updPanelContent.Update();
            }
        }
    }
}