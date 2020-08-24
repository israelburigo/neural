using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public class NeuralNetwork
    {
        public List<Neuron> Inputs { get; set; }
        public List<HiddenNeurons> Hiddens { get; set; }
        public List<Neuron> Outputs { get; set; }

        public List<Synapse> Synapses { get; set; }

        public float LearningRate { get; set; }

        public NeuralNetwork()
        {
            LearningRate = 0.1f;
        }

        public void Train()
        {
            FeedForward();
            BackPropagation();
        }

        public void BuildSynapses()
        {
            Synapses = new List<Synapse>();

            var hiddens = Hiddens.OrderBy(p => p.Seq).ToList();

            var orig = Inputs.ToList();

            foreach (var h in hiddens)
            {
                var dest = h;
                Build(orig, dest);
                orig = dest;
            }

            Build(orig, Outputs);
        }

        private void Build(List<Neuron> orig, List<Neuron> dest)
        {
            orig.ForEach(o =>
            {
                //weights
                var value = 1 / dest.Count;

                dest.ForEach(d =>
                {
                    Synapses.Add(new Synapse
                    {
                        Orig = o,
                        Dest = d,
                        Weight = value
                    });
                });
            });
        }


        private void BackPropagation()
        {
            var syns = Synapses.Where(p => Outputs.Any(q => q == p.Dest)).ToList();

            RecalculateSynapses(syns, Outputs);

            foreach (var hiddens in Hiddens.OrderByDescending(p => p.Seq))
            {
                syns = Synapses.Where(p => hiddens.Any(q => q == p.Dest)).ToList();
                RecalculateSynapses(syns, hiddens);
            }
        }

        private void RecalculateSynapses(List<Synapse> syns, List<Neuron> dests)
        {
            var errorDests = dests.Select(p => new
            {
                Neuron = p,
                Error = (float)Math.Pow(p.Value - p.Expected, 2) / 2
            });

         

        }

        private void FeedForward()
        {
            var syns = Synapses.Where(p => Inputs.Any(q => q == p.Orig)).ToList();
            CalculateNeuron(syns);

            foreach (var hiddens in Hiddens.OrderBy(p => p.Seq))
            {
                syns = Synapses.Where(p => hiddens.Any(q => q == p.Orig)).ToList();
                CalculateNeuron(syns);
            }
        }

        private void CalculateNeuron(List<Synapse> syns)
        {
            foreach (var synAg in syns.GroupBy(p => p.Dest))
            {
                var value = synAg.Sum(p => p.Weight * p.Orig.Value) + synAg.Key.Bias;
                synAg.Key.Value = Sigmoid(value);
            }
        }

        public float dSigmoid(float value)
        {
            return value * (1 - value);
        }

        public float Sigmoid(float value)
        {
            return (float)(1 / (1 + Math.Pow(Math.E, -value)));
        }
    }
}



