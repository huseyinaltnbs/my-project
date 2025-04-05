using Dental_Clinic_Management_Software.Models;
using Kimtoo.DbManager;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dental_Clinic_Management_Software.Lib;
using ServiceStack;

namespace Dental_Clinic_Management_Software.Forms
{
    public partial class FrmClinic : Form
    {
        private Clinic record;
        public FrmClinic()
        {
            InitializeComponent();
            this.record = Connections.GetConnection().Select<Clinic>().FirstOrDefault();
            bindingProvider1.Bind(record);
            resLogo.Image = ToImage(record.Logo);
            Cursor.Current = Cursors.Default;
        }


        public static Image ToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            ms.Position = 0;
            return Image.FromStream(ms, true);
        }
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resLogo.Image = Image.FromFile(openFileDialog1.FileName);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            record.Logo =ImageToByteArray(resLogo.Image);
            Connections.GetConnection().Save(record);
            bunifuSnackbar1.Show(this.FindForm(), "Saved", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success);
        }
    }
}
