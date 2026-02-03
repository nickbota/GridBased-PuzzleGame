using UnityEngine;
using GridSystem;
using Visual.Effects;

namespace Visual
{
    // Responsible for visual representation of a single gem
    public class GemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private EffectController _effectController;
        
        private GemData _gemData;
        private bool _isFound;

        public void Initialize(GemData data)
        {
            if (_spriteRenderer == null)
                Debug.LogError("Sprite Renderer not found!");

            if (_effectController == null)
                Debug.LogError("Effect controller not found!");

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

        public void OnGemClicked()
        {
            if (_isFound || _effectController == null) return;
            
            _effectController.PlayEffect(EffectType.Click);
        }

        private void OnGemFound(GemData data)
        {
            if (_isFound) return;
            
            _isFound = true;
            PlayFoundAnimation();
        }

        private void PlayFoundAnimation()
        {
            if (_effectController != null)
            {
                _effectController.PlayEffect(EffectType.Found);
            }
        }
    }
}