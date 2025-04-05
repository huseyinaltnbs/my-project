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
using Dental_Clinic_Management_Software.Models;
using Dental_Clinic_Management_Software.Pages;
using Kimtoo.BindingProvider;
using Kimtoo.DbManager;
using ServiceStack.OrmLite;


namespace Dental_Clinic_Management_Software.Forms
{
    public partial class FrmMain : Form
    {
        private MouseDragHelper dragHelper;

        public FrmMain()
        {
            InitializeComponent();
            dragHelper = new MouseDragHelper(this);
            dragHelper.Attach(panel1);
            dragHelper.Attach(panel2);
            dragHelper.Attach(pictureBox1);
            dragHelper.Attach(navigtionMenu1);

        }
        private TabControl pages;  // TabControl nesnesi
        private void ResetApplication()
        {
            // Uygulama yeniden başlatılacak
            Application.Restart();
        }


        private void navigtionMenu1_OnItemSelected(object sender, string path, EventArgs e)
        {
            if (path == "      Ayarlar.Database")
            {
                // Veritabanı bağlantı ayarlarını göster
                Kimtoo.DbManager.Connections.Show();

                // Burada veritabanı bağlantısını güncelleriz ve giriş ekranına döneriz
                ResetApplication();
                return;
            }
            if (path == "      Ayarlar.Clinic")
            {
                Cursor.Current = Cursors.WaitCursor;
                new FrmClinic().ShowDialog();
                return;
            }


            label9.Text = path.Trim();
            switch (path)
            {
                case "      Ana Sayfa":
                    bunifuPages1.SetPage("Ana Sayfa");  // Ana Sayfa sayfası
                    break;
                case "      Hastalar":
                    bunifuPages1.SetPage("Hastalar");  // Hastalar sayfası
                    break;
                case "      Dişçiler":
                    bunifuPages1.SetPage("Dişçiler");  // Dişçi sayfası
                    break;
                case "      Randevular":
                    bunifuPages1.SetPage("Randevular");  // Randevu sayfası
                    break;
                case "      Seanslar":
                    bunifuPages1.SetPage("Seanslar");  // Oturum sayfası
                    break;
                case "      Ayarlar":
                    bunifuPages1.SetPage("Ayarlar");  // Ayarlar sayfası
                    break;
                default:
                    break;
            }
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            bunifuFormDock1.WindowState = Bunifu.UI.WinForms.BunifuFormDock.FormWindowStates.Maximized;
            byte[] logoBytes = Connections.GetConnection().Select<Clinic>().FirstOrDefault()?.Logo;
            if (logoBytes != null)
            {
                using (MemoryStream ms = new MemoryStream(logoBytes))
                {
                    resLogo.Image = Image.FromStream(ms);
                }
            }

            #if DEBUG
            //hata ayıklama sürecini hızlandırmak için
            txtEmail.Text = "admin@email.com";
            txtSifre.Text = "admin1";
            #endif

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Giriş doğrulama işlemi
                if (validationProvider1.Validate().Length > 0) return;

                // Kullanıcıyı veritabanından çek
                var record = Connections.GetConnection().Select<Clinic>()
                                        .FirstOrDefault(r => r.Email.ToLower().Trim() == txtEmail.Text.ToLower().Trim());

                // Eğer veritabanında kayıt yoksa, hata almamak için kontrol et
                if (record == null)
                {
                    bunifuSnackbar1.Show(this, "Kullanıcı bulunamadı!", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error);
                    return;
                }

                // Şifreyi kontrol et
                if (record.Sifre.Trim() != txtSifre.Text.Trim())
                {
                    bunifuSnackbar1.Show(this, "Yanlış kullanıcı adı veya şifre", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error);
                    return;
                }

                // Giriş başarılı
                bunifuPages1.SetPage("Ana Sayfa");
                label9.Text = "Ana Sayfa";
                navigtionMenu1.Enabled = true;
                bunifuSnackbar1.Show(this, "Giriş Başarılı", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success);

                // Kullanıcı ismini label1'e aktar
                label1.Text = record.İsim;  // Giriş yapan kullanıcının ismini label1'e aktar
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bilinmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Her durumda navigation menu aktif olsun
                navigtionMenu1.Enabled = true;
            }
        }




        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Form yüklendiğinde label1 "Kullanıcı" olarak kalsın
                label1.Text = "Kullanıcı";  // Varsayılan olarak "Kullanıcı" yazılır

                // Veritabanından ilk kullanıcıyı çek
                var record = Connections.GetConnection().Select<Clinic>().FirstOrDefault();

                // Eğer kayıt varsa, textbox ve labela aktar
                if (record != null)
                {
                    txtEmail.Text = record.Email;
                    txtSifre.Text = record.Sifre;
                }
                else
                {
                    txtEmail.Text = "";
                    txtSifre.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
    }
}
