using Model.CommandData;
using Model.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubService.Util
{
    public class BookUtil
    {
        private static BookConfig _config { get; set; }

        private static Hashtable _configDetail { get; set; }

        private static JObject _validDate { get; set; }

        private static List<ProductData> _productConfig { get; set; }

        public BookConfig _Config
        {
            get
            {
                if (_config == null)
                {
                    using (CoreContext context = new CoreContext())
                    {
                        _config = context.BookConfig.Where(b => b.StartTime <= DateTime.Now && b.EndTime >= DateTime.Now && b.Status == 1).FirstOrDefault();
                    }
                }
                return _config;
            }
        }

        public Hashtable _ConfigDetail
        {
            get
            {
                if (_configDetail == null && _config != null)
                {
                    _configDetail = JsonConvert.DeserializeObject<Hashtable>(_config.Config);
                }
                return _configDetail;
            }
        }

        public JObject _ValidDate
        {
            get
            {
                if (_validDate == null && _config != null)
                {
                    _validDate = _ConfigDetail["ValidDate"] as JObject;
                }
                return _validDate;
            }
        }

        public List<ProductData> _ProductConfig
        {
            get
            {
                if (_productConfig == null && _config != null)
                {
                    _productConfig = (_ConfigDetail["ProductConfig"] as JArray).ToObject<List<ProductData>>();
                }
                return _productConfig;
            }
            set
            {
                _productConfig = value;
            }
        }

    }
}
