using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridFactory : IGridFactory
    {
        private readonly int? _seed;

        public GridFactory(int? seed = null)
        {
            _seed = seed;
        }

        public GridData CreateGrid(int width, int height)
        {
            return new GridData(width, height);
        }

        public List<GemData> PlaceGems(GridData gridData, List<GemDefinition> gemDefinitions)
        {
            if (gridData == null)
            {
                Debug.LogError("GridData is null. Cannot place gems.");
                return new List<GemData>();
            }

            if (gemDefinitions == null || gemDefinitions.Count == 0)
            {
                Debug.LogWarning("No gem definitions provided.");
                return new List<GemData>();
            }

            GridGenerator generator = new GridGenerator(gridData, _seed);
            return generator.PlaceGemsRandomly(gemDefinitions);
        }
    }
}
