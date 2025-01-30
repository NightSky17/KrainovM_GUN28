using UnityEngine;

public enum UnitType
{
    White,
    Black
}

public enum CellType
{
    White,
    Black
}

public enum GameStatus
{
    None,
    Selected,
    Movable,
    Targeted,
    Move
}

public enum CellHighlight
{
    None,
    Selected,
    Available
}