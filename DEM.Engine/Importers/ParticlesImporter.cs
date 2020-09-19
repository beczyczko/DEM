using System.IO;
using System.Linq;
using DEM.Engine.Elements;

namespace DEM.Engine.Importers
{
    public class ParticlesImporter
    {
        public Particle[] Import(string filePath, string separator)
        {
            var particles = File.ReadLines(filePath)
                .Skip(2)
                .Select(line => ParseToParticle(line, separator))
                .ToArray();

            return particles;
        }

        private Particle ParseToParticle(string line, string separator)
        {
            var values = line.Split(separator);
            var x = float.Parse(values[4]);
            var y = float.Parse(values[5]);
            var r = float.Parse(values[6]);
            var m = float.Parse(values[7]);
            var k = float.Parse(values[8]);
            var Vx = float.Parse(values[9]);
            var Vy = float.Parse(values[10]);
            var particle = new Particle(new Point2d(x, y), r, m, k, new Vector2d(Vx, Vy));
            return particle;
        }
    }
}