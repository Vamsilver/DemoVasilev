using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppDemo.Classes
{
    public class ProductListModel
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Articul { get; set; }
        public string Materials { get; set; }
        public byte[] Image { get; set; }
    }

}
