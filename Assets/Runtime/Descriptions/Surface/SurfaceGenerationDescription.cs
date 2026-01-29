using Runtime.Landscape.Grid;
using System;

namespace Runtime.Descriptions.Surface
{
    public class SurfaceGenerationDescription
    {
        private int[,] _landMatrix;
        private Random _random;

        // TODO: запихать в Description
        private int _centerLandProbability = 100;
        private int _edgeLandProbability = 0;
        private double _declineSteepness = 10.0;
        private double _islandMaxSizeRatio = 1.0;

        public int[,] Generate()
        {
            _landMatrix = new int[GridConstants.Height, GridConstants.Width];
            _random = new Random();

            double centerX = (GridConstants.Width - 1) / 2.0;
            double centerY = (GridConstants.Height - 1) / 2.0;
            double maxIslandDistance = Math.Min(centerX, centerY) * _islandMaxSizeRatio;

            for (int y = 0; y < GridConstants.Height; y++)
            {
                for (int x = 0; x < GridConstants.Width; x++)
                {
                    double distanceX = x - centerX;
                    double distanceY = y - centerY;
                    double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                    if (distance > maxIslandDistance)
                    {
                        _landMatrix[y, x] = 0;
                        continue;
                    }

                    double normalizedDistance = distance / maxIslandDistance;
                    double probability = CalculateProbability(normalizedDistance);
                    int isLand = (_random.Next(0, 100) < probability) ? 1 : 0;
                    _landMatrix[y, x] = isLand;
                }
            }

            return _landMatrix;
        }

        private double CalculateProbability(double normalizedDistance)
        {
            double distanceFactor = Math.Pow(normalizedDistance, _declineSteepness);
            double probability = _centerLandProbability - (_centerLandProbability - _edgeLandProbability) * distanceFactor;
            return Math.Max(0, Math.Min(100, probability));
        }
    }
}
