using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Extensions;
using Random = System.Random;

namespace Runtime.Descriptions.Locations.Surface
{
    public class SurfaceGenerationDescription
    {
        private readonly Dictionary<string, Dictionary<string, object>> _generationRules;

        public SurfaceGenerationDescription(Dictionary<string, object> data)
        {
            _generationRules = new Dictionary<string, Dictionary<string, object>>();

            foreach (var rule in data)
            {
                var ruleName = rule.Key;
                var ruleData = (Dictionary<string, object>)rule.Value;
                _generationRules[ruleName] = ruleData;
            }
        }

        public int[,] Generate(LocationDescription locationDescription)
        {
            var ruleName = locationDescription.SurfaceGenerationRules;
            _generationRules.TryGetValue(ruleName, out var ruleData);

            switch (ruleName)
            {
                case "island":
                    return GenerateIsland(ruleData, locationDescription);
                case "shop":
                    return GenerateShop(ruleData, locationDescription);
                default:
                    return GenerateDefault(ruleData, locationDescription);
            }
        }

        private int[,] GenerateIsland(Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            var width = ruleData.GetInt("width");
            var height = ruleData.GetInt("length");
            var seed = ruleData.GetInt("seed");

            var random = new Random(seed);
            var surfaceMatrix = new int[height, width];

            surfaceMatrix = GenerateIslandTerrain(surfaceMatrix, width, height, ruleData, random);
            surfaceMatrix = AssignBiomesToIsland(surfaceMatrix, width, height, ruleData, locationDescription, random);

            return surfaceMatrix;
        }

        private int[,] GenerateIslandTerrain(int[,] surfaceMatrix, int width, int height,
            Dictionary<string, object> ruleData, Random random)
        {
            var baseShapeFactor = ruleData.GetFloat("base_shape_factor");
            var erosionIntensity = ruleData.GetFloat("erosion_intensity");
            var erosionRadius = ruleData.GetInt("erosion_radius");
            var erosionDepth = ruleData.GetInt("erosion_depth");
            var smoothingIterations = ruleData.GetInt("smoothing_iterations");

            surfaceMatrix = CreateBaseShape(surfaceMatrix, height, width, baseShapeFactor);
            surfaceMatrix = ApplyErosion(surfaceMatrix, height, width, random, erosionDepth, erosionRadius,
                erosionIntensity);
            surfaceMatrix = SmoothEdges(surfaceMatrix, height, width, smoothingIterations);

            return surfaceMatrix;
        }

        private int[,] CreateBaseShape(int[,] surfaceMatrix, int height, int width, float baseShapeFactor)
        {
            var centerX = width / 2.0;
            var centerY = height / 2.0;

            var radiusX = (width / 2.0) * baseShapeFactor;
            var radiusY = (height / 2.0) * baseShapeFactor;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var dx = (x - centerX) / radiusX;
                    var dy = (y - centerY) / radiusY;

                    var distanceSquared = dx * dx + dy * dy;

                    if (distanceSquared <= 1.0)
                    {
                        surfaceMatrix[y, x] = 1;
                    }
                    else
                    {
                        surfaceMatrix[y, x] = 0;
                    }
                }
            }

