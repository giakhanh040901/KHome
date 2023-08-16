using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Pvcb.Configs
{
    public class PvcbConfiguration
    {
        public string PublicKey { get; set; }
        public SharedApiPvcb ApiPvcb { get; set; }
    }

    public class SharedApiPvcb
    {
        public string Pvcb { get; set; }
        public string Payment { get; set; }
        public PvcbClientCredential ClientCredential { get; set; }
    }

    public class PvcbClientCredential
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
