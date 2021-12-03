
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using dnlib.DotNet;
namespace StringExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ExtractedStrings = new List<string>();
        }

        private AssemblyDef Assembly;
        private List<string> ExtractedStrings;
        private void button1_Click(object sender, EventArgs e)
        {
            //dialogue so that user can select file that is either a exe or dll
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = ".NET Files|*.exe;*.dll";
                ofd.CheckFileExists = true;
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.FileName;
                    LoadAssembly(ofd.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program will extract strings found in\n" +
                "the #US stream (if one exists), and will attempt to\n" +
                "manipulate them. This will be useful for people that want\n" +
                "to see the user strings when theyre potientially hidden by\n" +
                "obfuscation. (where you cannot see them).\n" +
                "\n" +
                "Take note that if the strings in the assembly are encoded, the results\n" +
                "will be as well.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Assembly != null)//make sure assembly is not empty
            { 
                var USStream = ((ModuleDefMD)(Assembly.ManifestModule)).Metadata.USStream;//get #US stream
                if (USStream == null) throw new Exception("The #US stream is not present.");
                var USStreamReader = USStream.CreateReader();//initiate a reader from the stream
                do
                {
                    //strings are seperated by \0 zero terminators (which are used to seperate chars and strings in
                    //the #US heap. To fix this we Read the serialized string (as before every string it contains 
                    //the length for the string in the form of a int) so it saves us all the work from finding and fixing the chars.
                    //so now that we have all the strings, all the chars are null terminator seperated so to fix this we simply create
                    //a new array without the \0 values and add it to the list for later use. (Also making sure the array is UTF8 encoded
                    //so that their are no unicode characters)

                    //we then join every string in the list with the ',' char (so that it is all in one string). We do this
                    //so that we can retrieve every string cleanly as if we dont, we will get null strings. We then create
                    //a array of strings from the split characters and simply enumerate through them all and create listview items
                    //for them.

                    //in try catch exception block to guard from any exceptions from disrupting and stopping entire process
                    //as for some streams their may contain a misleading character terminator at the beginning which will cause
                    //an error
                    try
                    {
                        var str = USStreamReader.ReadSerializedString();
                        var strData = Encoding.UTF8.GetBytes(str);
                        var replacedData = strData.Where(x => x != 0).ToArray();
                        ExtractedStrings.Add(Encoding.UTF8.GetString(replacedData));
                    }
                    catch(Exception ex) { }
                  
                } while (USStreamReader.CurrentOffset < USStreamReader.EndOffset);
                var res = string.Join(",", ExtractedStrings);
                ExtractedStrings.Clear();
                var split = res.Split(new char[] { ',' });
                for(var i = 0; i < split.Length; i++)
                {
                    if (split[i] != "")
                    {
                        var lvItem = new ListViewItem();
                        lvItem.Text=i.ToString();
                        lvItem.SubItems.Add(split[i]);
                        listView1.Items.Add(lvItem);
                    }
                }
            }
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textBox1.Text = files[0];
                LoadAssembly(files[0]);
            }
        }
        private void LoadAssembly(string path)
        {
            //load assembly from given path, while also clearing all items
            //**Maybe add check to see if its really .NET by checking
            //CLR header size
            Assembly = AssemblyDef.Load(path);
            listView1.Items.Clear ();
        }
    }
}
