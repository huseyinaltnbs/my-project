using Kimtoo.DbManager;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dental_Clinic_Management_Software.Models
{
    [AutoGenerateTable(3)]
    [Alias("treatments")]
    public class Treatment
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [ForeignKey(typeof(Appointment), OnDelete = "CASCADE")]
        public int AppointmentID { get; set; }
        public string CosultationNote { get; set; } = "";
        public string Prescriptions { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Appointment GetAppointment()
          => Connections.GetConnection().SingleById<Appointment>(this.AppointmentID);
        public Dentist GetDentist()
          => GetAppointment().GetDentist();
        public Patient GetPatient()
          => GetAppointment().GetPatient();
        public List<Bill> GetBilling() // public class Bill -- public yapmadığım için hata alıyormuşum
          => Connections.GetConnection().Select<Bill>(r => r.TreatmentID == this.Id);
    }
}