            return surfaceMatrix;
        }

        private int[,] ApplyErosion(int[,] surfaceMatrix, int height, int width, Random random, int erosionDepth,
            int erosionRadius, float erosionIntensity)
        {
            int[,] originalMatrix = (int[,])surfaceMatrix.Clone();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (originalMatrix[y, x] == 1)
                    {
                        var distanceToWater = GetDistanceToWater(x, y, height, width, originalMatrix);

                        if (distanceToWater <= erosionDepth)
                        {
                            var depthFactor = 1.0 - (distanceToWater / erosionDepth);
                            var erosionChance = depthFactor * erosionIntensity;

                            if (random.NextDouble() < erosionChance)
                            {
                                for (var dy = -erosionRadius; dy <= erosionRadius; dy++)
                                {
                                    for (var dx = -erosionRadius; dx <= erosionRadius; dx++)
                                    {
                                        var nx = x + dx;
                                        var ny = y + dy;

                                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                                        {
                                            var distanceFromCenter = Math.Sqrt(dx * dx + dy * dy);

                                            if (distanceFromCenter <= erosionRadius)
                                            {
                                                surfaceMatrix[ny, nx] = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return surfaceMatrix;
        }

        private double GetDistanceToWater(int x, int y, int height, int width, int[,] matrix)
        {
            var maxSearchDistance = Math.Max(width, height);

            for (var d = 1; d < maxSearchDistance; d++)
            {
                for (var dy = -d; dy <= d; dy++)
                {
                    for (var dx = -d; dx <= d; dx++)
                    {
                        var nx = x + dx;
                        var ny = y + dy;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (matrix[ny, nx] == 0)
                            {
                                return Math.Sqrt(dx * dx + dy * dy);
                            }
                        }
                    }
                }
            }

            return maxSearchDistance;
        }

        private int[,] SmoothEdges(int[,] surfaceMatrix, int height, int width, int smoothingIterations)
        {
            for (var iter = 0; iter < smoothingIterations; iter++)
            {
                int[,] newMatrix = new int[height, width];

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var neighbors = CountLandNeighbors(x, y, surfaceMatrix, height, width);

                        if (surfaceMatrix[y, x] == 1)
                        {
                            newMatrix[y, x] = (neighbors >= 3) ? 1 : 0;
                        }
                        else
                        {
                            newMatrix[y, x] = (neighbors >= 5) ? 1 : 0;
                        }
                    }
                }

                surfaceMatrix = newMatrix;
            }

            return surfaceMatrix;
        }

        private int CountLandNeighbors(int x, int y, int[,] surfaceMatrix, int height, int width)
        {
            int count = 0;

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        if (surfaceMatrix[ny, nx] == 1)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private int[,] AssignBiomesToIsland(int[,] surfaceMatrix, int width, int height,
            Dictionary<string, object> ruleData, LocationDescription locationDescription, Random random)
        {
            var availableBiomes = locationDescription.Surface.Where(s => s != 0).ToArray();

            var biomesCount = ruleData.GetInt("biomes_count");
            var smoothingIterations = ruleData.GetInt("biome_smoothing_iterations");

            var biomeCenters = GenerateBiomeCenters(surfaceMatrix, width, height, availableBiomes,
                random, biomesCount);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (surfaceMatrix[y, x] == 0) continue;

                    var minDistance = double.MaxValue;
                    var closestBiome = availableBiomes[0];

                    foreach (var center in biomeCenters)
                    {
                        var distance = Math.Sqrt(Math.Pow(x - center.x, 2) + Math.Pow(y - center.y, 2));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestBiome = center.biomeId;
                        }
                    }

                    surfaceMatrix[y, x] = closestBiome;
                }
            }

            surfaceMatrix = SmoothBiomes(surfaceMatrix, width, height, smoothingIterations);

            return surfaceMatrix;
        }

        private int[,] SmoothBiomes(int[,] matrix, int width, int height, int iterations)
        {
            for (var iter = 0; iter < iterations; iter++)
            {
                var newMatrix = (int[,])matrix.Clone();

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (matrix[y, x] == 0) continue;

                        var currentBiome = matrix[y, x];
                        var biomeCounts = new Dictionary<int, int>();
                        var sameBiomeCount = 0;

                        for (var dy = -1; dy <= 1; dy++)
                        {
                            for (var dx = -1; dx <= 1; dx++)
                            {
                                if (dx == 0 && dy == 0) continue;

                                var nx = x + dx;
                                var ny = y + dy;

                                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                                {
                                    var neighborBiome = matrix[ny, nx];
                                    if (neighborBiome != 0)
                                    {
                                        if (!biomeCounts.ContainsKey(neighborBiome))
                                            biomeCounts[neighborBiome] = 0;
                                        biomeCounts[neighborBiome]++;

                                        if (neighborBiome == currentBiome)
                                            sameBiomeCount++;
                                    }
                                }
                            }
                        }

                        if (sameBiomeCount >= 4)
                            continue;
                        if (biomeCounts.Count > 0 && biomeCounts.Values.Max() >= 3)
                        {
                            newMatrix[y, x] = biomeCounts.OrderByDescending(kv => kv.Value).First().Key;
                        }
                    }
                }

                matrix = newMatrix;
            }

            return matrix;
        }

        private List<(int x, int y, int biomeId)> GenerateBiomeCenters(int[,] surfaceMatrix,
            int width, int height, int[] availableBiomes, Random random, int biomesCount)
        {
            var biomeCenters = new List<(int x, int y, int biomeId)>();
            var centersPerBiome = biomesCount;

            for (var i = 0; i < centersPerBiome; i++)
            {
                foreach (var biome in availableBiomes)
                {
                    for (var attempts = 0; attempts < 100; attempts++)
                    {
                        var x = random.Next(0, width);
                        var y = random.Next(0, height);

                        if (surfaceMatrix[y, x] != 0)
                        {
                            biomeCenters.Add((x, y, biome));
                            break;
                        }
                    }
                }
            }

            return biomeCenters;
        }


        private int[,] GenerateShop(Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            var width = ruleData.GetInt("width");
            var height = ruleData.GetInt("length");
            var seed = ruleData.GetInt("seed");

            var random = new Random(seed);
            var surfaceMatrix = new int[height, width];
            const int defaultSurface = 5;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    surfaceMatrix[y, x] = defaultSurface;
                }
            }

            return surfaceMatrix;
        }

        private int[,] GenerateDefault(Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            var width = ruleData.GetInt("width");
            var height = ruleData.GetInt("length");
            var seed = ruleData.GetInt("seed");

            var random = new Random(seed);
            var surfaceMatrix = new int[height, width];

            return surfaceMatrix;
        }
    }
}