using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;//optionA
using System.Runtime.Serialization;
using System.IO;//memoryStream
using System.Runtime.ExceptionServices;
using System.Web.Script.Serialization;
 










namespace JSONNewtSoft
{
    public partial class frmEncode : Form
    {
        public frmEncode()
        {
            InitializeComponent();
        }

        private void btnToClass_Click(object sender, EventArgs e)
        

        {
            string json = txtJSON.Text;
            try
            {

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
            catch (Exception ex)
            

            {
                MessageBox.Show(ex.Message);

            }



        }

        private void btnCreateJson_Click(object sender, EventArgs e)
        {
            person returnPerson = new person();

            returnPerson.firstName = txtfirstName.Text;
            returnPerson.lastName = txtlastName.Text;
            returnPerson.contact.Add("phone", txtPhone.Text);
            returnPerson.contact.Add("email", txtEmail.Text);
            returnPerson.contact.Add("Instant Messenger", "absce@provide.edu");

            foreach (string f in lstFriends.Items)
            {
                returnPerson.Friends.Add(f);
            }
            String newJson = Newtonsoft.Json.JsonConvert.SerializeObject(returnPerson);

            MessageBox.Show(newJson);


        }

        private void btnSerialize_Click(object sender, EventArgs e)
        {
            BlogSite bsObj = new BlogSite()
            {
                Name = "C-Sharp",
                Description = "Using DataContractJsonSerializer"
            };

            //  DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(BlogSite)); //Not available in .net Core
            MemoryStream msObj = new MemoryStream();
            //  js.WriteObject(msObj, bsObj);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            // "{\"Description\":\"Using DataContractJsonSerializer\",\"Name\":\"C-Sharp\"}"  
            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

        }

        private void btnDeSerialize_Click(object sender, EventArgs e)
        {
            string json = "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}";

            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                // Deserialization from JSON  
                // DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(BlogSite));////Not available in .net Core
                //BlogSite bsObj2 = (BlogSite)deserializer.ReadObject(ms);
                //  Response.Write("Name: " + bsObj2.Name); // Name: C-Sharp
                // Response.Write("Description: " + bsObj2.Description); // Description: Using DataContractJsonSerializer
            }

        }

        private void btnSerializeJSS_Click(object sender, EventArgs e)

        {
            // Creating BlogSites object  
            BlogSites bsObj = new BlogSites()
            {
                Name = "C-sharp",
                Description = "Share Knowledge"
            };

            // Serializing object to json data  
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(bsObj); // {"Name":"C-sharpcorner","Description":"Share Knowledge"}  

        }

        private void btnDeSerializeJSS_Click(object sender, EventArgs e)
        {
            string jsonData = "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}";

            // Deserializing json data to object
            JavaScriptSerializer js = new JavaScriptSerializer();
            BlogSites blogObject = js.Deserialize<BlogSites>(jsonData);
            string name = blogObject.Name;
            string description = blogObject.Description;

            //// Other way to whithout help of BlogSites class  
            //dynamic blogObject = js.Deserialize<dynamic>(jsonData);
            //string name = blogObject["Name"];
            //string description = blogObject["Description"];
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }

    //private void btnSerializeJSONnet_Click(object sender, EventArgs e)
    //    {
    //        // Creating BlogSites object  
    //        BlogSites bsObj = new BlogSites()
    //        {
    //            Name = "C-Sharp",
    //            Description = "Using DataContractJsonSerializer"
    //        };

    //        // Convert BlogSites object to JOSN string format              
    //        String jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(bsObj);            
    //        Console.Write(jsonData);
    //        MessageBox.Show(jsonData);
    //    }

    //    private void BtnDeSerializeJSONnet_Click(object sender, EventArgs e)
            
    //    {
    //        BlogSites bsObj = new BlogSites();
                
    //       string json = @"{'Name': 'C-Sharp',   'Description': 'Using DataContractJsonSerializer' }";
    //        txtJsonNet.Text = json;
    //                    //Populate object
    //        Newtonsoft.Json.JsonConvert.PopulateObject(json, bsObj);
    //        txtName.Text = bsObj.Name;
    //        txtDescription.Text = bsObj.Description;
            
    //    }
     
 //   }
}
