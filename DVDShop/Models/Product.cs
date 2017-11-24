using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDShop.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ProductTrailer { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
