using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public partial class NeuralNetwork
    {
        public void BackPropagation()
        {
            CalculateSynapse(Outputs);

            foreach (var hiddens in Hiddens.OrderByDescending(p => p.Seq))
                CalculateSynapse(hiddens);
        }

        private void CalculateSynapse(IEnumerable<Neuron> neurons)
        {
            foreach (var syn in Synapses.Where(p => neurons.Any(q => q == p.Dest)))
            {
                var dEdW = syn.Dest.Error * dActivation(syn.Dest.Value) * syn.Orig.Value;
                syn.Weight = syn.Weight - LearningRate * dEdW;
            }
        }

    }
}
