using System;
using UnityEngine;
using static PlaceableHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class FullChestLogic : ItemControl
{
	public GameObject itemToClone;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowBackBelt = false;

		AddToWorld(grid, this);
		TryAttachFrontBelt();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
	}

	public override void MoveItem()
	{
		if (frontBelt && !frontBelt.getItemSlot())
			frontBelt.setItemSlot(Instantiate(itemToClone, frontBelt.transform.position, Quaternion.identity, grid.transform));
	}
}
