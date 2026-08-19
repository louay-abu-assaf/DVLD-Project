using DVLD_Buisness_Tier;
using DVLD_Project.Global;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }


        private void radButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUsernameAndPassword(guna2TextBox1.Text.Trim(), guna2TextBox2.Text.Trim());

            if (user != null)
            {

                if (checkBox1.Checked)
                {
                    //store username and password
                    clsGlobal.RememberUsernameAndPassword(guna2TextBox1.Text.Trim(), guna2TextBox2.Text.Trim());

                }
                else
                {
                    //store empty username and password
                    clsGlobal.RememberUsernameAndPassword("", "");

                }

                //incase the user is not active
                if (!user.IsActive)
                {

                    guna2TextBox1.Focus();
                    MessageBox.Show("Your Account is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;
                this.Hide();
                MainScreen frm = new MainScreen();
                frm.ShowDialog();


            }
            else
            {
                guna2TextBox1.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                guna2TextBox1.Text = UserName;
                guna2TextBox2.Text = Password;
                checkBox1.Checked = true;
            }
            else
                checkBox1.Checked = false;
        }
    }
}
