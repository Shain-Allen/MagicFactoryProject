using System;
using System.Collections.Generic;
using UnityEngine;

public class FullChestLogic : Placeable
{
    GridControl grid;
    BeltLogic output;
    GameObject itemSlot;

    public override void PlacedAction(GridControl grid_)
    {
        grid = grid_;
        output = null;

        grid.OnBeltTimerCycle += BeltCycle;
    }

    public void BeltCycle(object sender, EventArgs e)
	{
	    if(output.itemSlot == null)
            MoveItem();
	}

    public void MoveItem()
	{
		if (output && !output.itemSlot)
		{
			itemSlot.transform.position = output.transform.position;
			output.itemSlot = itemSlot;
		}
	}

	public override void RemovedAction()
    {
        grid.placeObjects.Remove(transform.position);
        
        if(output != null)
        {
            output.backBelt = null;
            output.TryAttachBackBelt(HelpFuncs.EulerToVector(output.transform.rotation.eulerAngles.z));
            output.UpdateSprite();
        }
        output = null;

		Destroy(gameObject);
    }
}
