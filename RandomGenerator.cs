using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;

namespace Fisher2
{
    public class RandomGenerator : Node
    {
        [Export]
        public bool UseSeed { get; set; }
        [Export]
        public int Seed { get; set; }

        private TRandom _random;
        private bool _initialized = false;

        public override void _Ready()
        {
            if (!_initialized)
            {
                if (UseSeed)
                {
                    InitGenerator(Seed);
                }
                else
                {
                    var gameState = GetNode<GameState>("/root/GameState");
                    InitGenerator(gameState.GameSeed);
                }
            }
        }

        public void InitGenerator()
        {
            _random = new TRandom();
        }


        public void InitGenerator(int seed)
        {
            _random = new TRandom(seed);
        }

        public float NextFloat(float min, float max)
        {
            double val = (_random.NextDouble() * (max - min) + min);
            return (float)val;
        }

        public int Next(int minX, int maxX)
        {
            return _random.Next(minX, maxX);
        }
    }
}
