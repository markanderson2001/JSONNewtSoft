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

            person Athom = new person();

            Newtonsoft.Json.JsonConvert.PopulateObject(json, Athom);
            MessageBox.Show(Athom.firstName);


        }
    }
}
