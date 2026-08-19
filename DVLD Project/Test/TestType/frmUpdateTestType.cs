using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmUpdateTestType : Form
    {

        clsTestType.enTestType TestTypeID ;
        clsTestType _TestType;

        public frmUpdateTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            this.TestTypeID = TestTypeID;
        }

        private void LoadTestTypeInfo()
        {


            _TestType = clsTestType.Find((clsTestType.enTestType)TestTypeID);
            if (_TestType == null)
            {
                MessageBox.Show("Error Happen", "error");
            }
            else
            {
                lblId.Text = ((int)_TestType.TestTypeID).ToString();
                tbtitle.Text = _TestType.TestTypeName.ToString();
                tbFees.Text = _TestType.TestTypeFee.ToString();
                tbDescription.Text = _TestType.TestTypeDescription.ToString();
            }


        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            LoadTestTypeInfo();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string Title = tbtitle.Text;
            int fee = int.Parse(tbFees.Text);

            _TestType.TestTypeName = Title;
            _TestType.TestTypeFee = fee;

            if (_TestType.UpdateTestTypeInfo())
            {
                MessageBox.Show("Test Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Test Type Failed to Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
