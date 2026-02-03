using UnityEngine;
using DG.Tweening;

namespace Visual.Effects
{
    public class GlowEffect : BaseEffect
    {
        [Header("Settings")]
        [SerializeField] private float _intensity = 1.5f;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Color _glowColor = Color.white;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Color _originalColor;
        private Tween _currentTween;

        private void Awake()
        {
            _originalColor = _spriteRenderer.color;
        }

        public override void Play()
        {
            Stop();

            if (_spriteRenderer == null)
                Debug.LogError("Sprite renderer not found!");

            Color targetColor = _glowColor * _intensity;

            _currentTween = DOTween.Sequence()
                .Append(_spriteRenderer.DOColor(targetColor, _duration))
                .Append(_spriteRenderer.DOColor(_originalColor, _duration))
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject);
        }

        public override void Stop()
        {
            if (_currentTween != null && _currentTween.IsActive())
                _currentTween.Kill();

            if (_spriteRenderer != null)
                _spriteRenderer.color = _originalColor;
        }
    }
}
