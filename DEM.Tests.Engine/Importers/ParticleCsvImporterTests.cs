using DEM.Engine.Importers;
using FluentAssertions;
using Xunit;

namespace DEM.Tests.Engine.importers
{
    public class ParticleCsvImporterTests
    {
        [Fact]
        public void CanImportParticlesFromFile()
        {
            // arrange
            const string filePath = "Importers/particles_triangular_random.tsv";
            const string dataSeparator = "\t";
            var particlesCsvImporter = new ParticlesImporter();

            // act
            var particles = particlesCsvImporter.Import(filePath, dataSeparator);

            // assert
            particles.Length.Should().Be(175);
        }
    }
}