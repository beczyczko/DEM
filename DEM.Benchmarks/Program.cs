﻿using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using DEM.Engine;

namespace DEM.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summaries = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, new DebugInProcessConfig());
        }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Vector2DBenchmark
    {
        private static readonly Vector2d Vector2d = new Vector2d(1F, 2F);
        private static readonly Vector2 Vector2 = new Vector2(1F, 2F);
        private const float X2 = 3;
        private const float Y2 = 5;

        [Benchmark]
        public void ScalarOfVector2d()
        {
            var vector2dScalar = Vector2d.Scalar;
        }

        [Benchmark]
        public void ScalarOfVector2()
        {
            var vector2dScalar = Vector2.Length();
        }
    }
}