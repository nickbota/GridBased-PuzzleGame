using UnityEngine;

// ScriptableObject defining the properties of a gem
[CreateAssetMenu(fileName = "New Gem", menuName = "GridBased_Puzzle/Gem Definition")]
public class GemDefinition : ScriptableObject
{
    [SerializeField] private string gemName;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Sprite sprite;

    public string GemName => gemName;
    public int Width => width;
    public int Height => height;
    public Sprite Sprite => sprite;
}
