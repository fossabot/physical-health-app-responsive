﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PhysicalHealthApp
{
    public partial class ClinicianSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = "";
                try
                {
                    id = Request.QueryString["id"].ToString();
                }
                catch
                {
                    Response.Redirect("Default.aspx");
                    return;
                }

                this.hdnClinicianID.Value = id;

                GetClinicianDataFromString(id);

                switch (Session["userType"].ToString().ToLower())
                {
                    case "patient":
                        Response.Redirect("Unauthorised.aspx");
                        break;
                    case "clinician":
                        this.lblSummaryType.Text = "My Summary";
                        if (id != Session["userID"].ToString())
                        {
                            Response.Redirect("Unauthorised.aspx");
                        }
                        break;
                    case "super user":
                        Response.Redirect("Unauthorised.aspx");
                        break;
                }

                GetMyPatients(id);

            }
        }

        private void GetMyPatients(string id)
        {
            string sql = "SELECT * FROM app_user WHERE matchedclinicianid = CAST(@userid AS INT);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("userid", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            this.dgMyPatients.DataSource = dt;
            this.dgMyPatients.DataBind();

        }

        private void GetClinicianDataFromString(string id)
        {
            string sql = "SELECT * FROM app_user WHERE userid = CAST(@userid AS INT);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("userid", id)
            };



            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {

                string userFullName = dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["lastname"].ToString();
                try
                {
                    this.lblClinianName.Text = userFullName.ToUpper();
                }
                catch { }

                try
                {
                    this.lblGMC.Text = dt.Rows[0]["gmccode"].ToString();
                }
                catch { }
               
            }
        }
    }
}