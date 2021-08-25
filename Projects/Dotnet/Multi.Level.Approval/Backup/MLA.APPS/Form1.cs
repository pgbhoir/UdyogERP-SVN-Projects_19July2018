using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using MLA.LL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace MLA.APPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string connectionstring;
        public static string Connectionstring
        {
            get { return Form1.connectionstring; }
            set { Form1.connectionstring = value; }
        }

        cls_Gen_Mgr_Approval m_obj_Approval = new cls_Gen_Mgr_Approval(Connectionstring);

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
        }
        private void InitData()
        {
           // m_obj_Approval.GetEntitiesData("SO", "");
        //    grid.DataSource = m_obj_Approval.DsEntities.Tables["main"];

            //grid.DataSource = records;
        }

        private object[,] relations = 
        { 
            {"Item","Account",null},
            {"Item","Account",null}
        };

        //Specifies the maximum number of details 
        private void gridView1_MasterRowGetRelationCount(object sender,
        MasterRowGetRelationCountEventArgs e)
        {
            e.RelationCount = 3;
        }

        //Specifies whether a specific detail contains data 
        //The detail should not be displayed if the corresponding relations element is null 
        private void gridView1_MasterRowEmpty(object sender, MasterRowEmptyEventArgs e)
        {
            e.IsEmpty = IsRelationEmpty(e.RowHandle, e.RelationIndex);
        }

        bool IsRelationEmpty(int rowHandle, int relationIndex)
        {
            return rowHandle ==  GridControl.InvalidRowHandle || relations[rowHandle,
                relationIndex] == null;
        }


        //Get child data for specific relations 
        //The ChildRecordsProducts, ChildRecordsCustomers, ChildRecordsShippers and Records  
        //are custom classes derived from ArrayList 
        private void gridView1_MasterRowGetChildList(object sender,
            MasterRowGetChildListEventArgs e)
        {
            if (IsRelationEmpty(e.RowHandle, e.RelationIndex)) return;
            string s = relations[e.RowHandle, e.RelationIndex].ToString();
            switch (s)
            {
                case "Item":
                    int tran_cd = Convert.ToInt32(gridView1.GetDataRow(e.RowHandle)["tran_cd"]);
                    e.ChildList = m_obj_Approval.GetItemDataList("SO", tran_cd);
                    break;
                case "Account":
                   // e.ChildList = new ChildRecordsCustomers();
                    break;
            }
        }

      

        //Provide names for relations 
        //The names are used to determine pattern Views for representing detail data 
        private void gridView1_MasterRowGetRelationName(object sender,
            MasterRowGetRelationNameEventArgs e)
        {
            if (IsRelationEmpty(e.RowHandle, e.RelationIndex)) 
                e.RelationName = "";
            else 
                e.RelationName = relations[e.RowHandle, e.RelationIndex].ToString();
        }

      


    }
}
