using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.SU.DTO;

using SS.SU.BLL;
using SS.Standard.Security;
using SS.Standard.UI;

using SS.SU.Query;

using SS.DB.DTO;
using SS.DB.BLL;
using SS.DB.Query;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
    public partial class AnnouncementInfo : BaseUserControl
    {
        public ISuAnnouncementGroupLangService SuAnnouncementGroupLangService { get; set; }
        public ISuAnnouncementService SuAnnouncementService { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                getAnnouncementGroup();
        }

        protected void rptAnnouncementGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;

            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptAnnouncement = (Repeater)item.FindControl("rptAnnouncement");
                Image ctlImage = item.FindControl("ctlImage") as Image;
                SuAnnouncementGroupLang suAnnouncementGroupLang = (SuAnnouncementGroupLang)item.DataItem;
                short announcementGroupId = suAnnouncementGroupLang.AnnouncementGroupid;
                short languageId = suAnnouncementGroupLang.Language.Languageid;
                IList<SuAnnouncementLang> list = QueryProvider.SuAnnouncementLangQuery.FindByAnnouncementLanguageId(languageId, announcementGroupId);
                string imageName = suAnnouncementGroupLang.AnnouncementGroup.ImagePath;
                string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
                ctlImage.ImageUrl = filePath + imageName.Trim();
                // bind the Child Repeater 
                rptAnnouncement.DataSource = list;
                rptAnnouncement.DataBind();  
            }
        }
        protected void rptAnnouncement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            int paramImage = 0;
            if (ParameterServices.DisplayNewImage != "")
                paramImage = Int32.Parse(ParameterServices.DisplayNewImage);

            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton ctlAnnouncement = (LinkButton)item.FindControl("ctlAnnouncement");
                Image imgNew = (Image)item.FindControl("imgNew");
                SuAnnouncementLang suAnnouncementLang = (SuAnnouncementLang)item.DataItem;
                short announcementId = suAnnouncementLang.AnnouncementId;
                short languageId = suAnnouncementLang.Language.Languageid;
                SuAnnouncement announcement = SuAnnouncementService.FindByIdentity(announcementId);
                DateTime EffectiveDate = announcement.EffectiveDate.Value;
                string filePath = ParameterServices.IconUploadFilePath ;
                if (EffectiveDate.AddDays(paramImage) >= DateTime.Now.Date)
                {
                    imgNew.ImageUrl = filePath + "new.gif";
                }
                else
                {
                    imgNew.ImageUrl = filePath + "blank.gif";
                }
                string newWindowUrl = "../SCG.eAccounting.Web/Forms/SU/Programs/AnnouncementPopup.aspx?announcementId=" + announcementId + "&languageId=" + languageId;
                ctlAnnouncement.Attributes.Add("onclick", "javascript:window.open('" + newWindowUrl + "',null,'height=300,width=600,status=yes,toolbar=no,menubar=no,location=no')");
            }
        }
        private void getAnnouncementGroup()
        {
            short languageId = 1; //UserAccount.CurrentLanguageID
            IList<SuAnnouncementGroupLang> list = QueryProvider.SuAnnouncementGroupLangQuery.FindByAnnouncementLanguageId(languageId);
            rptAnnouncementGroup.DataSource = list;
            rptAnnouncementGroup.DataBind();
        }
    }
}