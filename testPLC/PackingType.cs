using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testPLC
{   
    [Serializable]
    public class PackingType
    {
        public string PackerNo { get; set; }
        public string PlateNo { get; set; }
        public string ImageNo { get; set; }
        public string CertificateNo { get; set; }
        public string Name { get; set; }
        public List<int> Item { get; set; }
    }
}
