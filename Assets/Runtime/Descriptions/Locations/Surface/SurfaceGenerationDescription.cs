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
            var width = ruleData.GetInt("width");
            var height = ruleData.GetInt("length");
            var surfaceMatrix = new int[height, width];

            switch (ruleName)
            {
                case "island":
                    return GenerateIsland(surfaceMatrix, width, height, ruleData, locationDescription);
                case "shop":
                    return GenerateShop(surfaceMatrix, width, height);
                default:
                    return GenerateDefault(surfaceMatrix, width, height);
            }
        }

        private int[,] GenerateIsland(int[,] surfaceMatrix, int width, int height,
            Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            var seed = ruleData.GetInt("seed");
            var random = new Random(seed);

            var centerLandProbability = ruleData.GetInt("center_land_probability");
            var edgeLandProbability = ruleData.GetInt("edge_land_probability");
            var declineSteepness = ruleData.GetFloat("decline_steepness");
            var islandMaxSizeRatio = ruleData.GetFloat("island_max_size_ratio");

            var centerX = (width - 1) / 2.0;
            var centerY = (height - 1) / 2.0;

            var maxDistanceX = centerX * islandMaxSizeRatio;
            var maxDistanceY = centerY * islandMaxSizeRatio;

            var availableBiomes = locationDescription.Surface.Where(s => s != 0).ToArray();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var distanceX = x - centerX;
                    var distanceY = y - centerY;

                    var normalizedX = distanceX / maxDistanceX;
                    var normalizedY = distanceY / maxDistanceY;

                    var normalizedDistance = Math.Sqrt(normalizedX * normalizedX + normalizedY * normalizedY);

                    if (normalizedDistance > 1)
                    {
                        surfaceMatrix[y, x] = 0;
                        continue;
                    }

                    var probability = CalculateProbability(normalizedDistance, centerLandProbability,
                        edgeLandProbability, declineSteepness);
                    var isLand = random.Next(0, 100) < probability;
                    surfaceMatrix[y, x] = isLand ? 1 : 0;
                }
            }

            var biomeCenters = new List<(int x, int y, int biomeId)>();
            var cellsPerBiome = (width * height) / (availableBiomes.Length * 100);

            for (var i = 0; i < cellsPerBiome; i++)
            {
                biomeCenters.AddRange(from biome in availableBiomes
                    let x = random.Next(0, width)
                    let y = random.Next(0, height)
                    where surfaceMatrix[y, x] != 0
                    select (x, y, biome));
            }

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

            return surfaceMatrix;
        }

        private int[,] GenerateShop(int[,] surfaceMatrix, int width, int height)
        {
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

        private int[,] GenerateDefault(int[,] surfaceMatrix, int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    surfaceMatrix[y, x] = 0;
                }
            }

            return surfaceMatrix;
        }

        private double CalculateProbability(double normalizedDistance, int centerProb, int edgeProb, double steepness)
        {
            var distanceFactor = Math.Pow(normalizedDistance, steepness);
            var probability = centerProb - (centerProb - edgeProb) * distanceFactor;
            return Math.Max(0, Math.Min(100, probability));
        }
    }
}