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
namespace Dental_Clinic_Management_Software
{
    public partial class FrmTreatment : Form
    {
        private  Treatment _treatment = null;
        public FrmTreatment(Appointment appointment)
        {
            InitializeComponent();
            try
            {
                this._treatment = appointment.GetTreatment();

                // Eğer _treatment null dönerse, yeni bir Treatment nesnesi oluşturulabilir
                if (_treatment == null)
                {
                    // Yeni bir tedavi oluşturuluyor
                    _treatment = new Treatment
                    {
                        AppointmentID = appointment.Id,
                    };

                    // Veritabanına kaydetme işlemi
                    Connections.GetConnection().Save(_treatment);

                    // Faturaların kaydedilmesi
                    Connections.GetConnection().Save(new Bill
                    {
                        TreatmentID = _treatment.Id,
                        Tanım = "Randevu Ücreti"
                    });
                    Connections.GetConnection().Save(new Bill
                    {
                        TreatmentID = _treatment.Id,
                        Tanım = "Danışman Ücreti"
                    });
                    Connections.GetConnection().Save(new Bill
                    {
                        TreatmentID = _treatment.Id,
                        Tanım = "Reçeteler"
                    });
                    Connections.GetConnection().Save(new Bill
                    {
                        TreatmentID = _treatment.Id,
                        Tanım = "Tedavi Ücreti"
                    });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıyı bilgilendirebilirsiniz
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Hata hakkında daha fazla bilgi edinmek için log yazabilirsiniz
                Console.WriteLine($"Hata: {ex.ToString()}");
            }


            bindingProvider1.Bind(_treatment);
            grid.Bind(_treatment.GetBilling());
            lblTot.Text = $"Toplam Tutar: {_treatment.GetBilling().Sum(r => r.Tutar).ToString("N2")} ₺";
            //kimtoo toolkit crud operations
            grid.OnDelete<Bill>((a, b) => Connections.GetConnection().Delete(a) >= 0);
            grid.OnEdit<Bill>((a, b) =>
            {
                Connections.GetConnection().Save(a);
                lblTot.Text = $"Toplam Tutar: {_treatment.GetBilling().Sum(r => r.Tutar).ToString("N2")} ₺";
                return true;
            });
            grid.OnError<Bill>((a, b) => { /** do nothing **/});

            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Connections.GetConnection().Save(_treatment);
            bunifuSnackbar1.Show(this.FindForm(), "Saved", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success); 
        }

        // Mouse control
        private bool mouseDown;
        private Point lastLocation;
        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grid.Bind(_treatment.GetBilling());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            grid.Bind(new Bill() { TreatmentID = _treatment.Id}, 1);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            grid.DeleteRow<Bill>();
            lblTot.Text = $"Toplam Tutar: {_treatment.GetBilling().Sum(r => r.Tutar).ToString("N2")} ₺";
        }


    }
}
