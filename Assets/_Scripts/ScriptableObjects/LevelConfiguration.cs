using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Configuration", menuName = "GridBased_Puzzle/Level Configuration")]
public class LevelConfiguration : ScriptableObject
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 6;
    [SerializeField] private int gridHeight = 6;

    [Header("Gem Configuration")]
    [SerializeField] private List<GemDefinition> gemDefinitions;

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public List<GemDefinition> GemDefinitions => gemDefinitions;
}
