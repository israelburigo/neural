using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public class Synapse
    {   
        public Neuron Orig { get; set; }
        public Neuron Dest { get; set; }
        public float Weight { get; set; }
    }
}
