using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class BattleState
{
    protected BattleController controller;

    public BattleState(BattleController controller)
    {
        this.controller = controller;
    }

    public virtual void OnCellClicked(Cell cell) { }
    public virtual void OnEscapePressed() { }
    public virtual void OnSpacePressed() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}

public class WhiteTurnState : BattleState
{
    public WhiteTurnState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateUnitsAbleToMove();
        controller.LogMessage("Ход белых");
    }

    public override void OnCellClicked(Cell cell)
    {
        if (cell.UnitOnCell != null && 
            cell.UnitOnCell.Type == UnitType.White && 
            controller.UnitsAbleToMove.Contains(cell.UnitOnCell))
        {
            controller.SelectUnit(cell);
            controller.SetState(new WhiteSelectedState(controller));
        }
    }
}

public class WhiteSelectedState : BattleState
{
    public WhiteSelectedState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        if(controller.IsSelectedUnitCrowned())
        {
            controller.UpdateCrownAvailableMoves();
        }
        else
        {
            controller.UpdateAvailableMoves();
        }
    }

    public override void OnCellClicked(Cell cell)
    {
        if (controller.AvailableMoves.Contains(cell))
        {
            controller.SelectTargetCell(cell);
            controller.SetState(new WhiteMoveReadyState(controller));
        }
    }

    public override void OnEscapePressed()
    {
        controller.ClearSelection();
        controller.SetState(new WhiteTurnState(controller));
    }
}

public class WhiteMoveReadyState : BattleState
{
    public WhiteMoveReadyState(BattleController controller) : base(controller) { }

    public override void OnSpacePressed()
    {
        controller.ExecuteMove();

        if (controller.UnitAtEdge()) 
        {
            controller.UpdateToQueen();
        }

        controller.SetState(new CheckCapturesBlack(controller));
    }

    public override void OnEscapePressed()
    {
        controller.ClearTargetCell();
        controller.SetState(new WhiteSelectedState(controller));
    }
}

public class CheckCapturesWhite : BattleState
{
    public CheckCapturesWhite(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        bool hasCaptures = false;
        
        hasCaptures = controller.HasForcedCaptures() || controller.HasCrownedUnitCaptures();

        if (hasCaptures)
        {
            controller.SetState(new WhiteCaptureState(controller));
        }
        else
        {
            controller.SetState(new WhiteTurnState(controller));
        }
    }
}

public class WhiteCaptureState : BattleState
{
    public WhiteCaptureState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateUnitsWithCaptures();
        controller.GetCrownedUnitCaptures();
        controller.LogMessage("Белые должны взять шашку");
    }

    public override void OnCellClicked(Cell cell)
    {
        if (cell.UnitOnCell != null && 
            cell.UnitOnCell.Type == UnitType.White && 
            controller.UnitsWithCaptures.ToList().Contains(cell.UnitOnCell))
        {
            controller.SelectUnit(cell);
            controller.SetState(new WhiteCaptureSelectedState(controller));
        }
        else
        {
            controller.LogMessage("Выберите шашку, которая может взять");
        }
    }
}

public class WhiteCaptureSelectedState : BattleState
{
    public WhiteCaptureSelectedState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateAvailableCaptures();
    }

    public override void OnCellClicked(Cell cell)
    {
        if (controller.AvailableCaptures.ToList().Contains(cell))
        {
            controller.SelectTargetCell(cell);
            controller.SetState(new WhiteCaptureReadyState(controller));
        }
    }

    public override void OnEscapePressed()
    {
        controller.ClearSelection();
        controller.SetState(new WhiteCaptureState(controller));
    }
}

public class WhiteCaptureReadyState : BattleState
{
    public WhiteCaptureReadyState(BattleController controller) : base(controller) { }

    public override void OnSpacePressed()
    {
        controller.ExecuteCapture();
        if (controller.UnitAtEdge()) 
        {
            controller.UpdateToQueen();
        }
        controller.SetState(new NextCheckCapturesWhite(controller));
    }

    public override void OnEscapePressed()
    {
        controller.ClearTargetCell();
        controller.SetState(new WhiteCaptureSelectedState(controller));
    }
}

public class NextCheckCapturesWhite : BattleState
{
    public NextCheckCapturesWhite(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        bool hasCaptures = false;
        
        hasCaptures = controller.HasCellForcedCaptures();

        if (hasCaptures)
        {
            controller.SetState(new WhiteCaptureState(controller));
        }
        else
        {
            controller.SetState(new CheckCapturesBlack(controller));
        }
    }
}


