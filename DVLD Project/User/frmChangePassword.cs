using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmChangePassword : Form
    {
        int _personID;
        private clsPerson _Person;
        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;

            this.Size = new System.Drawing.Size(870, 780);

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void _ResetDefualtValues()
        {
            tbCurrentPassword.Text = "";
            tbPassword.Text = "";
            tbComfirm.Text = "";
            tbCurrentPassword.Focus();
        }


        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            _User = clsUser.Find(_UserID);

            if (_User == null)
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;

            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }


        private bool CheckValidation()
        {
            return (string.IsNullOrEmpty(tbPassword.Text) ||
                string.IsNullOrEmpty(tbCurrentPassword.Text) ||
                string.IsNullOrEmpty(tbComfirm.Text));
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckValidation())
            {
                MessageBox.Show("Fill All Fields to Save The Data ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (tbComfirm.Text == tbPassword.Text)
                {
                    if (_User.ChangePassword(tbComfirm.Text))
                    {
                        MessageBox.Show("Password Changed Successfully ", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _ResetDefualtValues();
                    }

                }
                else
                {
                    MessageBox.Show("password and Confirm Password Not Match ", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }



            }
        }

        private void CheckCurrentPassword(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(tbCurrentPassword.Text.Trim()))
            {

                errorProvider1.SetError(tbCurrentPassword, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(tbCurrentPassword, null);
            }
            ;

            if (_User.Password != tbCurrentPassword.Text.Trim())
            {

                errorProvider1.SetError(tbCurrentPassword, "Current password is wrong!");
                return;
            }
            else
            {
                errorProvider1.SetError(tbCurrentPassword, null);
            }
            ;
        }

        private void tbComfirm_Leave(object sender, EventArgs e)
        {
            if (tbComfirm.Text != tbPassword.Text)
            {
                errorProvider1.SetError(tbComfirm, "Password Not Match");
            }
            else
            {
                errorProvider1.SetError(tbComfirm, "");

            }
        }
    }
}
