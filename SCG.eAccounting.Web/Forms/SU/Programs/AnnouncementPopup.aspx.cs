using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SS.Standard.UI;

using SS.SU.BLL;
using SS.SU.DTO;
using SS.SU.Query;
using SS.SU.DTO.ValueObject;

using SCG.eAccounting.Web.Helper;

using SS.DB.Query;
using SS.DB.DTO;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.Forms.SU.Programs
{
    public partial class AnnouncementPopup : BasePage
    {
        #region Property
        public ISuAnnouncementLangQuery SuAnnouncementLangQuery { get; set; }
        public ISuAnnouncementService SuAnnouncementService { get; set; }
        public IDbLanguageService DbLanguageService { get; set; }
        #endregion

        #region Page_Load
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            //ProgramCode = "AnnouncementPopup";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            getAnnouncementDetail();
        }
        #endregion

        #region Public Method
        private void getAnnouncementDetail()
        {
            short announcementId = UIHelper.ParseShort(Request.QueryString["announcementId"]);
            short languageId = UIHelper.ParseShort(Request.QueryString["languageId"]);
            IList<AnnouncementLang> announcementLangList = SuAnnouncementLangQuery.FindByDateAnnouncementLangId(announcementId, languageId);
            rptAnnouncementDetail.DataSource = announcementLangList;
            rptAnnouncementDetail.DataBind();
        }
        #endregion

        #region Event
        protected void rptAnnouncementDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            int paramImage = 0;
            if (ParameterServices.DisplayNewImage != "")
                paramImage = Int32.Parse(ParameterServices.DisplayNewImage);
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                Panel ctlPanelGroup = item.FindControl("ctlPanelGroup") as Panel;
                Label ctlDayOfMonth = item.FindControl("ctlDayOfMonth") as Label;
                Label ctlMonthOfYear = item.FindControl("ctlMonthOfYear") as Label;
                System.Web.UI.WebControls.Image ctlImageNew = item.FindControl("ctlImageNew") as System.Web.UI.WebControls.Image;
                Image ctlImage = item.FindControl("ctlImage") as Image;
                Image ctlImageHeader = item.FindControl("ctlImageHeader") as Image;

                AnnouncementLang announcementLang = (AnnouncementLang)item.DataItem;
                short announcementId = announcementLang.AnnouncementId.GetValueOrDefault(0);

                SuAnnouncement announcement = SuAnnouncementService.FindByIdentity(announcementId);

                DateTime EffectiveDate = announcement.EffectiveDate.Value;

                string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
                string imageName = announcementLang.ImagePath;
                string ststusfilePath = ParameterServices.IconUploadFilePath;

                if (EffectiveDate.AddDays(paramImage) >= DateTime.Now.Date)
                    ctlImageHeader.ImageUrl = ststusfilePath + "new.gif";
                else
                    ctlImageHeader.ImageUrl = ststusfilePath + "blank.gif";
                //ctlImage.ImageUrl = imageName.Trim();
                try
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\") + imageName);
                    ctlPanelGroup.Width = image.Width;
                    ctlPanelGroup.Height = image.Height;
                    ctlPanelGroup.BackImageUrl = filePath + imageName;
                }
                catch (Exception)
                {

                }


                ctlDayOfMonth.Text = UIHelper.ToDateString(announcement.CreDate, "dd");
                ctlMonthOfYear.Text = UIHelper.ToDateString(announcement.CreDate, "MMM");
            }
        }
        #endregion
    }
}
