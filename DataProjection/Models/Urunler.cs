using System;
using System.Collections.Generic;

#nullable disable

namespace DataProjection.Models
{
    public partial class Urunler
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public decimal? Fiyat { get; set; }
        public int? Stok { get; set; }
        public int? KategoriId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
