using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public class Neuron
    {
        public int Id { get; set; }
        public float Expected { get; set; }

        public float Value { get; set; }
        public float NetValue { get; set; }
        public float Bias { get; set; }
        public float Error { get; set; }
    }
}
