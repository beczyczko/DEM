using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DEM.Engine.Elements;

namespace DEM.Engine
{
    public class World
    {
        public static float ParticlesBounceFactor = 1; //todo db make it not static
        public static Vector2 StandardGravity = Vector2.UnitY * 0.0981f;

        public float CurrentTime { get; }
        public Vector2 Gravity { get; }
        public Particle[] Particles { get; }
        public RigidWall[] RigidWalls { get; }

        /// <summary>
        /// Ek - Kinetic energy - sum for all particles
        /// </summary>
        public float Ek => Particles.Sum(p => p.Ek);

        public World(Particle[] particles, RigidWall[] rigidWalls, float currentTime, Vector2 gravity)
        {
            CurrentTime = currentTime;
            Gravity = gravity;
            Particles = particles;
            RigidWalls = rigidWalls;
        }

        public World ProcessNextStep(float timeStep)
        {
            var currentParticles = Particles;

            ////////////////////////
            // var elementsToSectionsGrouper = new ElementsToSectionsGrouper();

            // var collidableElements = currentParticles
            //     .Select(p => p as ICollidable)
            //     .Concat(RigidWalls.Select(w => w as ICollidable))
            //     .ToArray();

            // var groupElementsIntoSections = elementsToSectionsGrouper.GroupElementsIntoSections(collidableElements);
            // GetDistinctCollisions(groupElementsIntoSections);
            ////////////////////////


            var particlesNewState = currentParticles.ToArray(); //copy

            var restoringForces = RestoringForceCalc(currentParticles, RigidWalls);
            //todo db Cohesion - particles <--> particle
            //todo db Cohesion - rigid line <--> particle

            ApplyForcesToParticles(ref particlesNewState, restoringForces, timeStep);

            if (Gravity != Vector2.Zero)
            {
                ApplyGravityForcesToParticles(ref particlesNewState, timeStep);
            }

            MoveParticles(ref particlesNewState, timeStep);

            var rigidWallsNewState = RigidWalls.ToArray(); //copy

            var worldSnapshot = new World(particlesNewState, rigidWallsNewState, CurrentTime + timeStep, Gravity);
            return worldSnapshot;
        }

        private List<(ICollidable element1, ICollidable element2)> GetDistinctCollisions(
            (WorldSection[,] sections, int maxXIndex, int maxYIndex) groupedElements)
        {
            var distinctCollisions = new List<(ICollidable element1, ICollidable element2)>();

            for (int x = 0; x <= groupedElements.maxXIndex; x++)
            {
                for (int y = 0; y <= groupedElements.maxYIndex; y++)
                {
                    var collidableElements = groupedElements.sections[x, y].CollidableElements;
                    var possibleCollisions = collidableElements
                        .SelectMany(ce => collidableElements, (collidable1, collidable2) => (collidable1, collidable2))
                        .Where(tuple => !tuple.collidable1.Equals(tuple.collidable2));

                    foreach (var possibleCollision in possibleCollisions)
                    {
                        //todo db i think this do not work
                        if (!distinctCollisions.Contains(possibleCollision))
                        {
                            distinctCollisions.Add(possibleCollision);
                        }
                    }
                }
            }

            return distinctCollisions;
        }

