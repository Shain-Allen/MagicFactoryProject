using System;
using System.Collections.Generic;
using UnityEngine;

public class VoidChestLogic : Placeable
{
    GridControl grid;
    BeltLogic input;

    public override void PlacedAction(GridControl grid_)
    {
        grid = grid_;
        input = null;
        
        grid.OnBeltTimerCycle += BeltCycle;
    }

    public void BeltCycle(object sender, EventArgs e)
	{
	    MoveItem();
	}

    public void MoveItem()
	{
		if (input)
		{
            if (input.itemSlot)
            {
                Destroy(input.itemSlot);
                input.itemSlot = null;
            }
            input.MoveItem();
		}
	}

	public override void RemovedAction()
    {
        grid.placeObjects.Remove(transform.position);
        
        if(input != null)
        {
            input.frontBelt = null;
            input.UpdateSprite();
        }
        input = null;

		Destroy(gameObject);
    }
}
