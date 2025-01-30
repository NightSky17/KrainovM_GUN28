using UnityEngine;
using System.Collections.Generic;
public class Battlefield : MonoBehaviour
{
    public GameObject whiteCellPrefab; // Префаб белой клетки
    public GameObject blackCellPrefab; // Префаб черной клетки
    public GameObject whiteUnitPrefab; // Префаб белой фишки
    public GameObject blackUnitPrefab; // Префаб черной фишки
    public int gridSize = 8; // Размер доски (8x8)
    public float cellSize = 10.0f; // Размер клетки
    public Vector3 startPosition = Vector3.zero; // Начальная позиция доски

    private Cell[,] cells; // Сетка клеток
    public Dictionary<Cell, List<Cell>> whitePossibleMoves;
    public Dictionary<Cell, List<Cell>> blackPossibleMoves;
    public Dictionary<Cell, Dictionary<Cell, Cell>> whitePossibleCaptures; // Ключ - исходная клетка, значение - словарь (целевая клетка -> взятая шашка)
    public Dictionary<Cell, Dictionary<Cell, Cell>> blackPossibleCaptures;

    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    public Cell[,] GetCells()
    {
        return cells;
    }

    private void InitializePossibleMoves()
    {
        whitePossibleMoves = new Dictionary<Cell, List<Cell>>();
        blackPossibleMoves = new Dictionary<Cell, List<Cell>>();
        whitePossibleCaptures = new Dictionary<Cell, Dictionary<Cell, Cell>>();
        blackPossibleCaptures = new Dictionary<Cell, Dictionary<Cell, Cell>>();

        // Проходим по всем клеткам
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                var currentCell = cells[row, col];
                
                if (currentCell.Type == CellType.Black) // Ходить можно только с черных клеток
                {
                    // Обычные ходы
                    InitializeRegularMoves(currentCell, row, col);
                    
                    // Ходы взятия
                    InitializeCaptureMoves(currentCell, row, col);
                }
            }
        }
    }

    private void InitializeRegularMoves(Cell currentCell, int row, int col)
    {
        // Ходы для белых (движение от A1 к H8)
        var whiteMoves = new List<Cell>();
        if (row < gridSize - 1)
        {
            // Ход вправо-вверх (например, b4-c5)
            if (col < gridSize - 1)
            {
                whiteMoves.Add(cells[row + 1, col + 1]);
            }
            // Ход влево-вверх (например, b4-a5)
            if (col > 0)
            {
                whiteMoves.Add(cells[row + 1, col - 1]);
            }
        }
        whitePossibleMoves[currentCell] = whiteMoves;

        // Ходы для черных (движение от H8 к A1)
        var blackMoves = new List<Cell>();
        if (row > 0)
        {
            // Ход вправо-вниз
            if (col < gridSize - 1)
            {
                blackMoves.Add(cells[row - 1, col + 1]);
            }
            // Ход влево-вниз
            if (col > 0)
            {
                blackMoves.Add(cells[row - 1, col - 1]);
            }
        }
        blackPossibleMoves[currentCell] = blackMoves;
    }

    private void InitializeCaptureMoves(Cell currentCell, int row, int col)
    {
        var whiteCaptures = new Dictionary<Cell, Cell>();
        var blackCaptures = new Dictionary<Cell, Cell>();

        // Проверяем все направления для обоих цветов
        // Влево-вверх
        if (row > 1 && col > 1)
        {
            var captureCell = cells[row - 1, col - 1];
            var targetCell = cells[row - 2, col - 2];
            whiteCaptures[targetCell] = captureCell;
            blackCaptures[targetCell] = captureCell;
        }

        // Вправо-вверх
        if (row > 1 && col < gridSize - 2)
        {
            var captureCell = cells[row - 1, col + 1];
            var targetCell = cells[row - 2, col + 2];
            whiteCaptures[targetCell] = captureCell;
            blackCaptures[targetCell] = captureCell;
        }

        // Влево-вниз
        if (row < gridSize - 2 && col > 1)
        {
            var captureCell = cells[row + 1, col - 1];
            var targetCell = cells[row + 2, col - 2];
            whiteCaptures[targetCell] = captureCell;
            blackCaptures[targetCell] = captureCell;
        }

        // Вправо-вниз
        if (row < gridSize - 2 && col < gridSize - 2)
        {
            var captureCell = cells[row + 1, col + 1];
            var targetCell = cells[row + 2, col + 2];
            whiteCaptures[targetCell] = captureCell;
            blackCaptures[targetCell] = captureCell;
        }

        whitePossibleCaptures[currentCell] = whiteCaptures;
        blackPossibleCaptures[currentCell] = blackCaptures;
    }

    // Метод для получения возможных взятий для конкретной клетки
    public Dictionary<Cell, Cell> GetPossibleCaptures(Cell cell, UnitType unitType)
    {
        var result = new Dictionary<Cell, Cell>();
        var captures = unitType == UnitType.White ? 
            whitePossibleCaptures : blackPossibleCaptures;

        if (captures.TryGetValue(cell, out var possibleCaptures))
        {
            foreach (var capture in possibleCaptures)
            {
                var targetCell = capture.Key;
                var captureCell = capture.Value;
                
                // Проверяем, что целевая клетка пуста и на пути есть шашка противника
                if (targetCell.UnitOnCell == null && 
                    captureCell.UnitOnCell != null && 
                    captureCell.UnitOnCell.Type != unitType)
                {
                    result[targetCell] = captureCell;
                }
            }
        }

        return result;
    }

    // Обновленный метод получения всех возможных ходов
    public List<Cell> GetPossibleMoves(Cell cell, UnitType unitType)
    {
        var result = new List<Cell>();

        var moves = unitType == UnitType.White ? 
            whitePossibleMoves : blackPossibleMoves;
        
        if (moves.TryGetValue(cell, out var possibleMoves))
        {
            foreach (var moveCell in possibleMoves)
            {
                if (moveCell.UnitOnCell == null)
                {
                    result.Add(moveCell);
                }
            }
        }

        return result;
    }

    public void InitializeBattlefield()
    {
        cells = new Cell[gridSize, gridSize];

        // Массив букв для обозначения столбцов
        char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                // Выбор префаба клетки (первоначально делаем клетки черными и белыми)
                GameObject cellPrefab = (row + col) % 2 == 1 ? whiteCellPrefab : blackCellPrefab;

                // Создаем клетку в отрицательном направлении
                Vector3 position = startPosition + new Vector3(-col * cellSize, 0, -row * cellSize);
                GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, transform);

                // Инициализируем клетку
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Initialize(row, col, (row + col) % 2 == 1 ? CellType.White : CellType.Black);
                cell.Label = $"{row + 1}{letters[col]}";
                cells[row, col] = cell;

                // Размещаем фишки только на черных клетках
                if ((row + col) % 2 == 0)  // Проверка, что клетка черная
                {
                    if (row > 4) // Черные фишки
                    {
                        GameObject blackUnitObject = Instantiate(blackUnitPrefab, position, Quaternion.identity, transform);
                        Unit blackUnit = blackUnitObject.GetComponent<Unit>();
                        blackUnit.Initialize(UnitType.Black, cell);
                        cell.PlaceUnit(blackUnit);
                    }
                    else if (row < 3) // Белые фишки
                    {
                        GameObject whiteUnitObject = Instantiate(whiteUnitPrefab, position, Quaternion.identity, transform);
                        Unit whiteUnit = whiteUnitObject.GetComponent<Unit>();
                        whiteUnit.Initialize(UnitType.White, cell);
                        cell.PlaceUnit(whiteUnit);
                    }
                }
            }
        }

        InitializePossibleMoves(); // Инициализируем возможные ходы после создания клеток
    }

    // Метод для получения соседних клеток
    public List<Cell> GetAdjacentCells(Cell cell)
    {
        List<Cell> adjacentCells = new List<Cell>();

        int row = cell.Row;
        int col = cell.Col;

        // Соседи: сверху, снизу, слева, справа
        if (row > 0) adjacentCells.Add(cells[row - 1, col]); // Сосед сверху
        if (row < gridSize - 1) adjacentCells.Add(cells[row + 1, col]); // Сосед снизу
        if (col > 0) adjacentCells.Add(cells[row, col - 1]); // Сосед слева
        if (col < gridSize - 1) adjacentCells.Add(cells[row, col + 1]); // Сосед справа

        return adjacentCells;
    }

    public Cell GetCellAtPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
            
        return cells[x, y];
    }

    public List<Unit> GetUnits()
    {
        List<Unit> units = new List<Unit>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (cells[x, y].UnitOnCell != null)
                {
                    units.Add(cells[x, y].UnitOnCell);
                }
            }
        }
        return units;
    }
}
