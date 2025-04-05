using Kimtoo.DbManager;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dental_Clinic_Management_Software.Models
{
    [AutoGenerateTable(4)]
    [Alias("billing")]
    public class Bill // public yapmadığım için hata alıyormuşum
                      // sınıfın dışarıdan erişilebilir olmasını sağlar.
                      // internal olsaydı , farklı bir projede veya farklı bir derlemede bu sınıfa doğrudan erişim sağlanamazdı.
    {
        [AutoIncrement, PrimaryKey] // Tablolarda benzersiz kayıtlar elde etmemizi sağlayan sütuna verilen addır.
        public int Id { get; set; }

        [ForeignKey(typeof(Treatment), OnDelete = "CASCADE")]//  Bir tabloda benzersiz kayıt oluşturmayı sağlayan sütunun (Yani Diğer tabloda Primary Key olan) diğer tabloda bir sütun olarak bulunmasına denir.
        public int TreatmentID { get; set; }

        public string Tanım { get; set; } = "";
        public double Tutar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
