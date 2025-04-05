using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dental_Clinic_Management_Software.Models;
using Kimtoo.BindingProvider;
using ServiceStack.OrmLite;
namespace Dental_Clinic_Management_Software.Pages

{
    public partial class PageAppointments : UserControl
    {
        public bool IsAppointments { get; set; } = true;
        public PageAppointments()
        {
            if (this.IsInDesignMode()) return;
            InitializeComponent();
            LoadData();
            grid.OnDelete<Appointment>((a,b) => Kimtoo.DbManager.Connections.GetConnection().Delete(a) >= 0);
        }

        private void LoadData()
        {
            try
            {
                tabCat.Visible = this.IsAppointments;
                Cursor.Current = Cursors.WaitCursor;

                var db = Kimtoo.DbManager.Connections.GetConnection();

                // Bind dropdowns //Açılır menüleri bağla
                cDentists.DataSource = db.Select<Dentist>().ToList(); // .ToList() ile veriyi hemen yükleyin
                cDentists.Refresh();
                cPatients.DataSource = db.Select<Patient>().ToList();
                cPatients.Refresh();
                cDentists.BindingContext = new BindingContext();
                cPatients.BindingContext = new BindingContext();


                // Fetch and bind appointment data
                List<Appointment> data = db.Select<Appointment>();

                data = data.Where(r => r.Tarih.Date == datePick.Value.Date).ToList();
                if (this.IsAppointments)
                {
                    if (tabCat.CurrentSelection.Trim() == "Aktif")
                    {
                        // Sadece iptal edilmemiş randevuları göster
                        data = data.Where(r => !r.İptal).ToList();
                    }
                    else if (tabCat.CurrentSelection.Trim() == "İptal")
                    {
                        // Sadece iptal edilen randevuları göster
                        data = data.Where(r => r.İptal).ToList();
                    }
                }
                else
                {
                    data = data.Where(r => r.HasSessions()).ToList();
                }

                grid.Bind(data);
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
                Cursor.Current = Cursors.Default; // Hata olsa bile cursor default hale gelsin
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlDrawwer.Visible = false;
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            pnlDrawwer.Visible=true;
            bindingProvider1.Bind(new Appointment());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            pnlDrawwer.Visible = true; // Eğer buton bu panelin içindeyse

            // Check validation
            if (validationProvider1.Validate().Length > 0) return;
            // Get database here - Using Kimtoo Toolkit
            var db = Kimtoo.DbManager.Connections.GetConnection();
             db.Save(bindingProvider1.Get<Appointment>());
            LoadData();
            pnlDrawwer.Visible = false;
            bunifuSnackbar1.Show(this.FindForm(), "Başarılı", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success);
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0) return;

            try
            {
                if (e.ColumnIndex == colEdit.Index)
                {
                    pnlDrawwer.Visible = true;

                    // Seçili randevu (appointment) nesnesini al
                    var appointment = grid.GetRecord<Appointment>();
                    if (appointment != null)
                    {
                        // Bağlantıyı yap
                        bindingProvider1.Bind(appointment);

                        // Combobox'ların DataSource'unu sıfırla ve yeniden yükle
                        var db = Kimtoo.DbManager.Connections.GetConnection();

                        cPatients.DataSource = null;
                        cDentists.DataSource = null;

                        cPatients.DataSource = db.Select<Patient>().ToList();
                        cDentists.DataSource = db.Select<Dentist>().ToList();

                        cPatients.DisplayMember = "İsim";  // Hasta adını göster
                        cPatients.ValueMember = "Id";      // Hasta ID'sini sakla

                        cDentists.DisplayMember = "İsim";  // Dişçi adını göster
                        cDentists.ValueMember = "Id";      // Dişçi ID'sini sakla

                        // Seçili değeri ayarla
                        cPatients.SelectedValue = appointment.PatientId;
                        cDentists.SelectedValue = appointment.DentistId;

                        // Combobox'ların BindingContext'ini sıfırla ve yenile
                        cPatients.BindingContext = new BindingContext();
                        cDentists.BindingContext = new BindingContext();

                        cPatients.Refresh();
                        cDentists.Refresh();
                    }

                    grid.Refresh();  // Grid'i yenileme işlemi
                }

                if (e.ColumnIndex == colDel.Index)
                {
                    grid.DeleteRow<Appointment>(); // Kimtoo Toolkit Feature
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().Length > 0)
                grid.SearchRows(txtSearch.Text);
            else
                LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadData();
        }
         
        private void tabCat_OnSelectionChange(object sender, EventArgs e)
        {
            LoadData();
        }

        private void datePick_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOpen.Text = grid.GetRecord<Appointment>().HasSessions()? "Tedavi Seansını Aç" : "Tedavi Seansını Oluştur";
            btnOpen.Visible = !grid.GetRecord<Appointment>().İptal;
           
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var record = grid.GetRecord<Appointment>();
            new Dental_Clinic_Management_Software.FrmTreatment(record).ShowDialog();

        }

        private void PageAppointments_Load(object sender, EventArgs e)
        {
            
        }
    }
}
