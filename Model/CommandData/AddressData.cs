using System;
using System.Collections.Generic;
using System.Text;

namespace Model.CommandData
{
    public class AddressData
    {
        public int Id { get; set; }

        public string Receiver { get; set; }

        public string Status { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string ProvinceCode { get; set; }

        public string ProvinceName { get; set; }

        public string CityCode { get; set; }

        public string CityName { get; set; }

        public string AreaCode { get; set; }

        public string AreaName { get; set; }

        public string PostCode { get; set; }
    }
}
