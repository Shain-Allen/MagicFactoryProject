using System;
using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class VoidChestLogic : ItemControl
{
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowFrontBelt = false;
		grid.OnBeltTimerCycle += BeltCycle;

		AddToWorld(grid, this);
		TryAttachBackBelt();
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this);
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		if (itemSlot)
		{
			Destroy(itemSlot);
			itemSlot = null;
		}
		if (backBelt)
			backBelt.MoveItem(this);
	}

	// This is always a front belt, so always start a chain with this
	public void BeltCycle(object sender, EventArgs e)
	{
		MoveItem(null);
	}
}
