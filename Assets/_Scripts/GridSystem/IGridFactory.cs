using System.Collections.Generic;

namespace GridSystem
{
    public interface IGridFactory
    {
        GridData CreateGrid(int width, int height);
        List<GemData> PlaceGems(GridData gridData, List<GemDefinition> gemDefinitions);
    }
}
