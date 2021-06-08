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
            CalculateErrors();

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

        private void CalculateErrors()
        {
            Outputs.ForEach(p =>
            {
                p.Error = p.Value - p.Expected;
            });

            CalculateHiddenErrors();
        }

        private void CalculateHiddenErrors()
        {
            foreach (var hiddens in Hiddens.OrderByDescending(p => p.Seq))
                foreach (var neuron in hiddens)
                    neuron.Error = Synapses.Where(p => p.Orig == neuron).Sum(p => p.Dest.Error * dActivation(p.Dest.Value) * p.Weight);
        }

    }
}
