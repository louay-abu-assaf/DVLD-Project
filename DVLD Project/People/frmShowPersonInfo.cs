using DVLD_Buisness_Tier;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmShowPersonInfo : Form
    {
        int _PersonID;
        clsPerson person;

        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            LoadPersonInfo();
        }



        public void LoadPersonInfo()
        {
            person = clsPerson.Find(_PersonID);

            if (person == null)
            {
                MessageBox.Show("Person not found.");
                return;
            }

            clsCountry Country = clsCountry.Find(person.CountryID);
            ctrPersonInfo1.LoadPersonInfo(_PersonID);

        }


        private void ShowPersonInfo_Load(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