public class BlackTurnState : BattleState
{
    public BlackTurnState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateUnitsAbleToMove();
        controller.LogMessage("Ход черных");
    }

    public override void OnCellClicked(Cell cell)
    {
        if (cell.UnitOnCell != null && 
            cell.UnitOnCell.Type == UnitType.Black && 
            controller.UnitsAbleToMove.Contains(cell.UnitOnCell))
        {
            controller.SelectUnit(cell);
            controller.SetState(new BlackSelectedState(controller));
        }
    }
}

public class BlackSelectedState : BattleState
{
    public BlackSelectedState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        if(controller.IsSelectedUnitCrowned())
        {
            controller.UpdateCrownAvailableMoves();
        }
        else
        {
            controller.UpdateAvailableMoves();
        }
    }

    public override void OnCellClicked(Cell cell)
    {
        if (controller.AvailableMoves.Contains(cell))
        {
            controller.SelectTargetCell(cell);
            controller.SetState(new BlackMoveReadyState(controller));
        }
    }

    public override void OnEscapePressed()
    {
        controller.ClearSelection();
        controller.SetState(new BlackTurnState(controller));
    }
}

public class BlackMoveReadyState : BattleState
{
    public BlackMoveReadyState(BattleController controller) : base(controller) { }

    public override void OnSpacePressed()
    {
        controller.ExecuteMove();
        if (controller.UnitAtEdge()) 
        {
            controller.UpdateToQueen();
        }
        controller.SetState(new CheckCapturesWhite(controller));
    }

    public override void OnEscapePressed()
    {
        controller.ClearTargetCell();
        controller.SetState(new BlackSelectedState(controller));
    }
}


public class CheckCapturesBlack : BattleState
{
    public CheckCapturesBlack(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        bool hasCaptures = false;

        hasCaptures = controller.HasForcedCaptures() || controller.HasCrownedUnitCaptures();
        
        if (hasCaptures)
        {
            controller.SetState(new BlackCaptureState(controller));
        }
        else
        {
            controller.SetState(new BlackTurnState(controller));
        }
    }
} 


public class BlackCaptureState : BattleState
{
    public BlackCaptureState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateUnitsWithCaptures();
        controller.GetCrownedUnitCaptures();
        controller.LogMessage("Черные должны взять шашку");
    }

    public override void OnCellClicked(Cell cell)
    {
        if (cell.UnitOnCell != null && 
            cell.UnitOnCell.Type == UnitType.Black && 
            controller.UnitsWithCaptures.ToList().Contains(cell.UnitOnCell))
        {
            controller.SelectUnit(cell);
            controller.SetState(new BlackCaptureSelectedState(controller));
        }
        else
        {
            controller.LogMessage("Выберите шашку, которая может взять");
        }
    }
}

public class BlackCaptureSelectedState : BattleState
{
    public BlackCaptureSelectedState(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        controller.UpdateAvailableCaptures();
    }

    public override void OnCellClicked(Cell cell)
    {
        if (controller.AvailableCaptures.ToList().Contains(cell))
        {
            controller.SelectTargetCell(cell);
            controller.SetState(new BlackCaptureReadyState(controller));
        }
    }

    public override void OnEscapePressed()
    {
        controller.ClearSelection();
        controller.SetState(new BlackCaptureState(controller));
    }
}

public class BlackCaptureReadyState : BattleState
{
    public BlackCaptureReadyState(BattleController controller) : base(controller) { }

    public override void OnSpacePressed()
    {
        controller.ExecuteCapture();
        if (controller.UnitAtEdge()) 
        {
            controller.UpdateToQueen();
        }
        controller.SetState(new NextCheckCapturesBlack(controller));
    }

    public override void OnEscapePressed()
    {
        controller.ClearTargetCell();
        controller.SetState(new BlackCaptureSelectedState(controller));
    }
}

public class NextCheckCapturesBlack : BattleState
{
    public NextCheckCapturesBlack(BattleController controller) : base(controller) { }

    public override void OnEnter()
    {
        bool hasCaptures = false;
        
        hasCaptures = controller.HasCellForcedCaptures();

        if (hasCaptures)
        {
            controller.SetState(new BlackCaptureState(controller));
        }
        else
        {
            controller.SetState(new CheckCapturesWhite(controller));
        }
    }
}