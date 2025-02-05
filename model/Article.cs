using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_BDAvancee.model
{
    internal class Article
    {
        public int id { get; set; }
        public string code {get; set;}
        public string name {get; set;}
        public string description { get; set; }
        public string brand {get; set;}
        public string category { get; set; }
        public decimal price { get; set; }
        public string imageURL { get; set; }
        public Boolean valid { get; set; }

        public Article(int id, string code, string name, string description, string brand, string category, decimal price, string urlImage, bool valid)
        {
            this.id = id;
            this.code = code;
            this.name = name;
            this.description = description;
            this.brand = brand;
            this.category = category;
            this.price = price;
            this.imageURL = urlImage;
            this.valid = valid;
        }

        public Article(string code, string name, string description, string brand, string category, decimal price, string urlImage, Boolean valid)
        {
            this.code = code;
            this.name = name;
            this.description = description;
            this.brand = brand;
            this.category = category;
            this.price = price;
            this.imageURL = urlImage;
            this.valid = valid;
        }

        public Article() { }
    }
}
