public class SelectCellCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable() && cell.UnitOnCell != null)
        {
            cell.SelectCell();
            cell.SetSelected();
        }
    }
}

public class UnselectCellCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable() && cell.UnitOnCell != null)
        {
            cell.UnselectCell();
        }
    }
}

public class HighlightCellCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable() && cell.UnitOnCell?.Status != GameStatus.Selected)
        {
            cell.HighlightCell();
        }
    }
}

public class RemoveHighlightCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable() && cell.UnitOnCell?.Status != GameStatus.Selected)
        {
             cell.RemoveHighlight();
        }
    }
}

public class HighlightMoveTargetCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable() && cell.UnitOnCell == null)
        {
            cell.HighlightMoveTarget();
        }
    }
}

public class RemoveMoveTargetHighlightCommand : IGameplayCommand
{
    public void Interact(Cell cell)
    {
        if (cell.IsSelectable())
        {
            cell.RemoveMoveTargetHighlight();
        }
    }
}