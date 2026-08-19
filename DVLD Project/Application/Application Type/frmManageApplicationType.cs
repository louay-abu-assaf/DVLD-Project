using DVLD_Buisness_Tier;
using System;
using System.Data;
using System.Windows.Forms;


namespace DVLD_Project
{
    public partial class frmManageApplicationType : Form
    {
        public frmManageApplicationType()
        {
            InitializeComponent();
        }



        private void frmManageApplicationType_Load(object sender, EventArgs e)
        {
            LoadApplicationType();
        }


        private void LoadApplicationType()
        {
            DataTable dataTable = clsApplicationType.GetAllApplicationType();
            guna2DataGridView1.DataSource = dataTable;

            int Count = dataTable.Rows.Count;

            label2.Text = "# Record : " + Count;
        }



        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = System.Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells[0].Value);

            MessageBox.Show(ApplicationID.ToString());
            clsApplicationType applicationType = clsApplicationType.Find(ApplicationID);
            frmUpdateApplicationType frm = new frmUpdateApplicationType(ApplicationID);
            frm.ShowDialog();

            LoadApplicationType();
        }
    }
}
