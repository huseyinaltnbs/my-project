using Dental_Clinic_Management_Software.Models;
using Kimtoo.DbManager;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Dental_Clinic_Management_Software.Lib;
namespace Dental_Clinic_Management_Software
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                var db = Connections.GetConnection();

                // Eğer veritabanında hiç klinik yoksa, yeni bir tane oluştur
                if (db.Select<Clinic>().Count() == 0)
                {
                    db.Save(
                        new Clinic
                        {
                            İsim = "Dental Clinic",
                            Email = "admin@gmail.com",
                            Sifre = "1",
                            Adres = "Kahramanmaras, Onikisubat",
                            Logo = Image.FromFile("logo.png").ToBytes(),
                            Telefon = "+905222222222"
                        });
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bilinmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // DPI Ayarları (Windows Vista ve üstü için)
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            // Windows Forms Ayarları
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.FrmMain());
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public static bool IsInDesignMode(this UserControl container)
        {
            // Tasarım zamanında olup olmadığını kontrol et
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                // Tasarımda ise, bir Label ekleyebilirsiniz
                if (container.Controls.Count == 0)  // Kontrollerin sayısı 0 ise, label ekleyelim
                {
                    container.Controls.Add(new Label()
                    {
                        Text = container.GetType().Name,
                        AutoSize = false,
                        TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                        Dock = DockStyle.Fill,
                    });
                }
                return true;
            }
            return false;
        }

    }
}
