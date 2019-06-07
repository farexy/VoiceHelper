using System.Collections.Generic;
using VoiceHelper.Db;

namespace VoiceHelper.Models
{
    public class ProductsViewModel
    {
        public string Recognized { get; set; }
        public List<Product> Products { get; set; }
    }
}