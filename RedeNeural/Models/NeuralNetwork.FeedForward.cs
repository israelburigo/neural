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
        private void FeedForward()
        {
            CalculateNeurons(Inputs);

            foreach (var hiddens in Hiddens.OrderBy(p => p.Seq))
                CalculateNeurons(hiddens);

            CalculateHiddenErrors();
        }

        private void CalculateNeurons(IEnumerable<Neuron> neurons)
        {
            foreach (var synAg in Synapses.Where(p => neurons.Any(q => q == p.Orig)).GroupBy(p => p.Dest))
            {
                var value = synAg.Sum(p => p.Weight * p.Orig.Value) + synAg.Key.Bias;
                synAg.Key.NetValue = value;
                synAg.Key.Value = Activation(value);
                synAg.Key.Error = synAg.Key.Value - synAg.Key.Expected;
            }
        }

        private void CalculateHiddenErrors()
        {
            foreach (var hiddens in Hiddens.OrderByDescending(p => p.Seq))
                foreach (var neuron in hiddens)
                    neuron.Error = Synapses.Where(p => p.Orig == neuron).Sum(p => p.Dest.Error * dActivation(p.Dest.Value) * p.Weight);
        }
    }
}
