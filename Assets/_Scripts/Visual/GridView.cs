using System;
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
        
        [Header("Grid Layout Settings")]
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private float cellSpacing = 0.1f;
        
        private GridData gridData;
        private CellView[,] cellViews;

        public void Initialize(GridData data)
        {
            gridData = data;
            CreateGrid();
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
            if (cellViews == null) return;
            
            foreach (var cellView in cellViews)
            {
                if (cellView != null)
                    cellView.OnCellClicked -= HandleCellClicked;
            }
        }

        private Vector3 CalculateCellPosition(int x, int y)
        {
            float xPos = x * (cellSize + cellSpacing);
            float yPos = y * (cellSize + cellSpacing);
            
            return new Vector3(xPos, yPos, 0f);
        }

        private void CenterGrid()
        {
            float gridWidth = (gridData.Width - 1) * (cellSize + cellSpacing);
            float gridHeight = (gridData.Height - 1) * (cellSize + cellSpacing);
            
            Vector3 offset = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0f);
            transform.position += offset;
        }
    }
}