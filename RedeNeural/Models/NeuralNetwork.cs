﻿using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
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

    public class NeuralNetwork
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

        public void Train()
        {
            FeedForward();
            //BackPropagation();
        }

        public List<Synapse> BuildSynapses()
        {
            Synapses = new List<Synapse>();

            var seq = 0;
            Hiddens.ForEach(p => p.Seq = ++seq);

            var hiddens = Hiddens.OrderBy(p => p.Seq).ToList();

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
                        Weight = (float)_rand.NextDouble()
                    });
                }
            }
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

            syns.ForEach(p =>
            {
                var grad = LearningRate
                           * errorDests.First(q => q.Neuron == p.Dest).Error
                           * dActivation(p.Orig.Value);

                p.Weight -= grad;
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
                synAg.Key.Value = Activation(value);
            }
        }

        private float dActivation(float value)
        {
            switch (ActivationType)
            {
                case EnumActivation.Relu: return value < 0 ? 0 : 1;
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

        public void SetInput(int id, float value)
        {
            Inputs.Find(p => p.Id == id).Value = value;
        }

        public float GetOutput(int id)
        {
            return Outputs.Find(p => p.Id == id).Value;
        }


    }
}



