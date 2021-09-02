using System;
using UnityEngine;

/* For all overriding methods without documentation, check ItemControl.cs */
public class VoidChestLogic : ItemControl
{
	public override void PlacedAction(GridControl grid_)
	{
		allowOutputs = false;
		base.PlacedAction(grid_);
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		if (itemSlot)
		{
			Destroy(itemSlot);
			itemSlot = null;
		}
		if (inputIC)
			inputIC.MoveItem(this);
	}
}
