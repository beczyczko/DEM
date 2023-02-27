using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using DEM.Engine.CollisionSolver;
using DEM.Engine.Elements;
using DEM.Engine.Importers;

namespace DEM.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summaries = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args
                    // , new DebugInProcessConfig()
                    );
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Vector2DBenchmark
    {
        private static readonly Vector2 Vector2 = new Vector2(1F, 2F);
        private const float X2 = 3;
        private const float Y2 = 5;

        [Benchmark]
        public void ScalarOfVector2()
        {
            var vector2dScalar = Vector2.Length();
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ParticleParticleCollisionSolverBenchmark
    {
        private readonly ParticleParticleCollisionSolver _particleParticleCollisionSolver = new ParticleParticleCollisionSolver();
        private readonly Particle _particle1 = new Particle(new Vector2(2, 3), new Vector2(1, 2), 5, 1, 200);
        private readonly Particle _particle2 = new Particle(new Vector2(3, 5), new Vector2(2, 5), 5, 1, 200);

        [Benchmark]
        public void CalculateCollisionForce()
        {
            _particleParticleCollisionSolver.CalculateCollisionForce(_particle1, _particle2);
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CollisionSolverBenchmark
    {
        private readonly Particle _particle1 = new Particle(new Vector2(50, 50), new Vector2(52, 47), 25, 1, 200);

        private ICollidable[] _particles;

        [GlobalSetup]
        public void GlobalSetup()
        {
            const string filePath = "particles_triangular_random.tsv";
            const string dataSeparator = "\t";
            var particlesCsvImporter = new ParticlesImporter();
            var particles = particlesCsvImporter.Import(filePath, dataSeparator);

            _particles = particles.Select(p => p as ICollidable).ToArray();
        }

        [Benchmark]
        public void CalculateCollisionForce()
        {
            CollisionSolver.CalculateCollisionForce(_particle1, _particles);
        }
    }
}
