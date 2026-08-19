using System;
using System.Windows.Forms;

namespace DVLD_Project.User
{
    public partial class frmShowUserInfo : Form
    {
        int _UserID;
        public frmShowUserInfo(int userID)
        {
            InitializeComponent();
            _UserID = userID;
        }

        private void frmShowUserInfo_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
    }
}
