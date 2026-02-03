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
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject gemPrefab;
        
        [Header("Grid Layout Settings")]
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private float cellSpacing = 0.1f;
        
        private GridData gridData;
        private CellView[,] cellViews;
        private List<GemView> gemViews;

        public void Initialize(GridData data, List<GemData> placedGems)
        {
            gridData = data;
            CreateGrid();
            CreateGemViews(placedGems);
        }

        private void CreateGrid()
        {
            if (gridData == null)
            {
                Debug.LogError("GridData is null. Cannot create grid.");
                return;
            }

            if (cellPrefab == null)
            {
                Debug.LogError("Cell prefab is not assigned.");
                return;
            }

            cellViews = new CellView[gridData.Width, gridData.Height];
            
            for (int x = 0; x < gridData.Width; x++)
            {
                for (int y = 0; y < gridData.Height; y++)
                    InstantiateCell(x, y);
            }
            
            CenterGrid();
        }

        private void InstantiateCell(int x, int y)
        {
            Vector3 position = CalculateCellPosition(x, y);
            
            GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, transform);
            cellObject.name = $"Cell_{x}_{y}";
            
            CellView cellView = cellObject.GetComponent<CellView>();
            if (cellView == null)
            {
                Debug.LogError($"Cell prefab does not have a CellView component.");
                Destroy(cellObject);
                return;
            }
            
            CellData cellData = gridData.GetCell(x, y);
            cellView.Initialize(cellData);
            cellView.OnCellClicked += HandleCellClicked;
            
            cellViews[x, y] = cellView;
        }
        
        private void HandleCellClicked(int x, int y)
        {
            OnCellClicked?.Invoke(x, y);
        }
        
        private void OnDestroy()
        {
            if (cellViews != null)
            {
                foreach (var cellView in cellViews)
                {
                    if (cellView != null)
                        cellView.OnCellClicked -= HandleCellClicked;
                }
            }
        }

        private void CreateGemViews(List<GemData> placedGems)
        {
            gemViews = new List<GemView>();

            if (gemPrefab == null)
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
            
            GameObject gemObject = Instantiate(gemPrefab, transform);
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
            gemViews.Add(gemView);
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
            float xPos = x * (cellSize + cellSpacing);
            float yPos = y * (cellSize + cellSpacing);
            
            return new Vector3(xPos, yPos, 0);
        }

        private void CenterGrid()
        {
            float gridWidth = (gridData.Width - 1) * (cellSize + cellSpacing);
            float gridHeight = (gridData.Height - 1) * (cellSize + cellSpacing);
            
            Vector3 offset = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0);
            transform.position += offset;
        }
    }
}