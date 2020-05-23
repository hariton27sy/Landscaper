using System;
using System.Collections.Generic;
using OpenTK.Platform.Windows;

namespace SimpleGame.GameCore.Worlds
{
    public class NoiseGenerator
    {
        private int Seed { get; }
        private int Octaves { get; }
        private double Amplitude { get; }
        private double Persistence { get; }
        private double Frequency { get; }
        
        public delegate double Smooth(double x, double y);
        private readonly Smooth smooth;

        public NoiseGenerator(int seed, int octaves = 8, double amplitude = 1, double persistence = 0.65, double frequency = 0.015)
        {
            Seed = seed;
            Octaves = octaves;
            Amplitude = amplitude;
            Persistence = persistence;
            Frequency = frequency;

            smooth = CrossSmooth;
        }

        public double Noise(int x, int y)
        {
            // todo func smoothness on negative values 
            // x = Math.Abs(x);
            // y = Math.Abs(y);
            x += 1600;
            y += 1600;
            
            double total = 0.0;
            double freq = Frequency, amp = Amplitude;
            for (var i = 0; i < Octaves; ++i)
            {
                total += smooth(x * freq, y * freq) * amp;
                freq *= 2;
                amp *= Persistence;
            }
            if (total < -2.4) total = -2.4;
            else if (total > 2.4) total = 2.4;

            return (total/ 2.4);
        }

        public double NoiseGeneration(int x, int y)
        {
            int n = x + y * 57;
            n = (n << 13) ^ n;

            return (1.0 - ((n * (n * n * 15731 + 789221) + Seed) & 0x7fffffff) / 1073741824.0);
        }

        private double Interpolate(double x, double y, double a)
        {
            double value = (1 - Math.Cos(a * Math.PI)) * 0.5;
            return x * (1 - value) + y * value;
        }
        
        public double CrossSmooth(double x, double y)
        {
            var ix = (int)x;
            var iy = (int)y;
            var n1 = NoiseGeneration(ix, iy);
            var n2 = NoiseGeneration(ix + 1, iy);
            var n3 = NoiseGeneration(ix, iy + 1);
            var n4 = NoiseGeneration(ix + 1, iy + 1);

            var i1 = Interpolate(n1, n2, x - ix);
            var i2 = Interpolate(n3, n4, x - ix);

            return Interpolate(i1, i2, y - iy);
        }

        public double SquareSmooth(double x, double y)
        {
            var ix = (int)x;
            var iy = (int)y;
            var n1 = NoiseGeneration(ix - 1, iy - 1);
            var n2 = NoiseGeneration(ix - 1, iy);
            
            var n3 = NoiseGeneration(ix - 1, iy + 1);
            var n6 = NoiseGeneration(ix    , iy + 1);
            
            var n4 = NoiseGeneration(ix    , iy - 1);
            var n7 = NoiseGeneration(ix + 1, iy - 1);
            
            var n8 = NoiseGeneration(ix + 1, iy);
            var n9 = NoiseGeneration(ix + 1, iy + 1);

            var i1 = Interpolate(n1, n2, x - ix);
            var i2 = Interpolate(n3, n6, x - ix);
            var i3 = Interpolate(n4, n7, x - ix);
            var i4 = Interpolate(n8, n9, x - ix);
            
            var i11 = Interpolate(i1, i2, y - iy);
            var i12 = Interpolate(i3, i4, y - iy);
            
            
            return Interpolate(i11, i12, y - iy);
        }
    }
}