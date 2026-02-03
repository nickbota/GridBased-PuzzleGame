using System;
using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of a single cell in the grid
    // Updates appearance based on the CellData state
    public class CellView : MonoBehaviour
    {
        public event Action<int, int> OnCellClicked;
        
        [Header("Visual References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite coveredSprite;
        [SerializeField] private Sprite revealedSprite;

        private CellData cellData;

        public void Initialize(CellData data)
        {
            if (spriteRenderer == null)
                Debug.LogError("Sprite Renderer not found!");

            if (cellData != null)
                cellData.OnStateChanged -= OnCellStateChanged;

            cellData = data;
            cellData.OnStateChanged += OnCellStateChanged;
            
            UpdateVisuals();
        }

        private void OnDestroy()
        {
            if (cellData != null)
                cellData.OnStateChanged -= OnCellStateChanged;
        }

        private void OnCellStateChanged(CellData data)
        {
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (cellData == null || spriteRenderer == null) return;

            switch(cellData.State)
            {
                case CellData.CellState.Covered:
                    spriteRenderer.sprite = coveredSprite;
                    spriteRenderer.sortingOrder = 99;
                    break;
                case CellData.CellState.Revealed:
                    spriteRenderer.sprite = revealedSprite;
                    spriteRenderer.sortingOrder = 0;
                    break;
            }
        }

        private void OnMouseDown()
        {
            if (cellData != null)
                OnCellClicked?.Invoke(cellData.X, cellData.Y);
        }
    }
}