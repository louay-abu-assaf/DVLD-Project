using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmManageTestType : Form
    {
        public frmManageTestType()
        {
            InitializeComponent();
        }


        private void LoadDataInfo()
        {
            guna2DataGridView1.DataSource = clsTestType.GetAllTestTypesInfo();
            label2.Text = "#Record : " + guna2DataGridView1.Rows.Count.ToString();
        }

        private void frmManageTestType_Load(object sender, EventArgs e)
        {
            LoadDataInfo();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = System.Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            MessageBox.Show(TestTypeID.ToString());
            frmUpdateTestType frm = new frmUpdateTestType((clsTestType.enTestType)TestTypeID);
            frm.ShowDialog();

            LoadDataInfo();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
