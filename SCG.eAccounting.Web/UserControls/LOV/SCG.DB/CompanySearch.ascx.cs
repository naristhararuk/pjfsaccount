using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SS.Standard.UI;

namespace SCG.eAccounting.Web.UserControls.LOV.SCG.DB
{
    public partial class WebUserControl_CompanySearch : BaseUserControl
    {
        //private ICompanyService companyService;

        //public ICompanyService CompanyService
        //{
        //    get { return companyService; }
        //    set { companyService = value; }
        //}

        //public string CompanyName
        //{
        //    //get { return txtCompanyName.Text; }
        //    //set { txtCompanyName.Text = value; }
        //}

        //public int zIndex
        //{
        //    //get { return ModalPopupExtender1.zIndex; }
        //    //set { ModalPopupExtender1.zIndex = value; }
        //}



        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void grdCompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "SelectCompany")
            //{
            //    // Retrieve Object Row from GridView Selected Row
            //    GridViewRow selectedRow = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            //    int companyId = UIHelper.ParseInt(grdCompany.DataKeys[selectedRow.RowIndex].Value.ToString());
            //    Company selectedCompany = companyService.FindProxyByIdentity(companyId);

            //    // Return Selected Employee.
            //    CallOnObjectLookUpReturn(selectedCompany);

            //    // Hide Modal Popup.
            //    this.ModalPopupExtender1.Hide();
            //}
        }
        protected void grdCompany_DataBound(object sender, EventArgs e)
        {

        }
        protected void grdCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdCompany.PageIndex = e.NewPageIndex;
            //grdCompany.DataBind();
        }


        #region ObjectDataSource Event

        protected void odsCompany_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            //e.ObjectInstance = companyService;
        }
        #endregion

        #region LinkButton Event
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            //string empNo = txtEmpNo.Text;
            //string empName = txtEmpName.Text;
            //string idCardNo = txtIDCardNo.Text;
            //string birthDate = calBirthDate.DateValue;
            //int countryId = UIHelper.ParseInt(Session[SessionName.COUNTRY_ID].ToString());
            //int companyId = UIHelper.ParseInt(ddlCompany.SelectedValue);
            //int locationId = UIHelper.ParseInt(ddlLocation.SelectedValue);
            //int jobTitleId = UIHelper.ParseInt(ddlJobTitle.SelectedValue);

            //grdEmployee.DataSource = employeeService.FindBySearchCriteria(empNo, empName, idCardNo, birthDate, countryId, companyId, locationId, jobTitleId);
            // //grdCompany.DataBind();
            // //UpdatePanelCompanyGrid.Update();
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion

        #region Public Method
        public void Show()
        {
            //CallOnObjectLookUpCalling();
            //this.UpdatePanelSearchCriteria.Update();

            //grdCompany.DataBind();

            //this.ModalPopupExtender1.Show();
        }

        public void Hide()
        {
            //this.ModalPopupExtender1.Hide();
        }
        #endregion
    }
}
