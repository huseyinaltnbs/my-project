using Kimtoo.DbManager;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dental_Clinic_Management_Software.Models
{ 
    [AutoGenerateTable(1)]
    [Alias("patients")]
    public class Patient
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string İsim { get; set; } = "";
        public string Telefon { get; set; } = "";
        public string Adres { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime DOB { get; set; } = DateTime.Now;
        public string Cinsiyet { get; set; } = "";
        public string Alerji { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
