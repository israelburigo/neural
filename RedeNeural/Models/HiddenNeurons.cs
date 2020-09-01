using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public class HiddenNeurons : List<Neuron>
    {
        public int Seq { get; set; }

        public HiddenNeurons(int qNeurons)
        {
            while (qNeurons-- > 0)
                Add(new Neuron());
        }
    }
}
