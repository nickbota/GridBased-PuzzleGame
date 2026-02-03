using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of a gem on the grid
    // Updates appearance when the gem is found

    [RequireComponent(typeof(SpriteRenderer))]
    public class GemView : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private GemData gemData;
        private bool isFound;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
        public void Initialize(GemData data)
        {
            gemData = data;
            gemData.OnGemFound += OnGemFound;
            
            if (gemData.Definition != null && gemData.Definition.Sprite != null)
                spriteRenderer.sprite = gemData.Definition.Sprite;
            
            isFound = false;
        }
        private void OnDestroy()
        {
            if (gemData != null)
                gemData.OnGemFound -= OnGemFound;
        }

        private void OnGemFound(GemData data)
        {
            if (isFound) return;
            
            isFound = true;
            ShowGemSprite();
            PlayFoundAnimation();
        }

        private void ShowGemSprite()
        {
            spriteRenderer.enabled = true;
        }
        private void PlayFoundAnimation()
        {

        }
    }
}