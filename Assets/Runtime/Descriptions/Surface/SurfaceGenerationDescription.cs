using System;
using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.Landscape.Grid;

namespace Runtime.Descriptions.Surface
{
    public class SurfaceGenerationDescription
    {
        private int[,] _landMatrix;
        private Random _random;

        private readonly int _centerLandProbability;
        private readonly int _edgeLandProbability;
        private readonly double _declineSteepness;
        private readonly double _islandMaxSizeRatio;

        public SurfaceGenerationDescription(Dictionary<string, object> data)
        {
            _centerLandProbability = data.GetInt("center_land_probability");
            _edgeLandProbability = data.GetInt("edge_land_probability");
            _declineSteepness = data.GetFloat("decline_steepness");
            _islandMaxSizeRatio = data.GetFloat("island_max_size_ratio");
        }

        public int[,] Generate()
        {
            _landMatrix = new int[GridConstants.Height, GridConstants.Width];
            _random = new Random();

            const double centerX = (GridConstants.Width - 1) / 2.0;
            const double centerY = (GridConstants.Height - 1) / 2.0;
            var maxIslandDistance = Math.Min(centerX, centerY) * _islandMaxSizeRatio;

            for (var y = 0; y < GridConstants.Height; y++)
            {
                for (var x = 0; x < GridConstants.Width; x++)
                {
                    var distanceX = x - centerX;
                    var distanceY = y - centerY;
                    var distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                    if (distance > maxIslandDistance)
                    {
                        _landMatrix[y, x] = 0;
                        continue;
                    }

                    var normalizedDistance = distance / maxIslandDistance;
                    var probability = CalculateProbability(normalizedDistance);
                    var isLand = _random.Next(0, 100) < probability ? 1 : 0;
                    _landMatrix[y, x] = isLand;
                }
            }

            return _landMatrix;
        }

        private double CalculateProbability(double normalizedDistance)
        {
            var distanceFactor = Math.Pow(normalizedDistance, _declineSteepness);
            var probability = _centerLandProbability - (_centerLandProbability - _edgeLandProbability) * distanceFactor;
            return Math.Max(0, Math.Min(100, probability));
        }
    }
}
