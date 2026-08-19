using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        clsApplicationType _ApplicationType;

        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
            LoadApplicationInfo();
        }

        private void LoadApplicationInfo()
        {
            if (_ApplicationTypeID == -1)
            {
                MessageBox.Show("Application Not Found ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);
                if (_ApplicationType == null)
                {
                    MessageBox.Show("Error Happen", "error");
                }
                else
                {
                    lblId.Text = _ApplicationTypeID.ToString();
                    tbtitle.Text = _ApplicationType.ApplicationTitle.ToString();
                    tbFees.Text = _ApplicationType.ApplicationFee.ToString();
                }

            }
        }
        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string Title = tbtitle.Text;
            int fee = int.Parse(tbFees.Text);

            _ApplicationType.ApplicationTitle = Title;
            _ApplicationType.ApplicationFee = fee;

            if (_ApplicationType.UpdateApplicationType())
            {
                MessageBox.Show("Application Type Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Application Type Failed to Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
