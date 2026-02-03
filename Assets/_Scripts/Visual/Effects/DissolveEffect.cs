using UnityEngine;
using DG.Tweening;

namespace Visual.Effects
{
    public class DissolveEffect : BaseEffect
    {
        [Header("Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InBack;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Tween _currentTween;
        private Vector3 _originalScale;
        private float _originalAlpha;

        private void Awake()
        {
            _originalScale = transform.localScale;
            _originalAlpha = _spriteRenderer.color.a;
        }

        public override void Play()
        {
            Stop();

            if (_spriteRenderer == null)
                Debug.LogError("Sprite renderer not found!");

            _currentTween = DOTween.Sequence()
                .Join(_spriteRenderer.DOFade(0f, _duration))
                .SetEase(_ease)
                .OnComplete(() => gameObject.SetActive(false))
                .SetLink(gameObject);
        }
        public override void Stop()
        {
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }

            if (_spriteRenderer != null)
            {
                Color color = _spriteRenderer.color;
                color.a = _originalAlpha;
                _spriteRenderer.color = color;
            }
        }
    }
}