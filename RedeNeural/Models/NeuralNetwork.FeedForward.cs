using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public partial class NeuralNetwork
    {
        public void FeedForward()
        {
            CalculateNeurons(Inputs);

            foreach (var hiddens in Hiddens.OrderBy(p => p.Seq))
                CalculateNeurons(hiddens);
        }       

        private void CalculateNeurons(IEnumerable<Neuron> neurons)
        {
            foreach (var synAg in Synapses.Where(p => neurons.Any(q => q == p.Orig)).GroupBy(p => p.Dest))
            {
                var value = synAg.Sum(p => p.Weight * p.Orig.Value) + synAg.Key.Bias;
                synAg.Key.NetValue = value;
                synAg.Key.Value = Activation(value);                
            }
        }      
    }
}
