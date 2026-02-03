using System;
using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of a single cell in the grid
    // Updates appearance based on the CellData state

    [RequireComponent(typeof(SpriteRenderer))]
    public class CellView : MonoBehaviour
    {
        public event Action<int, int> OnCellClicked;
        
        [Header("Visual References")]
        [SerializeField] private Sprite coveredSprite;
        [SerializeField] private Sprite revealedSprite;
        
        private SpriteRenderer spriteRenderer;
        private CellData cellData;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(CellData data)
        {
            if (cellData != null)
            {
                cellData.OnStateChanged -= OnCellStateChanged;
            }

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

            spriteRenderer.sprite = cellData.State == CellData.CellState.Covered 
                ? coveredSprite 
                : revealedSprite;
        }

        private void OnMouseDown()
        {
            if (cellData != null)
                OnCellClicked?.Invoke(cellData.X, cellData.Y);
        }
    }
}