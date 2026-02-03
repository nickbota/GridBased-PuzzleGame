using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of a gem on the grid
    // Updates appearance when the gem is found
    public class GemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private GemData gemData;
        private bool isFound;

        public void Initialize(GemData data)
        {
            if (spriteRenderer == null)
                Debug.LogError("Sprite Renderer not found!");

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
            PlayFoundAnimation();
        }
        private void PlayFoundAnimation()
        {

        }
    }
}