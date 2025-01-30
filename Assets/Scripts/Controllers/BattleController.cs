using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMPro.TextMeshProUGUI debugText;
    [SerializeField] private TMPro.TextMeshProUGUI restartText;
    private float tabHoldTime = 0f;
    private bool isTabPressed = false;

    public Battlefield battlefield;

    private SelectCellCommand selectCommand = new SelectCellCommand();
    private UnselectCellCommand unselectCommand = new UnselectCellCommand();
    private HighlightCellCommand highlightCommand = new HighlightCellCommand();
    private RemoveHighlightCommand removeHighlightCommand = new RemoveHighlightCommand();
    private HighlightMoveTargetCommand highlightMoveTargetCommand = new HighlightMoveTargetCommand();
    private RemoveMoveTargetHighlightCommand removeMoveTargetHighlightCommand = new RemoveMoveTargetHighlightCommand();
    private BattleState currentState;
  
    private Unit selectedUnit;
    private Cell selectedTargetCell; // Выбранная целевая клетка для хода

    private List<Unit> unitsAbleToMove = new List<Unit>(); // Список юнитов, которые могут ходить
    private List<Cell> availableCells = new List<Cell>(); // Клетки, на которые можно переместиться
    
    private List<Unit> unitsWithCaptures = new List<Unit>();
    private List<Cell> availableCaptures = new List<Cell>();
    
    public IReadOnlyList<Unit> UnitsWithCaptures => unitsWithCaptures;
    public IReadOnlyList<Cell> AvailableCaptures => availableCaptures;

    // Свойства для доступа из состояний
    public List<Unit> UnitsAbleToMove => unitsAbleToMove;
    public List<Cell> AvailableMoves => availableCells;

    private void Awake()
    {
        battlefield.InitializeBattlefield();
    }

    private void Start()
    {
        foreach (var cell in battlefield.GetCells())
        {
            cell.SetController(this);
        }

        foreach (var unit in battlefield.GetUnits())
        {
            unit.SetController(this);
        }

        InitializeGame();
    }

    // При наведении мыши на клетку
    public void HandlePointerEnter(Cell cell)
    {
        highlightCommand.Interact(cell);
    }

    // При уходе мыши с клетки
    public void HandlePointerExit(Cell cell)
    {
        removeHighlightCommand.Interact(cell);
    }

    private void InitializeGame()
    {
        // Устанавливаем начальное состояние
        SetState(new WhiteTurnState(this));
    }

    public void SetState(BattleState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void UpdateUnitsAbleToMove()
    {
        unitsAbleToMove.Clear();
        var cells = battlefield.GetCells();
        
        foreach (var cell in cells)
        {
            if (cell.UnitOnCell != null && 
                cell.UnitOnCell.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
            {
                // Проверяем, есть ли у шашки возможные ходы
                var possibleMoves = battlefield.GetPossibleMoves(cell, cell.UnitOnCell.Type);
                if (possibleMoves.Count > 0)
                {
                    cell.UnitOnCell.SetMovable();
                    unitsAbleToMove.Add(cell.UnitOnCell);
                }
            }
        }
    }

    private bool IsWhiteTurn()
    {
        var stateType = currentState.GetType();
        return currentState is WhiteTurnState || 
               currentState is WhiteSelectedState ||
               currentState is WhiteMoveReadyState ||
               currentState is CheckCapturesWhite ||
               currentState is WhiteCaptureState  || 
               currentState is WhiteCaptureSelectedState || 
               currentState is WhiteCaptureReadyState ||
               currentState is NextCheckCapturesWhite;
    }

    public void ClearAllHighlights()
    {
        var cells = battlefield.GetCells();
        foreach (var cell in cells)
        {
            removeHighlightCommand.Interact(cell);
            removeMoveTargetHighlightCommand.Interact(cell);
        }
    }

    public void SelectUnit(Cell cell)
    {
        if (selectedUnit != null)
        {
            unselectCommand.Interact(selectedUnit.CurrentCell);
        }
        selectedUnit = cell.UnitOnCell;
        selectCommand.Interact(cell);
    }

    public void HandleCellClicked(Cell cell)
    {
        if (!playerController.CanInteract()) return;
            currentState.OnCellClicked(cell);
    }

    public void ClearSelection()
    {
        if (selectedUnit != null)
        {
            unselectCommand.Interact(selectedUnit.CurrentCell);
            selectedUnit = null;
        }
        ResetAllCellStatus();
        ClearTargetCell();
    }

    public void ClearTargetCell()
    {
        if (selectedTargetCell != null)
        {
            removeMoveTargetHighlightCommand.Interact(selectedTargetCell);
            selectedTargetCell = null;
        }
    }

    private void Update()
    {
        if (!playerController.CanInteract()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentState.OnEscapePressed();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState.OnSpacePressed();
        }

        // Обработка зажатия Tab
        if (Input.GetKey(KeyCode.Tab))
        {
            restartText.transform.position = new Vector2(350.0f, 420f);

            if (!isTabPressed)
            {
                isTabPressed = true;
                tabHoldTime = 0f;
                restartText.transform.position = new Vector2(0.0f, -200.0f);
            }
            
            tabHoldTime += Time.deltaTime;
            
            if (tabHoldTime >= 3f)
            {
                restartText.text = "<mark=#ff000000>Перезапуск...</mark>";
                RestartGame();
            }
            else
            {
                float remainingTime = 3f - tabHoldTime;
                restartText.text = $"<mark=#ff000000>Удерживайте TAB для перезапуска ({remainingTime:F1}с)</mark>";
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
            restartText.text = "";
            restartText.transform.position = new Vector2(350.0f, -420f);
        }
    }

    
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateAvailableMoves()
    {
        availableCells.Clear();
        if (selectedUnit == null) return;

        var moves = IsWhiteTurn() ? battlefield.whitePossibleMoves : battlefield.blackPossibleMoves;
        if (moves.ContainsKey(selectedUnit.CurrentCell))
        {
            var mvs = moves[selectedUnit.CurrentCell];
            foreach(var move in mvs)
                move.SetTargeted();
            availableCells.AddRange(mvs);
        }
    }

    void ResetAllCellStatus()
    {
        foreach (var cell in battlefield.GetCells())
        {
            cell.ResetStatus();
        }
    }

    public void SelectTargetCell(Cell cell)
    {
        selectedTargetCell = cell;
        selectedUnit.SetMove();
        highlightMoveTargetCommand.Interact(cell);
    }

    public async void ExecuteMove()
    {
        if (selectedUnit == null || selectedTargetCell == null) return;

        var startCell = selectedUnit.CurrentCell;
        await playerController.ProcessMove(selectedUnit, selectedTargetCell);
        
        ClearSelection();
        ResetAllUnitsStatus();
        ClearAllHighlights();
    }

    // Сбрасывает статус всех юнитов
    private void ResetAllUnitsStatus()
    {
        foreach (var unit in battlefield.GetUnits())
        {
            unit.ResetStatus(); // Сброс статуса
        }
    }

    public bool HasForcedCaptures()
    {
        foreach (var cell in battlefield.GetCells())
        {
            if (cell.UnitOnCell != null && 
                cell.UnitOnCell.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
            {
                var captures = battlefield.GetPossibleCaptures(cell, cell.UnitOnCell.Type);
                if (captures.Count > 0) return true;
            }
        }
        return false;
    }

    public bool HasCellForcedCaptures()
    {
        if (selectedUnit != null && 
                selectedUnit.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
        {
            var captures = battlefield.GetPossibleCaptures(selectedUnit.CurrentCell, selectedUnit.Type);
            if (captures.Count > 0) return true;
        }
        return false;
    }

    public void UpdateUnitsWithCaptures()
    {
        unitsWithCaptures.Clear();
        availableCaptures.Clear();
        foreach (var cell in battlefield.GetCells())
        {
            if (cell.UnitOnCell != null && cell.UnitOnCell.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
            {
                var captures = battlefield.GetPossibleCaptures(cell, cell.UnitOnCell.Type);
                if (captures.Count > 0)
                {
                    cell.UnitOnCell.SetMovable();
                    unitsWithCaptures.Add(cell.UnitOnCell);
                }
            }
        }
    }

    public void UpdateAvailableCaptures()
    {
        availableCaptures.Clear();
        if (selectedUnit == null) return;

        var captures = IsWhiteTurn() ? battlefield.whitePossibleCaptures : battlefield.blackPossibleCaptures;
        if (captures.ContainsKey(selectedUnit.CurrentCell))
        {
            availableCaptures.AddRange(captures[selectedUnit.CurrentCell].Keys);
        }
    }

    public async void ExecuteCapture()
    {
        if (selectedUnit == null || selectedTargetCell == null) return;

        var startCell = selectedUnit.CurrentCell;
        Vector2Int direction = (selectedTargetCell.BoardPosition - selectedUnit.CurrentCell.BoardPosition) / 2;
        Cell middleCell = battlefield.GetCellAtPosition(
            selectedUnit.CurrentCell.BoardPosition.x + direction.x,
            selectedUnit.CurrentCell.BoardPosition.y + direction.y
        );
        
        Unit capturedUnit = middleCell.UnitOnCell;
        await playerController.ProcessCapture(selectedUnit, selectedTargetCell, capturedUnit);
        
        // Очищаем исходную клетку и клетку со взятой шашкой
        startCell.UnitOnCell = null;
        middleCell.UnitOnCell = null;
        
        ClearSelection();
        ClearAllHighlights();
    }

    public void UpdateToQueen()
    {
        if (selectedUnit != null)
        {
            selectedUnit.PromoteToQueen();
        }
    }

    public bool UnitAtEdge()
    {
        if (selectedUnit != null)
        {
           if (selectedUnit.Type == UnitType.White)
           {
                return selectedUnit.CurrentCell.Row == 7;
           }
           else
           {
                return selectedUnit.CurrentCell.Row == 0;
           }
        }
        return false;
    }

    public void UpdateCrownAvailableMoves()
    {
        availableCells.Clear();
        var currentPosition = selectedUnit.CurrentCell.BoardPosition;
        
        // Проверяем все 4 диагональных направления
        CheckCrownDirection(currentPosition, new Vector2Int(1, 1));   // вверх-вправо
        CheckCrownDirection(currentPosition, new Vector2Int(1, -1));  // вниз-вправо
        CheckCrownDirection(currentPosition, new Vector2Int(-1, 1));  // вверх-влево
        CheckCrownDirection(currentPosition, new Vector2Int(-1, -1)); // вниз-влево
    }

    private void CheckCrownDirection(Vector2Int start, Vector2Int direction)
    {
        Vector2Int current = start + direction;
        
        while (current.x >= 0 && current.x < 8 && current.y >= 0 && current.y < 8)
        {
            Cell cell = battlefield.GetCells()[current.x, current.y];
            if (cell.UnitOnCell == null)
            {
                availableCells.Add(cell);
                current += direction;
            }
            else
            {
                break; // Останавливаемся если встретили шашку
            }
        }
    }

    public bool IsSelectedUnitCrowned()
    {
        return selectedUnit.IsCrowned;
    }

    public bool HasCrownedUnitCaptures()
    {
        var units = battlefield.GetUnits();
        bool hasCaptures = false;

        foreach (var unit in units)
        {
            if (unit.IsCrowned && unit.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
            {
                // Проверяем все диагональные направления до первой шашки
                var currentPosition = unit.CurrentCell.BoardPosition;
                
                if (CheckCrownDirectionCaptures(currentPosition, new Vector2Int(1, 1), unit.Type)) hasCaptures = true;   
                if (CheckCrownDirectionCaptures(currentPosition, new Vector2Int(1, -1), unit.Type)) hasCaptures = true;  
                if (CheckCrownDirectionCaptures(currentPosition, new Vector2Int(-1, 1), unit.Type)) hasCaptures = true;  
                if (CheckCrownDirectionCaptures(currentPosition, new Vector2Int(-1, -1), unit.Type)) hasCaptures = true;
            }
        }
        return hasCaptures;
    }

    private bool CheckCrownDirectionCaptures(Vector2Int start, Vector2Int direction, UnitType type)
    {
        Vector2Int current = start + direction;
        
        // Идем по диагонали до первой шашки
        while (current.x >= 0 && current.x < 8 && current.y >= 0 && current.y < 8)
        {
            Cell cell = battlefield.GetCells()[current.x, current.y];
            if (cell.UnitOnCell != null)
            {
                // Если нашли шашку противника, проверяем следующую клетку за ней
                if (cell.UnitOnCell.Type != type)
                {
                    Vector2Int capturePosition = current + direction;
                    if (capturePosition.x >= 0 && capturePosition.x < 8 && 
                        capturePosition.y >= 0 && capturePosition.y < 8)
                    {
                        Cell captureCell = battlefield.GetCells()[capturePosition.x, capturePosition.y];
                        if (captureCell.UnitOnCell == null)
                        {
                            return true; // Можем взять
                        }
                    }
                }
                break; // Встретили любую шашку - дальше не идем
            }
            current += direction;
        }
        return false;
    }

    public void GetCrownedUnitCaptures()
    {
        var units = battlefield.GetUnits();

        foreach (var unit in units)
        {
            if (unit.IsCrowned && unit.Type == (IsWhiteTurn() ? UnitType.White : UnitType.Black))
            {
                var currentPosition = unit.CurrentCell.BoardPosition;
                List<Cell> captures = new List<Cell>();
                
                GetCrownDirectionCaptures(currentPosition, new Vector2Int(1, 1), unit.Type, captures);   
                GetCrownDirectionCaptures(currentPosition, new Vector2Int(1, -1), unit.Type, captures);  
                GetCrownDirectionCaptures(currentPosition, new Vector2Int(-1, 1), unit.Type, captures);  
                GetCrownDirectionCaptures(currentPosition, new Vector2Int(-1, -1), unit.Type, captures);
                
                if (captures.Count > 0)
                {
                    unitsWithCaptures.Add(unit);
                    availableCaptures.AddRange(captures);
                }
            }
        }
    }

    private void GetCrownDirectionCaptures(Vector2Int start, Vector2Int direction, UnitType type, List<Cell> captures)
    {
        Vector2Int current = start + direction;
        
        while (current.x >= 0 && current.x < 8 && current.y >= 0 && current.y < 8)
        {
            Cell cell = battlefield.GetCells()[current.x, current.y];
            if (cell.UnitOnCell != null)
            {
                if (cell.UnitOnCell.Type != type)
                {
                    Vector2Int capturePosition = current + direction;
                    if (capturePosition.x >= 0 && capturePosition.x < 8 && 
                        capturePosition.y >= 0 && capturePosition.y < 8)
                    {
                        Cell captureCell = battlefield.GetCells()[capturePosition.x, capturePosition.y];
                        if (captureCell.UnitOnCell == null)
                        {
                            captures.Add(captureCell);
                        }
                    }
                }
                break;
            }
            current += direction;
        }
    }

    public void LogMessage(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
    }
}
