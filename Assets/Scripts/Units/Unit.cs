using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnitType Type { get; private set; }
    public Cell CurrentCell { get; set; }
    public GameStatus Status { get; private set; } = GameStatus.None;
    private BattleController battleController;

    public GameObject whiteQueenPrefab;
    public GameObject blackQueenPrefab;

    public bool IsCrowned { get; private set; } = false;

    public void PromoteToQueen()
    {
        // Сохраняем текущую позицию и поворот
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        
        // Удаляем старую модель
        Destroy(transform.GetChild(0).gameObject);
        
        // Создаем новую модель дамки
        GameObject queenPrefab = Type == UnitType.White ? whiteQueenPrefab : blackQueenPrefab;
        GameObject queenModel = Instantiate(queenPrefab, position, rotation, transform);
        queenModel.transform.localPosition = Vector3.zero;
        IsCrowned = true;
    }

    public void Initialize(UnitType type, Cell cell)
    {
        Type = type;
        MoveToCell(cell);
    }

    public void SetController(BattleController controller)
    {
        battleController = controller;
    }

    public void MoveToCell(Cell cell)
    {
        CurrentCell = cell;
        transform.position = cell.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleController.HandlePointerEnter(CurrentCell);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleController.HandlePointerExit(CurrentCell);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        battleController.HandleCellClicked(CurrentCell);
    }

    public void SetSelected()
    {
        Status = GameStatus.Selected;
    }

    public void ResetStatus()
    {
        Status = GameStatus.None;
    }

    public void SetMove()
    {
        Status = GameStatus.Move;
    }

    public void SetMovable()
    {
        Status = GameStatus.Movable;
    }
}