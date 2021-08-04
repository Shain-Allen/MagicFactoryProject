using System;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;
using static ItemControlHelpers;

public class VoidChestLogic : ItemControl
{
	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowFrontBelt = false;

		AddToWorld(grid, this);
		TryAttachBackBelt();

		grid.OnBeltTimerCycle += BeltCycle;
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void TryAttachBackBelt()
	{
		backBelt = null;
		TryAttachBackBeltHelper(grid, this, 180);
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public void BeltCycle(object sender, EventArgs e)
	{
		MoveItem();
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);

		if (backBelt != null)
		{
			backBelt.frontBelt = null;
			backBelt.UpdateSprite();
		}
		backBelt = null;

		Destroy(gameObject);
	}
}
