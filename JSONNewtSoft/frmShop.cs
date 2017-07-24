using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace JSONNewtSoft
{
    public partial class frmShop : Form
    {
        public frmShop()
        {
            InitializeComponent();
        }


        //basic constructor -iquatable since we are dealing with lists - want to be easily add and delete off list
        //iequatable - easy to delete things

        class Item : IEquatable<Item>
        {
            public string name;
            public int price;

            public Item(string name, int price = 0)
            {
                this.name = name;
                this.price = price;

            }

            //two of these class items are equal - they have the same name
            public bool Equals(Item other)
            {
                if (other == null) return false;
                return (this.name.Equals(other.name));
            }
        }

        private void btnShopping_Click(object sender, EventArgs e)
        {
             
            {
                // read file into a string and deserialize JSON to a type
                System.Diagnostics.Debug.WriteLine("Reading data.json");
                
                //read from jason and store in string
                string jsonSTRING = File.ReadAllText("data.json");
                //deserialize string into a list
                List<Item> myList = JsonConvert.DeserializeObject<List<Item>>(jsonSTRING);
                //check if file empty
                if (myList == null)
                    myList = new List<Item>();

                string input = "";//keayboard entries
                int inputInt = 0;
                string inputString = "";

              //  while (input != "q")
                {
                    System.Diagnostics.Debug.WriteLine("Press 'a' to Add new Item");
                    System.Diagnostics.Debug.WriteLine("Press 'd' to Delete Item");
                    System.Diagnostics.Debug.WriteLine("Press 's' to Show Content");
                    System.Diagnostics.Debug.WriteLine("Press 'q' to Quit Program");
                    System.Diagnostics.Debug.WriteLine("Press Command:");
                    //input = System.Diagnostics.Debug.ReadLine();
                    input = txtAction.Text;


                    switch (input) //Switch on input string
                    {
                        case "a":
                            System.Diagnostics.Debug.WriteLine("Adding a new item");
                            System.Diagnostics.Debug.WriteLine("Enter item name:");
                            inputString = txtAction.Text;//System.Diagnostics.Debug.ReadLine();
                            System.Diagnostics.Debug.WriteLine("Enter item price (Numeric Values Only):");
                            inputInt = 20;// txtAction.Text; Convert.ToInt32(Console.ReadLine());
                            myList.Add(new Item(inputString, inputInt));
                            System.Diagnostics.Debug.WriteLine("Added item " + inputString + " with price " + inputInt);
                            break;
                        case "d":
                            Console.WriteLine("Deleting item");
                            Console.WriteLine("Enter item name to delete:");
                            inputString = Console.ReadLine();
                            myList.Remove(new Item(inputString));
                            Console.WriteLine("Deleted item with name " + inputString);
                            break;
                        case "q":
                            Console.WriteLine("Quit Program");
                            break;
                        case "s":

                            Console.WriteLine("\nShowing Contents:");
                            foreach (Item item in myList)
                            {
                                Console.WriteLine("Item: " + item.name + " | $" + item.price);
                            }
                            Console.WriteLine("\n");
                            break;
                        default:
                            Console.WriteLine("Incorrect command, try again");
                            break;

                    }
                }
                Console.WriteLine("Rewriting data.json");
                string data = JsonConvert.SerializeObject(myList);
                File.WriteAllText("data.json", data);
                Console.ReadLine();

            }
        }
    }
}
