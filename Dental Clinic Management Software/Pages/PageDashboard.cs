using Dental_Clinic_Management_Software.Models;
using Kimtoo.BindingProvider;
using Kimtoo.DbManager;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dental_Clinic_Management_Software.Pages
{
    public partial class PageDashboard : UserControl
    {
        public PageDashboard()
        {
            if (this.IsInDesignMode()) return;
            InitializeComponent();
        }
        public void LoadData()
        {
            var db = Connections.GetConnection();

            var appointments = db.Select<Appointment>();
            var patients = db.Select<Patient>();
            var dentists = db.Select<Dentist>();
            var billing = db.Select<Bill>();

            // Labelları Güncelle
            lblAppointments.Text = appointments.Count.ToString("N0");
            lblPatients.Text = patients.Count.ToString("N0");
            lblDentists.Text = dentists.Count.ToString("N0");
            lblBilling.Text = billing.Sum(r => r.Tutar).ToString("N2") + " ₺";

            lblActive.Text = appointments.Where(r => !r.HasSessions() && !r.İptal && (r.Tarih > DateTime.Now || r.Tarih.Date == DateTime.Today)).Count().ToString("N0");
            lblComplete.Text = appointments.Where(r => r.HasSessions()).Count().ToString("N0");
            lblCancalled.Text = appointments.Where(r => r.İptal).Count().ToString("N0");

            grid1.Bind(patients.OrderByDescending(r => r.CreatedAt).Take(20));
            grid2.Bind(billing.OrderByDescending(r => r.CreatedAt).Take(20));

        }
        private void PageDashboard_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        

    }
}
