using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of a gem on the grid
    // Updates appearance when the gem is found
    public class GemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private GemData _gemData;
        private bool _isFound;

        public void Initialize(GemData data)
        {
            if (_spriteRenderer == null)
                Debug.LogError("Sprite Renderer not found!");

            _gemData = data;
            _gemData.OnGemFound += OnGemFound;

            if (_gemData.Definition != null && _gemData.Definition.Sprite != null)
                _spriteRenderer.sprite = _gemData.Definition.Sprite;
            
            _isFound = false;
        }
        private void OnDestroy()
        {
            if (_gemData != null)
                _gemData.OnGemFound -= OnGemFound;
        }

        private void OnGemFound(GemData data)
        {
            if (_isFound) return;
            
            _isFound = true;
            PlayFoundAnimation();
        }
        private void PlayFoundAnimation()
        {

        }
    }
}