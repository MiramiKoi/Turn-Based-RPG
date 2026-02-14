using System;
using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Locations.Environment
{
    public class EnvironmentGenerationDescription
    {
        private readonly Dictionary<string, Dictionary<string, object>> _generationRules;
        private readonly Dictionary<int, float> _environmentElementsWithProbabilities;
        private readonly Random _random;

        public EnvironmentGenerationDescription(
            Dictionary<string, object> environmentGenerationData,
            Dictionary<string, object> environmentData)
        {
            _random = new Random();
            _generationRules = new Dictionary<string, Dictionary<string, object>>();
            _environmentElementsWithProbabilities = new Dictionary<int, float>();
            
            foreach (var rule in environmentGenerationData)
            {
                var ruleName = rule.Key;
                var ruleData = (Dictionary<string, object>)rule.Value;
                _generationRules[ruleName] = ruleData;
            }
            
            foreach (var key in environmentData.Keys)
            {
                if (int.TryParse(key, out var environmentId))
                {
                    var node = environmentData.GetNode(key);
                    if (node.ContainsKey("spawn_probability"))
                    {
                        var probability = node.GetFloat("spawn_probability");
                        _environmentElementsWithProbabilities[environmentId] = probability;
                    }
                }
            }
        }

        public int[,] Generate(LocationDescription locationDescription, int[,] surfaceMatrix)
        {
            var ruleName = locationDescription.EnvironmentGenerationRules;
            _generationRules.TryGetValue(ruleName, out var ruleData);
            
            var height = surfaceMatrix.GetLength(0);
            var width = surfaceMatrix.GetLength(1);
            var environmentMatrix = new int[height, width];
            
            switch (ruleName)
            {
                case "island":
                    return GenerateWithClustering(environmentMatrix, surfaceMatrix, ruleData, locationDescription);
                case "shop":
                    return GenerateShop(environmentMatrix, surfaceMatrix, ruleData, locationDescription);
                default:
                    return GenerateEmpty(environmentMatrix, surfaceMatrix);
            }
        }

        private int[,] GenerateWithClustering(int[,] environmentMatrix, int[,] surfaceMatrix, 
            Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            var minClusterSize = ruleData.GetInt("min_cluster_size");
            var maxClusterSize = ruleData.GetInt("max_cluster_size");
            var clusterSpawnProbability = ruleData.GetFloat("cluster_spawn_probability");
            
            var height = surfaceMatrix.GetLength(0);
            var width = surfaceMatrix.GetLength(1);
            
            var seedsByBiome = new Dictionary<int, List<(int x, int y)>>();
            
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var biomeId = surfaceMatrix[i, j];
                    
                    if (biomeId > 0 && _environmentElementsWithProbabilities.ContainsKey(biomeId) && 
                        _random.NextDouble() < clusterSpawnProbability)
                    {
                        if (!seedsByBiome.ContainsKey(biomeId))
                        {
                            seedsByBiome[biomeId] = new List<(int x, int y)>();
                        }
                        seedsByBiome[biomeId].Add((i, j));
                    }
                }
            }

            foreach (var biomePair in seedsByBiome)
            {
                var biomeId = biomePair.Key;
                var seeds = biomePair.Value;
                
                foreach (var seed in seeds)
                {
                    var clusterSize = _random.Next(minClusterSize, maxClusterSize + 1);

                    for (var k = 0; k < clusterSize; k++)
                    {
                        var offsetX = _random.Next(-2, 3);
                        var offsetY = _random.Next(-2, 3);
                        var x = Math.Clamp(seed.x + offsetX, 0, height - 1);
                        var y = Math.Clamp(seed.y + offsetY, 0, width - 1);
                        
                        if (surfaceMatrix[x, y] == biomeId && environmentMatrix[x, y] == 0)
                        {
                            environmentMatrix[x, y] = biomeId;
                        }
                    }
                }
            }
            
            return environmentMatrix;
        }
        
        private int[,] GenerateShop(int[,] environmentMatrix, int[,] surfaceMatrix, 
            Dictionary<string, object> ruleData, LocationDescription locationDescription)
        {
            return environmentMatrix;
        }
        
        private int[,] GenerateEmpty(int[,] environmentMatrix, int[,] surfaceMatrix)
        {
            return environmentMatrix;
        }
    }
}