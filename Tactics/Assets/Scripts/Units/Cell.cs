using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int Row { get; private set; }
    public int Col { get; private set; }
    public CellType Type { get; private set; }
    public GameStatus Status { get; private set; } = GameStatus.None;
    public Unit UnitOnCell { get; set; }
    public string Label { get; set; }
    private Renderer rend;
    private BattleController battleController;
    private Coroutine blinkCoroutine;
    private Color originalColor;
    private bool isBlinking = false;

    private Vector2Int boardPosition;

    public Vector2Int BoardPosition => boardPosition;


    public void Initialize(int row, int col, CellType type)
    {
        Row = row;
        Col = col;
        Type = type;

        rend = GetComponent<Renderer>();
        if (Type == CellType.White)
        {
            rend.material.color = Color.white;
        }
        else
        {
            rend.material.color = Color.black;
        }

        boardPosition = new Vector2Int(row, col);
    }

    public void SetController(BattleController controller)
    {
        battleController = controller;
    }

    public void PlaceUnit(Unit unit)
    {
        UnitOnCell = unit;
    }

    public void RemoveUnit()
    {
        UnitOnCell = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        battleController.HandleCellClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleController.HandlePointerEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleController.HandlePointerExit(this);
    }

    public void HighlightCell()
    {
        if(UnitOnCell?.Status == GameStatus.Movable || Status == GameStatus.Targeted)
            rend.material.color = Color.yellow;
        else
            rend.material.color = Color.green;
    }

    public void RemoveHighlight()
    {
        rend.material.color = Color.black;
    }

    public void HighlightMoveTarget()
    {
        if (!isBlinking)
        {
            originalColor = rend.material.color;
            blinkCoroutine = StartCoroutine(BlinkEffect());
            isBlinking = true;
        }
    }

    public void RemoveMoveTargetHighlight()
    {
        if (isBlinking)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            isBlinking = false;
            rend.material.color = Type == CellType.White ? Color.white : Color.black;
        }
    }

    private IEnumerator BlinkEffect()
    {
        Color highlightColor = new Color(0.3f, 0.8f, 0.3f); // Зеленоватый цвет
        float blinkSpeed = 2f; // Скорость мигания (в Герцах)
        
        while (true)
        {
            // Плавное изменение цвета туда-обратно
            float t = (1 + Mathf.Sin(Time.time * blinkSpeed * Mathf.PI)) / 2;
            rend.material.color = Color.Lerp(originalColor, highlightColor, t);
            yield return null;
        }
    }

    public void SelectCell()
    {
        rend.material.color = Color.red;
    }

    public void UnselectCell()
    {
        rend.material.color = Color.black;
        ResetStatus();
        if(UnitOnCell != null)
            UnitOnCell.ResetStatus();
    }

    public bool IsSelectable()
    {
        return Type == CellType.Black;
    }

    public void SetSelected()
    {   
        if(UnitOnCell != null)
            UnitOnCell.SetSelected();
    }

    public override string ToString()
    {
        return Label;
    }

    public void ResetStatus()
    {
        Status = GameStatus.None;
    }
    public void SetTargeted()
    {
        Status = GameStatus.Targeted;
    }
}