using System;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;
using static ItemControlHelpers;

public class FullChestLogic : ItemControl
{
	public GameObject itemToClone;

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		allowFrontBelt = true;
		allowBackBelt = false;

		AddToWorld(grid, this);
		TryAttachFrontBelt();
	}

	public override void TryAttachFrontBelt()
	{
		frontBelt = null;
		TryAttachFrontBeltHelper(grid, this);
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void MoveItem()
	{
		if (frontBelt && !frontBelt.itemSlot)
		{
			frontBelt.itemSlot = Instantiate(itemToClone, frontBelt.transform.position, Quaternion.identity, grid.transform);
		}
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);

		if (frontBelt != null)
		{
			frontBelt.backBelt = null;
			frontBelt.TryAttachBackBelt();
			frontBelt.UpdateSprite();
		}
		frontBelt = null;

		Destroy(gameObject);
	}
}
