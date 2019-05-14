using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ISO_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PS2_Tools.Backups.Bin_Cue.Convert_To_ISO(textBox1.Text.Trim(), textBox2.Text.Trim() + "\\" + Path.GetFileName(textBox1.Text.Trim().Replace(".cue", "")));
            //once done show message 
            MessageBox.Show("Done");
        }

        private void btnCue_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Select cue File";
            theDialog.Filter = "Cue File |*.cue";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = theDialog.FileName.ToString();
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                textBox2.Text = folderName;
            }
        }
    }
}
