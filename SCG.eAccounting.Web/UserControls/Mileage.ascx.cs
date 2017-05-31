using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Standard.UI;
using SCG.eAccounting.Web.Helper;


namespace SCG.eAccounting.Web.UserControls
{
    public partial class Mileage : BaseUserControl
    {
        #region protected void Page_Load(object sender, EventArgs e)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ctlTxtCarLicenseNo.Text = "พห-4489";
                ctlTxtPermissionNo.Text = string.Empty;
                ctlTxtHomeOffice.Text = "100";
                ctlTxtPrivateUse.Text = "10";
                ctlTxtRate.Text = "7.35";
                ctlTxtExceedingRate.Text = "3.80";
                ctlTxtAmount.Text = "1,000.00";

                ctlLblTotalAmountData.Text = "1,434.50";
                ctlLblSupportData.Text = "800.00";
                ctlLblOverSupportData.Text = "634.50";

                ctlLblOneToHundredDistance.Text = "190";
                ctlLblOneToHundredRate.Text = "7.35";
                ctlLblOneToHundredAmount.Text = "1,396.50";
                ctlLblExceedingHundredDistance.Text = "10";
                ctlLblExceedingHundredRate.Text = "3.80";
                ctlLblExceedingHundredAmount.Text = "38.00";
                ctlLblTotalAmountAllData.Text = "1,434.50";

                ctlLblTotalDistanceData.Text = "200";
                ctlLblTotalRateData.Text = "4.00";
                ctlLblTotalSupportData.Text = "800.00";

                ctlLblTotalAmountOpenData.Text = "800.00";
                
                divAsDetermined.Visible = false;
                divAmount.Visible = false;
                divTotalAmount.Visible = false;
                divTotalAmountOpen.Visible = false;
                ctlLblAdjust.Visible = false;
                ctlTxtAdjust.Visible = false;
                ctlUpdatePanel.Update();
                ctlUpdatePanelTotal.Update();

                this.SetCombo();
            }
        }
        #endregion protected void Page_Load(object sender, EventArgs e)

        #region public void SetCombo()
        private void SetCombo()
        {
            #region Type of Car
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList1 = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfCar1.ID = UIHelper.ParseShort("1");
            TypeOfCar1.Symbol = "Passenger Car";
            translateList1.Add(TypeOfCar1);

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfCar2.ID = UIHelper.ParseShort("2");
            TypeOfCar2.Symbol = "Motorcycle";
            translateList1.Add(TypeOfCar2);

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfCar3 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfCar3.ID = UIHelper.ParseShort("3");
            TypeOfCar3.Symbol = "Pick-up";
            translateList1.Add(TypeOfCar3);

            ctlDdlTypeOfCar.DataSource = translateList1;
            ctlDdlTypeOfCar.DataTextField = "Symbol";
            ctlDdlTypeOfCar.DataValueField = "Id";
            ctlDdlTypeOfCar.DataBind();
            #endregion Type of Car

            #region Type of Use
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList2 = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfUse1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfUse1.ID = UIHelper.ParseShort("1");
            TypeOfUse1.Symbol = "Regular";
            translateList2.Add(TypeOfUse1);

            SS.DB.DTO.ValueObject.TranslatedListItem TypeOfUse2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            TypeOfUse2.ID = UIHelper.ParseShort("2");
            TypeOfUse2.Symbol = "On Assignment";
            translateList2.Add(TypeOfUse2);

            ctlDdlTypeOfUse.DataSource = translateList2;
            ctlDdlTypeOfUse.DataTextField = "Symbol";
            ctlDdlTypeOfUse.DataValueField = "Id";
            ctlDdlTypeOfUse.DataBind();
            #endregion Type of Use

            #region Owner
            IList<SS.DB.DTO.ValueObject.TranslatedListItem> translateList3 = new List<SS.DB.DTO.ValueObject.TranslatedListItem>();

            SS.DB.DTO.ValueObject.TranslatedListItem Owner1 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            Owner1.ID = UIHelper.ParseShort("1");
            Owner1.Symbol = "Employee";
            translateList3.Add(Owner1);

            SS.DB.DTO.ValueObject.TranslatedListItem Owner2 = new SS.DB.DTO.ValueObject.TranslatedListItem();
            Owner2.ID = UIHelper.ParseShort("2");
            Owner2.Symbol = "Company";
            translateList3.Add(Owner2);

            ctlDdlOwner.DataSource = translateList3;
            ctlDdlOwner.DataTextField = "Symbol";
            ctlDdlOwner.DataValueField = "Id";
            ctlDdlOwner.DataBind();

            ctlDdlOwner.Items.Insert(0, new ListItem(String.Format("--{0}--", GetMessage("Please Select")), ""));
            #endregion Owner          
        }
        #endregion public void SetCombo()

        #region protected void ctlDdlOwner_SelectedIndexChanged(object sender, EventArgs e)
        protected void ctlDdlOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlDdlOwner.SelectedItem.Text.ToLower() == "company")
            {
                divAsDetermined.Visible = false;
                divAmount.Visible = true;
                divTotalAmount.Visible = false;
                divTotalAmountOpen.Visible = true;
                ctlLblAdjust.Visible = true;
                ctlTxtAdjust.Visible = true;
                ctlUpdatePanel.Update();
                ctlUpdatePanelTotal.Update();
            }
            else if (ctlDdlOwner.SelectedItem.Text.ToLower() == "employee")
            {
                divAsDetermined.Visible = true;
                divAmount.Visible = false;
                divTotalAmount.Visible = true;
                divTotalAmountOpen.Visible = false;
                ctlLblAdjust.Visible = false;
                ctlTxtAdjust.Visible = false;
                ctlUpdatePanel.Update();
                ctlUpdatePanelTotal.Update();
            }
        }
        #endregion protected void ctlDdlOwner_SelectedIndexChanged(object sender, EventArgs e)

        #region protected void ctlDdlTypeOfCar_SelectedIndexChanged(object sender, EventArgs e)
        protected void ctlDdlTypeOfCar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ctlDdlTypeOfCar.SelectedItem.Text.ToLower() == "motorcycle")
            {
                ctlLblRate.Text = "1-30 lm. Rate";
                ctlLblExceedingRate.Text = "Exceeding 30 km. Rate";
            }
            else
            {
                ctlLblRate.Text = "1-100 lm. Rate";
                ctlLblExceedingRate.Text = "Exceeding 100 km. Rate";
            }
        }
        #endregion protected void ctlDdlTypeOfCar_SelectedIndexChanged(object sender, EventArgs e)
    }
}