using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Configuration", menuName = "GridBased_Puzzle/Level Configuration")]
public class LevelConfiguration : ScriptableObject
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridWidth = 6;
    [SerializeField] private int _gridHeight = 6;

    [Header("Gem Configuration")]
    [SerializeField] private List<GemDefinition> _gemDefinitions;

    [Header("Game Rules")]
    [SerializeField] private int _lives = 3;

    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;
    public List<GemDefinition> GemDefinitions => _gemDefinitions;
    public int Lives => _lives;
}
