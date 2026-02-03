using UnityEngine;

// ScriptableObject defining the properties of a gem
[CreateAssetMenu(fileName = "New Gem", menuName = "GridBased_Puzzle/Gem Definition")]
public class GemDefinition : ScriptableObject
{
    [SerializeField] private string _gemName;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Sprite _sprite;

    public string GemName => _gemName;
    public int Width => _width;
    public int Height => _height;
    public Sprite Sprite => _sprite;
}
