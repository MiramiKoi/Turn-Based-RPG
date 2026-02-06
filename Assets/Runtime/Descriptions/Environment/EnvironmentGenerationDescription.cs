using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Extensions;
using Runtime.Landscape.Grid;

namespace Runtime.Descriptions.Environment
{
    public class EnvironmentGenerationDescription
    {
        private int[,] _environmentMatrix;
        private readonly Random _random;
        private readonly Dictionary<int, float> _environmentElementsWithProbabilities;

        private readonly int _minClusterSize;
        private readonly int _maxClusterSize;
        private readonly float _clusterSpawnProbability;

        public EnvironmentGenerationDescription(Dictionary<string, object> environmentGenerationData, Dictionary<string, object> environmentData)
        {
            _random = new Random();
            _environmentElementsWithProbabilities = new Dictionary<int, float>();

            foreach (var key in environmentData.Keys)
            {
                if (int.TryParse(key, out var environmentId))
                {
                    var node = environmentData.GetNode(key);
                    var probability = node.GetFloat("spawn_probability");
                    _environmentElementsWithProbabilities[environmentId] = probability;
                }
            }

            _minClusterSize = environmentGenerationData.GetInt("min_cluster_size");
            _maxClusterSize = environmentGenerationData.GetInt("max_cluster_size");
            _clusterSpawnProbability = environmentGenerationData.GetFloat("cluster_spawn_probability");
        }

        private int[,] GenerateWithClustering(int[,] surfaceMatrix)
        {
            const int height = GridConstants.Height;
            const int width = GridConstants.Width;
            var environmentMatrix = new int[height, width];
            var seeds = new List<(int x, int y, int environmentElementType)>();

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (surfaceMatrix[i, j] > 0 && _random.NextDouble() < _clusterSpawnProbability)
                    {
                        seeds.Add((i, j, GetRandomEnvironmentElement()));
                    }
                }
            }

            foreach (var seed in seeds)
            {
                var clusterSize = _random.Next(_minClusterSize, _maxClusterSize + 1);

                for (var k = 0; k < clusterSize; k++)
                {
                    var offsetX = _random.Next(-2, 3);
                    var offsetY = _random.Next(-2, 3);
                    var x = Math.Clamp(seed.x + offsetX, 0, height - 1);
                    var y = Math.Clamp(seed.y + offsetY, 0, width - 1);

                    if (surfaceMatrix[x, y] > 0 && environmentMatrix[x, y] == 0)
                    {
                        environmentMatrix[x, y] = seed.environmentElementType;
                    }
                }
            }
            return environmentMatrix;
        }

        private int GetRandomEnvironmentElement()
        {
            var randomEnvironmentElement = _environmentElementsWithProbabilities.Keys.First();
            float totalProbability = 0;
            float currentSum = 0;
            var randomValue = (float)_random.NextDouble();

            foreach (var environmentElement in _environmentElementsWithProbabilities)
            {
                totalProbability += environmentElement.Value;
            }

            foreach (var environmentElement in _environmentElementsWithProbabilities)
            {
                var normalizedProbability = environmentElement.Value / totalProbability;
                currentSum += normalizedProbability;

                if (randomValue <= currentSum)
                {
                    randomEnvironmentElement = environmentElement.Key;
                    return randomEnvironmentElement;
                }
            }
            return randomEnvironmentElement;
        }

        public int[,] Generate(int[,] surfaceMatrix)
        {
            _environmentMatrix = new int[GridConstants.Height, GridConstants.Width];
            _environmentMatrix = GenerateWithClustering(surfaceMatrix);
            return _environmentMatrix;
        }
    }
}