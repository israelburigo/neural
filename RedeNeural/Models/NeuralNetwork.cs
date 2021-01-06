using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public enum EnumActivation
    {
        Relu,
        Sigmoid,
        Value
    }

    public partial class NeuralNetwork
    {
        public List<Neuron> Inputs { get; set; }
        public List<HiddenNeurons> Hiddens { get; set; }
        public List<Neuron> Outputs { get; set; }
        public List<Synapse> Synapses { get; set; }
        public float LearningRate { get; set; }
        public EnumActivation ActivationType { get; set; }

        private readonly Random _rand;

        public NeuralNetwork(Random r)
        {
            _rand = r;
            ActivationType = EnumActivation.Relu;
            LearningRate = 0.05f;
            Inputs = new List<Neuron>();
            Hiddens = new List<HiddenNeurons>();
            Outputs = new List<Neuron>();
        }

        public void CalculateOutputs()
        {
            FeedForward();
        }

        public void RecalculateSynapses()
        {
            BackPropagation();
        }

        public List<Synapse> BuildSynapses()
        {
            Synapses = new List<Synapse>();

            var seq = 0;
            Hiddens.ForEach(p => p.Seq = ++seq);

            var hiddens = Hiddens.OrderBy(p => p.Seq);

            var orig = Inputs.ToList();

            var id = 0;
            foreach (var h in hiddens)
            {
                var dest = h;
                Build(orig, dest, ref id);
                orig = dest;
            }

            Build(orig, Outputs, ref id);

            return Synapses;
        }

        public void SetInput(int id, float value)
        {
            Inputs.Find(p => p.Id == id).Value = value;
        }

        public float GetOutput(int id)
        {
            return Outputs.Find(p => p.Id == id).Value;
        }

        private void Build(List<Neuron> orig, List<Neuron> dest, ref int id)
        {
            foreach (var o in orig)
            {
                foreach (var d in dest)
                {
                    Synapses.Add(new Synapse
                    {
                        Id = ++id,
                        Orig = o,
                        Dest = d,
                        Weight = (float)_rand.NextDouble() * 2 - 1
                    });
                }
            }
        }

        private float dActivation(float value)
        {
            switch (ActivationType)
            {
                case EnumActivation.Relu: return value > 0 ? 1 : 0;
                case EnumActivation.Sigmoid: return value * (1 - value);
                default: return value;
            }
        }

        private float Activation(float value)
        {
            switch (ActivationType)
            {
                case EnumActivation.Relu: return value < 0 ? 0 : value;
                case EnumActivation.Sigmoid: return (float)(1 / (1 + Math.Pow(Math.E, -value)));
                default: return value;
            }
        }

    }
}



