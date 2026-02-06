using Runtime.Extensions;
using Runtime.Landscape.Grid;
using System;
using System.Collections.Generic;
using System.Linq;

public class EnvironmentGenerationDescription
{
    private int[,] _environmentMatrix;
    private Random _random;
    private Dictionary<int, float> _environmentElementsWithProbabilities;

    private int _minClusterSize;
    private int _maxClusterSize;
    private float _clusterSpawnProbability;

    public EnvironmentGenerationDescription(Dictionary<string, object> environmentGenerationData, Dictionary<string, object> environmentData)
    {
        _random = new Random();
        _environmentElementsWithProbabilities = new Dictionary<int, float>();

        foreach (var key in environmentData.Keys)
        {
            if (int.TryParse(key, out int environmentId))
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
        int height = GridConstants.Height;
        int width = GridConstants.Width;
        int[,] environmentMatrix = new int[height, width];
        List<(int x, int y, int environmentElementType)> seeds = new List<(int x, int y, int environmentElementType)>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (surfaceMatrix[i, j] > 0 && _random.NextDouble() < _clusterSpawnProbability)
                {
                    seeds.Add((i, j, GetRandomEnvironmentElement()));
                }
            }
        }

        foreach (var seed in seeds)
        {
            int clusterSize = _random.Next(_minClusterSize, _maxClusterSize + 1);

            for (int k = 0; k < clusterSize; k++)
            {
                int offsetX = _random.Next(-2, 3);
                int offsetY = _random.Next(-2, 3);
                int x = Math.Clamp(seed.x + offsetX, 0, height - 1);
                int y = Math.Clamp(seed.y + offsetY, 0, width - 1);

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
        float randomValue = (float)_random.NextDouble();

        foreach (var environmentElement in _environmentElementsWithProbabilities)
        {
            totalProbability += environmentElement.Value;
        }

        foreach (var environmentElement in _environmentElementsWithProbabilities)
        {
            float normalizedProbability = environmentElement.Value / totalProbability;
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