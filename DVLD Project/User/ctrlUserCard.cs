using DVLD_Buisness_Tier;
using System.Windows.Forms;

namespace DVLD_Project.User
{
    public partial class ctrlUserCard : UserControl
    {

        private clsUser _User;
        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }


        public ctrlUserCard()
        {
            InitializeComponent();
        }


        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            if (_User == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillUserInfo();
        }


        private void _ResetPersonInfo()
        {

            ctrPersonInfo1.ResetPersonInfo();
            lblID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblisActive.Text = "[???]";
        }


        private void _FillUserInfo()
        {

            ctrPersonInfo1.LoadPersonInfo(_User.PersonID);
            lblID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();

            if (_User.IsActive)
                lblisActive.Text = "Yes";
            else
                lblisActive.Text = "No";

        }
    }
}
