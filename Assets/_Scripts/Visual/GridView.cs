using System;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

namespace Visual
{
    // Responsible for visual representation of the entire grid
    // Instantiates and positions CellView components based on GridData

    public class GridView : MonoBehaviour
    {
        public event Action<int, int> OnCellClicked;
        
        [Header("Prefab References")]
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private GameObject _gemPrefab;
        
        [Header("Grid Layout Settings")]
        [SerializeField] private float _cellSize = 1.0f;
        [SerializeField] private float _cellSpacing = 0.1f;
        
        private GridData _gridData;
        private CellView[,] _cellViews;
        private List<GemView> _gemViews;
        private Dictionary<int, GemView> _gemViewsById;

        public void Initialize(GridData data, List<GemData> placedGems)
        {
            _gridData = data;
            CreateGrid();
            CreateGemViews(placedGems);
        }
        private void OnDestroy()
        {
            if (_cellViews != null)
            {
                foreach (var cellView in _cellViews)
                {
                    if (cellView != null)
                        cellView.OnCellClicked -= HandleCellClicked;
                }
            }
        }

        private void CreateGrid()
        {
            if (_gridData == null)
            {
                Debug.LogError("GridData is null. Cannot create grid.");
                return;
            }

            if (_cellPrefab == null)
            {
                Debug.LogError("Cell prefab is not assigned.");
                return;
            }

            _cellViews = new CellView[_gridData.Width, _gridData.Height];
            
            for (int x = 0; x < _gridData.Width; x++)
            {
                for (int y = 0; y < _gridData.Height; y++)
                    InstantiateCell(x, y);
            }
            
            CenterGrid();
        }
        private void InstantiateCell(int x, int y)
        {
            Vector3 position = CalculateCellPosition(x, y);
            
            GameObject cellObject = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
            cellObject.name = $"Cell_{x}_{y}";
            
            CellView cellView = cellObject.GetComponent<CellView>();
            if (cellView == null)
            {
                Debug.LogError($"Cell prefab does not have a CellView component.");
                Destroy(cellObject);
                return;
            }
            
            CellData cellData = _gridData.GetCell(x, y);
            cellView.Initialize(cellData);
            cellView.OnCellClicked += HandleCellClicked;
            
            _cellViews[x, y] = cellView;
        }
        private void HandleCellClicked(int x, int y)
        {
            NotifyGemClicked(x, y);
            OnCellClicked?.Invoke(x, y);
        }
        private void NotifyGemClicked(int x, int y)
        {
            CellData cellData = _gridData.GetCell(x, y);
            
            if (cellData == null || !cellData.HasGem)
            {
                return;
            }

            GemView gemView = FindGemViewById(cellData.GemId);
            gemView?.OnGemClicked();
        }
        private GemView FindGemViewById(int gemId)
        {
            if (_gemViewsById != null && _gemViewsById.TryGetValue(gemId, out GemView gemView))
            {
                return gemView;
            }
            return null;
        }
        private void CreateGemViews(List<GemData> placedGems)
        {
            _gemViews = new List<GemView>();
            _gemViewsById = new Dictionary<int, GemView>();

            if (_gemPrefab == null)
            {
                Debug.LogError("Gem prefab is not assigned.");
                return;
            }

            foreach (var gemData in placedGems)
                CreateGemView(gemData);
        }
        private void CreateGemView(GemData gemData)
        {
            if (gemData.OccupiedCells.Count == 0)
            {
                Debug.LogWarning($"Gem {gemData.Id} has no occupied cells.");
                return;
            }

            Vector3 gemLocalPosition = CalculateGemPosition(gemData);
            
            GameObject gemObject = Instantiate(_gemPrefab, transform);
            gemObject.transform.localPosition = gemLocalPosition;
            gemObject.name = $"Gem_{gemData.Id}_{gemData.Definition.GemName}";

            GemView gemView = gemObject.GetComponent<GemView>();
            if (gemView == null)
            {
                Debug.LogError("Gem prefab does not have a GemView component.");
                Destroy(gemObject);
                return;
            }

            gemView.Initialize(gemData);
            _gemViews.Add(gemView);
            _gemViewsById[gemData.Id] = gemView;
        }

        private Vector3 CalculateGemPosition(GemData gemData)
        {
            float totalX = 0f;
            float totalY = 0f;
            int cellCount = gemData.OccupiedCells.Count;

            foreach (var cell in gemData.OccupiedCells)
            {
                Vector3 cellPosition = CalculateCellPosition(cell.X, cell.Y);
                totalX += cellPosition.x;
                totalY += cellPosition.y;
            }

            float avgX = totalX / cellCount;
            float avgY = totalY / cellCount;

            return new Vector3(avgX, avgY, 0);
        }
        private Vector3 CalculateCellPosition(int x, int y)
        {
            float xPos = x * (_cellSize + _cellSpacing);
            float yPos = y * (_cellSize + _cellSpacing);
            
            return new Vector3(xPos, yPos, 0);
        }
        private void CenterGrid()
        {
            float gridWidth = (_gridData.Width - 1) * (_cellSize + _cellSpacing);
            float gridHeight = (_gridData.Height - 1) * (_cellSize + _cellSpacing);
            
            Vector3 offset = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0);
            transform.position += offset;
        }
    }
}