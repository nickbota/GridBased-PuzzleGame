using System;
using UnityEngine;
using GridSystem;
using Visual.Effects;

namespace Visual
{
    // Responsible for visual representation of a single cell in the grid
    // Updates appearance based on the CellData state
    public class CellView : MonoBehaviour
    {
        public event Action<int, int> OnCellClicked;
        
        [Header("Visual References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _coveredSprite;
        [SerializeField] private Sprite _revealedSprite;

        [Header("Effects")]
        [SerializeField] private EffectController _effectController;

        private CellData _cellData;

        public void Initialize(CellData data)
        {
            if (_spriteRenderer == null)
                Debug.LogError("Sprite Renderer not found!");

            if (_cellData != null)
                _cellData.OnStateChanged -= OnCellStateChanged;

            _cellData = data;
            _cellData.OnStateChanged += OnCellStateChanged;
            
            UpdateVisuals();
        }

        private void OnDestroy()
        {
            if (_cellData != null)
                _cellData.OnStateChanged -= OnCellStateChanged;
        }

        private void OnCellStateChanged(CellData data)
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_cellData == null || _spriteRenderer == null) return;

            switch(_cellData.State)
            {
                case CellData.CellState.Covered:
                    _spriteRenderer.sprite = _coveredSprite;
                    _spriteRenderer.sortingOrder = 99;
                    break;
                case CellData.CellState.Revealed:
                    _spriteRenderer.sprite = _revealedSprite;
                    _spriteRenderer.sortingOrder = 0;
                    break;
            }
        }

        private void OnMouseDown()
        {
            if (_cellData != null)
            {
                _effectController?.PlayEffect(EffectType.Click);
                OnCellClicked?.Invoke(_cellData.X, _cellData.Y);
            }
        }
    }
}