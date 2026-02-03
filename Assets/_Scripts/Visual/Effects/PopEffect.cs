using UnityEngine;
using DG.Tweening;

namespace Visual.Effects
{
    public class PopEffect : BaseEffect
    {
        [Header("Settings")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _scale = 1.2f;
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private Ease _ease = Ease.OutBack;

        private Vector3 _originalScale;
        private Tween _currentTween;

        private void Awake()
        {
            if (_targetTransform == null)
                _targetTransform = transform;

            _originalScale = _targetTransform.localScale;
        }

        public override void Play()
        {
            Stop();

            Vector3 targetScale = _originalScale * _scale;

            _currentTween = DOTween.Sequence()
                .Append(_targetTransform.DOScale(targetScale, _duration).SetEase(_ease))
                .Append(_targetTransform.DOScale(_originalScale, _duration).SetEase(Ease.InBack))
                .SetLink(gameObject);
        }

        public override void Stop()
        {
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }

            if (_targetTransform != null)
                _targetTransform.localScale = _originalScale;
        }
    }
}
