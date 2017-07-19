using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;

namespace JSONNewtSoft
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnToClass_Click(object sender, EventArgs e)
        {
            string json = txtJSON.Text;

            person Mark = new person();

            Newtonsoft.Json.JsonConvert.PopulateObject(json, Mark);
            //MessageBox.Show(Athom.firstName);
            
            txtfirstName.Text = Mark.firstName;
            txtlastName.Text = Mark.lastName;
            txtEmail.Text = Mark.contact["email"];
            txtPhone.Text = Mark.contact["phone"];

            foreach (string friend in Mark.Friends)
            {
                lstFriends.Items.Add(friend);
            }


        }

        private void btnCreateJson_Click(object sender, EventArgs e)
        {
            person returnPerson = new person();

            returnPerson.firstName = txtfirstName.Text;
            returnPerson.lastName = txtlastName.Text;
            returnPerson.contact.Add("phone",txtPhone.Text);
            returnPerson.contact.Add("email", txtEmail.Text);
            returnPerson.contact.Add("Instant Messenger", "absce@provide.edu");

            foreach (string f in lstFriends.Items)
            {
                returnPerson.Friends.Add(f);
            }
            String newJson = Newtonsoft.Json.JsonConvert.SerializeObject(returnPerson);

            MessageBox.Show(newJson);


        }
    }
}
