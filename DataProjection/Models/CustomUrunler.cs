using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataProjection.Models
{
    public partial class Urunler
    {
        [NotMapped]//veri tabanında karşılığı yok anlamında kullanılır.
        public string EncrypedId { get; set; }
    }
}
