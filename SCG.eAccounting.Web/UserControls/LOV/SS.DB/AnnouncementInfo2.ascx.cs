using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SS.Standard.Security;
using SS.Standard.UI;

using SS.SU.DTO;
using SS.SU.DTO.ValueObject;

using SS.SU.BLL;
using SS.SU.Query;

using SCG.eAccounting.Web.Helper;

using SS.DB.DTO;
using SS.DB.Query;
using SS.DB.BLL;

namespace SCG.eAccounting.Web.UserControls.LOV.SS.DB
{
	public partial class AnnouncementInfo2 : BaseUserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            short languageId = UserAccount.CurrentLanguageID;
            short announcementGroupId = UIHelper.ParseShort(ParameterServices.DefaultAnnoucementGroup);
			
			GetNews(languageId, announcementGroupId);
		}
		
		private void GetNews(short languageId, short announcementGroupId)
		{
			
			IList<SuAnnouncementGroupLang> announcmentGroupLangList = QueryProvider.SuAnnouncementGroupLangQuery.FindByAnnouncementGroupIdAndLanguageId(languageId, announcementGroupId);
			if (announcmentGroupLangList.Count > 0)
			{
				ctlHeader.Text = announcmentGroupLangList[0].AnnouncementGroupName;
			}
			else
			{
				ctlHeader.Text = "News";
			}
			
			IList<SuAnnouncementLang> announcementList = QueryProvider.SuAnnouncementLangQuery.FindByAnnouncementLanguageId(languageId, announcementGroupId);
			rptAnnouncement.DataSource = announcementList;
			rptAnnouncement.DataBind();
		}
		protected void rptAnnouncement_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			RepeaterItem item = e.Item;

			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
			{
				Panel ctlPanelGroup = item.FindControl("ctlPanelGroup") as Panel;
				Label ctlDayOfMonth = item.FindControl("ctlDayOfMonth") as Label;
				Label ctlMonthOfYear = item.FindControl("ctlMonthOfYear") as Label;
				System.Web.UI.WebControls.Image ctlImageNew = item.FindControl("ctlImageNew") as System.Web.UI.WebControls.Image;
				LinkButton ctlReadMore = item.FindControl("ctlReadMore") as LinkButton;

				SuAnnouncementLang announcementLang = (SuAnnouncementLang)item.DataItem;
                string filePath = ParameterServices.AnnouncementGoupUploadFilePath;
				string imageName = announcementLang.Announcement.AnnouncementGroup.ImagePath;

				DateTime EffectiveDate = announcementLang.Announcement.EffectiveDate.Value;
				int paramImage = 0;
                if (ParameterServices.DisplayNewImage != "")
				{
                    paramImage = Int32.Parse(ParameterServices.DisplayNewImage);
				}
                string statusFilePath = ParameterServices.IconUploadFilePath;
				if (EffectiveDate.AddDays(paramImage) >= DateTime.Now.Date)
				{
					ctlImageNew.ImageUrl = statusFilePath + "new.gif";
				}
				else
				{
					ctlImageNew.ImageUrl = statusFilePath + "blank.gif";
				}
                try
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + filePath.Replace("~", string.Empty).Replace("/", "\\") + imageName);
                    ctlPanelGroup.Width = image.Width;
                    ctlPanelGroup.Height = image.Height;
                    ctlPanelGroup.BackImageUrl = filePath + imageName;
                }
                catch(Exception)
                {
                    //for not found file using default image
                }
			
				
				ctlDayOfMonth.Text = UIHelper.ToDateString(announcementLang.Announcement.CreDate, "dd");
				ctlMonthOfYear.Text = UIHelper.ToDateString(announcementLang.Announcement.CreDate, "MMM");

                string newWindowUrl = ResolveClientUrl("~/Forms/SU/Programs/AnnouncementPopup.aspx?announcementId=" + announcementLang.AnnouncementId + "&languageId=" + announcementLang.LanguageId);				
				ctlReadMore.OnClientClick = "javascript:window.open('" + newWindowUrl + "',null,'height=300,width=600,status=yes,toolbar=no,menubar=no,location=no,resizable=1,scrollbars=1'); return false;";
			}
		}
	}
}