using System;
using UnityEngine;
using static ItemControlHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class VoidChestLogic : ItemControl
{
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowFrontBelt = false;

		AddToWorld(grid, this);
		TryAttachBackBelt();

		grid.OnBeltTimerCycle += BeltCycle;
	}

	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this, 180);
	}

	public override void MoveItem()
	{
		if (backBelt)
		{
			if (backBelt.itemSlot)
			{
				Destroy(backBelt.itemSlot);
				backBelt.itemSlot = null;
			}
			backBelt.MoveItem();
		}
	}

	// This is always a front belt, so always start a chain with this
	public void BeltCycle(object sender, EventArgs e)
	{
		MoveItem();
	}
}