        private void ApplyForcesToParticles(ref Particle[] particles, Vector2[] forces, float timeStep)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].ApplyForce(forces[i], timeStep);
            }
        }

        private void ApplyGravityForcesToParticles(ref Particle[] particles, float timeStep)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].ApplyForce(Gravity, timeStep);
            }
        }

        public Vector2[] RestoringForceCalc(Particle[] particles, RigidWall[] rigidWalls)
        {
            var collidableElements = particles
                .Select(p => p as ICollidable)
                .Concat(rigidWalls.Select(w => w as ICollidable))
                .ToArray();

            var forces = new Vector2[particles.Length];
            for (var i = 0; i < particles.Length; i++)
            {
                forces[i] = particles[i].CalculateCollisionForce(collidableElements);
            }

            return forces;
        }

        private void MoveParticles(ref Particle[] particles, float timeStep)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                particles[i].Move(timeStep);
            }
        }
    }

    // public class ElementsToSectionsGrouper
    // {
    //     public (WorldSection[,] sections, int maxXIndex, int maxYIndex) GroupElementsIntoSections(ICollidable[] collidableElements)
    //     {
    //         var elementBoundaries = collidableElements.Select(c => c.Boundary).ToArray();
    //         var worldBoundary = new Boundary(
    //             elementBoundaries.Min(b => b.Top),
    //             elementBoundaries.Max(b => b.Bottom),
    //             elementBoundaries.Min(b => b.Left),
    //             elementBoundaries.Max(b => b.Right)
    //             );
    //
    //         var maxParticleSize = 50f; //todo db hardcored, need to calc it somehow
    //         var distanceBetweenSections = maxParticleSize * 2; //todo db need to specify better factor than just 2
    //         var horizontalSectionsCount = (int)Math.Ceiling((worldBoundary.Right - worldBoundary.Left) / distanceBetweenSections) + 1;
    //         var verticalSectionsCount = (int)Math.Ceiling((worldBoundary.Bottom - worldBoundary.Top) / distanceBetweenSections) + 1;
    //         var worldSections = new WorldSection[horizontalSectionsCount, verticalSectionsCount];
    //
    //         var sectionMaxXIndex = horizontalSectionsCount - 1;
    //         var sectionMaxYIndex = verticalSectionsCount - 1;
    //
    //         for (int x = 0; x < horizontalSectionsCount; x++)
    //         {
    //             for (int y = 0; y < verticalSectionsCount; y++)
    //             {
    //                 worldSections[x, y] = new WorldSection(
    //                     distanceBetweenSections,
    //                     1.25f,
    //                     x,
    //                     y,
    //                     new Vector2(worldBoundary.Left, worldBoundary.Top)
    //                     );
    //             }
    //         }
    //
    //         for (int i = 0; i < collidableElements.Length; i++)
    //         {
    //             var element = collidableElements[i];
    //
    //             var sectionsThatFit = new List<WorldSection>();
    //             if (element is Particle particle)
    //             {
    //                 var closestSectionPositionIndex = particle.Position / distanceBetweenSections + new Vector2(0.5f);
    //                 var indexX = (int)Math.Floor(closestSectionPositionIndex.X);
    //                 var indexY = (int)Math.Floor(closestSectionPositionIndex.Y);
    //
    //                 var coordinatesToCheck = new List<(int x, int y)>();
    //                 coordinatesToCheck.Add((indexX, indexY));
    //                 if (indexX > 0 && indexY > 0)
    //                 {
    //                     coordinatesToCheck.Add((indexX - 1, indexY - 1));
    //                 }
    //                 if (indexY > 0)
    //                 {
    //                     coordinatesToCheck.Add((indexX, indexY - 1));
    //                 }
    //                 if (indexX < sectionMaxXIndex && indexY > 0)
    //                 {
    //                     coordinatesToCheck.Add((indexX + 1, indexY - 1));
    //                 }
    //
    //                 if (indexX > 0)
    //                 {
    //                     coordinatesToCheck.Add((indexX - 1, indexY));
    //                 }
    //                 if (indexX < sectionMaxXIndex)
    //                 {
    //                     coordinatesToCheck.Add((indexX + 1, indexY));
    //                 }
    //
    //                 if (indexX > 0 && indexY < sectionMaxYIndex)
    //                 {
    //                     coordinatesToCheck.Add((indexX - 1, indexY + 1));
    //                 }
    //                 if (indexY < sectionMaxYIndex)
    //                 {
    //                     coordinatesToCheck.Add((indexX, indexY + 1));
    //                 }
    //                 if (indexX < sectionMaxXIndex && indexY < sectionMaxYIndex)
    //                 {
    //                     coordinatesToCheck.Add((indexX + 1, indexY + 1));
    //                 }
    //
    //                 //todo db problem when particle is moving too far OutOfBoundaryException happend
    //                 var sections = coordinatesToCheck
    //                     .Where(c => worldSections[c.x, c.y].Boundary.IsInside(particle.Position))
    //                     .Select(c => worldSections[c.x, c.y])
    //                     .ToArray();
    //                 sectionsThatFit.AddRange(sections);
    //             }
    //             else if (element is RigidWall rigidWall)
    //             {
    //                 //todo db for now collision with walls in checked for every section
    //                 for (int x = 0; x < horizontalSectionsCount; x++)
    //                 {
    //                     for (int y = 0; y < verticalSectionsCount; y++)
    //                     {
    //                         sectionsThatFit.Add(worldSections[x, y]);
    //                     }
    //                 }
    //             }
    //
    //             foreach (var worldSection in sectionsThatFit)
    //             {
    //                 worldSection.AddElement(element);
    //             }
    //         }
    //         //todo db assign elements to sections
    //
    //         return (worldSections, sectionMaxXIndex, sectionMaxYIndex);
    //     }
    // }

    public class WorldSection
    {
        public int XIndex { get; }
        public int YIndex { get; }
        public Boundary Boundary { get; }

        public List<ICollidable> CollidableElements { get; } = new List<ICollidable>();

        public WorldSection(
            float distanceBetweenSections,
            float sectionsOverlap,
            int xIndex,
            int yIndex,
            Vector2 section00CenterPosition)
        {
            XIndex = xIndex;
            YIndex = yIndex;
            var sectionCenterPosition = section00CenterPosition + new Vector2(xIndex, yIndex) * distanceBetweenSections;

            var sectionSize = distanceBetweenSections * sectionsOverlap;
            Boundary = new Boundary(
                sectionCenterPosition.Y - sectionSize / 2,
                sectionCenterPosition.Y + sectionSize / 2,
                sectionCenterPosition.X - sectionSize / 2,
                sectionCenterPosition.X + sectionSize / 2);
        }

        public void AddElement(ICollidable collidable)
        {
            CollidableElements.Add(collidable);
        }
    }
}