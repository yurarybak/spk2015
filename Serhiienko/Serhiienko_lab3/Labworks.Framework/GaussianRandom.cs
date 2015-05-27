using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labworks.Framework
{
    public sealed class GaussianRandom
    {
        private static readonly Lazy<GaussianRandom> lazy = new Lazy<GaussianRandom>(() => new GaussianRandom(new Random()));
        public static GaussianRandom Instance { get { return lazy.Value; } }

        private bool _hasDeviate;
        private double _storedDeviate;
        private readonly Random _random;

        public GaussianRandom(Random random = null)
        {
            _random = random ?? new Random();
        }

        public double NextGaussian(double mu = 0, double sigma = 1)
        {
            if (sigma <= 0)
                throw new ArgumentOutOfRangeException("sigma", "Must be greater than zero.");

            if (_hasDeviate)
            {
                _hasDeviate = false;
                return _storedDeviate * sigma + mu;
            }

            double v1, v2, rSquared;
            do
            {
                v1 = 2 * _random.NextDouble() - 1;
                v2 = 2 * _random.NextDouble() - 1;
                rSquared = v1 * v1 + v2 * v2;
            } while (rSquared >= 1 || rSquared == 0);

            var polar = Math.Sqrt(-2 * Math.Log(rSquared) / rSquared);
            _storedDeviate = v2 * polar;
            _hasDeviate = true;
            return v1 * polar * sigma + mu;
        }
    }
}
