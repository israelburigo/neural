using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    [Serializable]
    public class Synapse
    {
        public int Id { get; set; }        
        public Neuron Orig { get; set; }        
        public Neuron Dest { get; set; }
        public float Weight { get; set; }
    }
}
