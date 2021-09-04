using System;
using UnityEngine;

/* For all overriding methods without documentation, check ItemControl.cs */
public class VoidChestLogic : ItemControl
{
	public override void PlacedAction(GridControl grid_)
	{
		inputValidRelPoses.Add(-transform.up);
		base.PlacedAction(grid_);
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		if (itemSlots[0])
		{
			Destroy(itemSlots[0]);
			itemSlots[0] = null;
		}
		if (inputICs[0])
			inputICs[0].MoveItem(this);
	}
}
