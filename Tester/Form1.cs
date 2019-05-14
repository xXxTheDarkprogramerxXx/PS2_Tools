using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Convert bin/cue to iso
            PS2_Tools.Backups.Bin_Cue.Convert_To_ISO(@"C:\Users\3deEchelon\Downloads\Pac-Man Fever (USA)\Pac-Man Fever (USA).cue", Application.StartupPath + "\\Pac-Man Fever (USA)");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //"C:\Users\3deEchelon\Desktop\PS2\X2 - Wolverine's Revenge (USA).iso"
            PS2_Tools.Backups.ISO iso = new PS2_Tools.Backups.ISO(@"C:\Users\3deEchelon\Desktop\PS2\X2 - Wolverine's Revenge (USA).iso");
            var isofile = iso.Read_ISO(PS2_Tools.Backups.ISO.ISO_File);

            var ps2id = iso.Read_ContentID(PS2_Tools.Backups.ISO.ISO_File);

            var ps2content = PS2_Tools.PS2_Content.GetPS2Item(ps2id);

        }
    }
}
