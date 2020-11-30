using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class Product
    {

        private int _id;

        private string _ebayId;

        private string _name;

        private double _price;

        private string _description;

        private string _img_path;

        private int _quantity;

        public Product(int id, string ebayId, string name, double price, string desc, string img)
        {
            _id = id;
            _ebayId = ebayId;
            _name = name;
            _price = price;
            _description = desc;
            _img_path = img;
            _quantity = 1;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetEbayId()
        {
            return _ebayId;
        }

        public string GetName()
        {
            return _name;
        }

        public double GetPrice()
        {
            return _price;
        }

        public string GetDescription()
        {
            return _description;
        }

        public string GetImgPath()
        {
            return _img_path;
        }

        public int GetQuantity()
        {
            return _quantity;
        }

        public void SetQuantity(int newquant)
        {
            _quantity = newquant;
        }
    }
}