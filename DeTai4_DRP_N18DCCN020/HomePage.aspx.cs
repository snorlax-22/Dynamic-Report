using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DeTai4_DRP_N18DCCN020
{
    public partial class HomePage : System.Web.UI.Page
    {
        public static List<String> listTableName = new List<string>();
        public static List<String> listColumnName = new List<string>();
        public static List<String> listColumnNameTemp1 = new List<string>();
        public static List<String> listTableNameTemp1 = new List<string>();
        public static List<String> listColumnNameTemp2 = new List<string>();
        public static List<String> listTableNameTemp2 = new List<string>();
        public static String constraintColumns = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.GetTableName();
            }
        }

        protected void CheckBoxListTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            constraintColumns = "";
            CheckBoxListColumn.Items.Clear();
            listTableName.Clear();
            listColumnNameTemp2.Clear();
            listTableNameTemp2.Clear();
            LabelMess.Text = "";
            foreach (ListItem item in CheckBoxListTable.Items)
            {
                if (item.Selected)
                {
                    listTableName.Add(item.Text);

                }
            }
            for (int i = 0; i < listTableName.Count; i++)
            {
                GetColumnName(listTableName[i].ToString());

            }
            foreach (ListItem item in CheckBoxListColumn.Items)
            {
                listColumnNameTemp2.Add(item.Text.ToString());
                listTableNameTemp2.Add(item.Value.ToString());
            }
            //string where = "";
            //where += string.Join(", ", listColumnNameTemp2);

            //LabelMess.Text = where;
        }

        protected void CheckBoxListColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in CheckBoxListColumn.Items)
            {

                if (item.Selected)
                {
                    listColumnNameTemp1.Add(item.Text.ToString());
                    listTableNameTemp1.Add(item.Value.ToString());
                }
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("TenCot", Type.GetType("System.String"));
            dt.Columns.Add("TenBang", Type.GetType("System.String"));

            string[] arrTemp1 = listColumnNameTemp1.ToArray();
            string[] arrTemp2 = listTableNameTemp1.ToArray();

            for (int i = 0; i < arrTemp1.GetLength(0); i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["TenCot"] = arrTemp1[i];
                dt.Rows[i]["TenBang"] = arrTemp2[i];
            }
            listColumnNameTemp1.Clear();
            listTableNameTemp1.Clear();

            GridView1.DataSource = dt;
            GridView1.DataBind();


        }

        protected void ButtonClearColumn_Click(object sender, EventArgs e)
        {
            CheckBoxListColumn.Items.Clear();
            listColumnName.Clear();
            GridView1.Controls.Clear();
            for (int i = 0; i < listTableName.Count; i++)
            {
                GetColumnName(listTableName[i].ToString());

            }
            LabelMess.Text = "";
        }

        //function to return first table's primary key and also second table's foreign key
        //use SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME LIKE 'FK%'
        //input: two tables
        //output: name of the column
        public static void GetConstraintColumns(string table1, string table2)
        {
            string constr = ConfigurationManager.ConnectionStrings["dbstring"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME LIKE 'FK_" + table1 + "_" + table2 + "%' OR CONSTRAINT_NAME LIKE 'FK_" + table2 + "_" + table1 + "%'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {
                                constraintColumns = row["COLUMN_NAME"].ToString();
                            }
                        }
                    }
                }
            }
        }

        //function return bool to check if 2 table have foreign key
        public bool CheckFK(string table1, string table2)
        {
            string strConn = ConfigurationManager.ConnectionStrings["dbstring"].ConnectionString;
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            string sql = "SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["CONSTRAINT_NAME"].ToString().Contains(table1) && reader["CONSTRAINT_NAME"].ToString().Contains(table2))
                {
                    return true;
                }
            }
            return false;
        }

        protected void ButtonQuery_Click(object sender, EventArgs e)
        {
            string mess = "";

            string tableName = string.Join(", ", listTableName);
            String columnName = "";
            String orderbyStatement = "";
            mess = "SELECT ";
            String dk = "";


            String where = "";
            int w = 0;

            //function check to check pair by pair of a list of table to see if they have foreign key
            //2 for loop to check each pair
            //input: list of table
            //output: true if they have foreign key
            for (int i = 0; i < listTableName.Count; i++)
            {
                for (int j = i + 1; j < listTableName.Count; j++)
                {
                    if (CheckFK(listTableName[i].ToString(), listTableName[j].ToString()))
                    {
                        GetConstraintColumns(listTableName[i].ToString(), listTableName[j].ToString());
                        if (w == 0)
                        {
                            where += " WHERE " + listTableName[i].ToString() + "." + constraintColumns + " = " + listTableName[j].ToString() + "." + constraintColumns;
                            w = 1;
                        }
                        else
                        {
                            where += " AND " + listTableName[i].ToString() + "." + constraintColumns + " = " + listTableName[j].ToString() + "." + constraintColumns;
                        }
                    }
                }
            }

            String messTemp = where;
            int aaaaaa = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                aaaaaa++;
                TextBox strBang = new TextBox();
                TextBox strCot = new TextBox();
                TextBox dieuKien = (TextBox)GridView1.Rows[i].Cells[2].FindControl("TextBoxDieuKien");
                if (dieuKien.Text.ToString() != "")
                {
                    strBang.Text = GridView1.Rows[i].Cells[4].Text;
                    strCot.Text = GridView1.Rows[i].Cells[3].Text;
                    //check if dk is null and is there any WHERE statement before
                    //if there are WHERE stament in 'mess', add AND
                    //if there isn't WHERE stament 'mess', add WHERE
                    if (dk == "")
                    {
                        if (messTemp.Contains("WHERE"))
                        {
                            dk += " AND " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                        }
                        else
                        {
                            dk += " WHERE " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                        }
                    }
                    else
                    {
                        dk += " AND " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                    }

                    // if(dk.ToString()=="")
                    //     dk += " WHERE " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                    // else
                    //     dk += " AND " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                }


                strBang.Text = GridView1.Rows[i].Cells[4].Text;
                strCot.Text = GridView1.Rows[i].Cells[3].Text;

                String dropdownValue = "";
                
                //get selected value from dropDownList1 
                dropdownValue = ((DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownList1")).SelectedValue.ToString();

                if (dropdownValue != "")
                {
                    if (dropdownValue == "COUNT")
                    {
                        columnName = "COUNT(" + strBang.Text.ToString() + "." + strCot.Text.ToString() + ") AS " + strCot.Text.ToString();
                    }
                    else if (dropdownValue == "MIN")
                    {
                        columnName = "MIN(" + strBang.Text.ToString() + "." + strCot.Text.ToString() + ") AS " + strCot.Text.ToString();
                    }
                    else if (dropdownValue == "MAX")
                    {
                        columnName = "MAX(" + strBang.Text.ToString() + "." + strCot.Text.ToString() + ") AS " + strCot.Text.ToString();
                    }
                     else if (dropdownValue == "ASC")
                    {
                        columnName = strBang.Text.ToString() + "." + strCot.Text.ToString();
                        orderbyStatement = " ORDER BY " + strBang.Text.ToString() + "." + strCot.Text.ToString() + " ASC";
                    }
                     else if (dropdownValue == "DESC")
                    {
                        columnName = strBang.Text.ToString() + "." + strCot.Text.ToString();
                        orderbyStatement = " ORDER BY " + strBang.Text.ToString() + "." + strCot.Text.ToString() + " DESC";
                    }
                }
                else
                {
                    columnName = strBang.Text.ToString() + "." + strCot.Text.ToString();
                }
                
                //columnName = strBang.Text.ToString() + "." + strCot.Text.ToString();

                if (i < GridView1.Rows.Count - 1)
                {
                    columnName += ", ";
                }
                mess += columnName;
            }
            //check in mess, remove 1 of columnName if there are more than 2 column with the same name
            //input: SELECT MaCot, MaCot, TenCot
            //ouput: SELECT MaCot, TenCot
           
            // for(int i = 0; i < listColumnNameTemp2.Count-1; i++)
            // {
            //     GetConstraintColumns(listTableNameTemp2[i], listTableNameTemp2[i+1]);
            //     for (int j = i+1; j < listColumnNameTemp2.Count; j++)
            //     {
            //         if (listColumnNameTemp2[j] == listColumnNameTemp2[i]){
            //             w++;
            //             if (CheckFK(listTableNameTemp2[i], listTableNameTemp2[j]))
            //             {
            //                 if (w > 1)
            //                 {
            //                    where += " AND " + listTableNameTemp2[i].ToString() + "." + listColumnNameTemp2[i] + " = " + listTableNameTemp2[j].ToString() + "." + listColumnNameTemp2[j];
            //                 }
            //                 else
            //                 {
            //                    where += listTableNameTemp2[i].ToString() + "." + listColumnNameTemp2[i] + " = " + listTableNameTemp2[j].ToString() + "." + listColumnNameTemp2[j];
            //                 }
            //             }
            //         }
            //     }
            // }

            // if (!where.Equals(""))
            // {
            //     where = " WHERE " + where;
            // }

            mess += " FROM " + tableName + where + dk + orderbyStatement;
            LabelMess.Text = mess;

        }





        protected void ButtonReport_Click(object sender, EventArgs e)
        {
            String query = LabelMess.Text;
            Session["query"] = query;
            Response.Redirect("Report.aspx");
            Server.Execute("Report.aspx");
        }
        private void GetTableName()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager
                        .ConnectionStrings["dbstring"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "SELECT ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS VALUE, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = 'QLVT18' AND TABLE_NAME NOT LIKE 'sys%' AND TABLE_NAME NOT LIKE 'MS%'";

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["TABLE_NAME"].ToString();
                            item.Value = sdr["VALUE"].ToString();
                            CheckBoxListTable.Items.Add(item);
                            CheckBoxListTable.AutoPostBack = true;
                        }
                    }
                    conn.Close();
                }
            }
        }
        private void GetColumnName(String tableName)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager
                        .ConnectionStrings["dbstring"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME NOT LIKE 'rowguid%'";

                    cmd.CommandText = query;
                    cmd.Connection = conn;
                    conn.Open();
                    int i = 0;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = sdr["COLUMN_NAME"].ToString();
                            item.Value = tableName.ToString();
                            CheckBoxListColumn.Items.Add(item);
                            CheckBoxListColumn.RepeatColumns = 5;
                            CheckBoxListColumn.AutoPostBack = true;
                            DataTable dt = new DataTable(tableName);
                            i++;
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}