using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BridgeLogic : ItemControl
{
	public ItemControl leftOut = null;
	public ItemControl frontOut = null;
	public ItemControl rightIn = null;
	public ItemControl backIn = null;
	public GameObject topItem = null; // for the back to front path
	public GameObject bottomItem = null; // for the right to left path

	// Placement in the world
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		grid.OnBeltTimerCycle += BeltCycle;

		AddToWorld(grid, this);
		TryAttachFrontBelt();
		TryAttachBackBelt();
	}
	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this);

		if (backIn)
		{
			backIn.setFrontBeltToNull(this);
			backIn.TryAttachFrontBelt();
		}
		backIn = null;

		if (frontOut)
		{
			frontOut.setBackBeltToNull(this);
			frontOut.TryAttachBackBelt();
		}
		frontOut = null;

		if (rightIn)
		{
			rightIn.setFrontBeltToNull(this);
			rightIn.TryAttachFrontBelt();
		}
		rightIn = null;

		if (leftOut)
		{
			leftOut.setBackBeltToNull(this);
			leftOut.TryAttachBackBelt();
		}
		leftOut = null;

		if (topItem)
			Destroy(topItem);
		topItem = null;

		if (bottomItem)
			Destroy(bottomItem);
		bottomItem = null;

		Destroy(gameObject);
	}

	// Front Belt Stuff
	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this, 0);
		TryAttachBackBeltHelper(grid, this, 90);
	}
	public override bool AllowFrontBeltTo(ItemControl askingIC)
	{
		int relativeAngle = getRelativeAngle(this, askingIC);
		Debug.Log($"Asking if allow front to {relativeAngle}");
		if (relativeAngle == 0 && !frontOut)
			return true;
		if (relativeAngle == 270 && !leftOut)
			return true;
		return false;
	}
	public override void setFrontBelt(ItemControl newIC)
	{
		int relativeAngle = getRelativeAngle(this, newIC);
		Debug.Log($"Setting front to {relativeAngle}");
		if (relativeAngle == 0)
			frontOut = newIC;
		else if (relativeAngle == 270)
			leftOut = newIC;
	}
	public override void setFrontBeltToNull(ItemControl deletingIC)
	{
		int relativeAngle = getRelativeAngle(this, deletingIC);
		if (relativeAngle == 0)
			frontOut = null;
		else if (relativeAngle == 90)
			leftOut = null;
	}

	// Back Belt Stuff
	public override void TryAttachBackBelt()
	{
		TryAttachBackBeltHelper(grid, this, 180);
		TryAttachBackBeltHelper(grid, this, 270);
	}
	public override bool AllowBackBeltFrom(ItemControl askingIC)
	{
		int relativeAngle = getRelativeAngle(this, askingIC);
		Debug.Log($"Asking if allow back from {relativeAngle}");
		if (relativeAngle == 180 && !backIn)
			return true;
		if (relativeAngle == 90 && !rightIn)
			return true;
		return false;
	}
	public override void setBackBelt(ItemControl newIC)
	{
		int relativeAngle = getRelativeAngle(this, newIC);
		Debug.Log($"Setting back from {relativeAngle}");
		if (relativeAngle == 180)
			backIn = newIC;
		else if (relativeAngle == 90)
			rightIn = newIC;
	}
	public override void setBackBeltToNull(ItemControl deletingIC)
	{
		int relativeAngle = getRelativeAngle(this, deletingIC);
		if (relativeAngle == 180)
			backIn = null;
		else if (relativeAngle == 270)
			rightIn = null;
	}

	// Item Stuff
	public override bool AllowItem(ItemControl askingIC)
	{
		if (askingIC == backIn)
			return !topItem;
		else if (askingIC == rightIn)
			return !bottomItem;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			return !topItem;
	}
	public override void setItemSlot(ItemControl askingIC, GameObject item)
	{
		if (askingIC == backIn)
			topItem = item;
		else if (askingIC == rightIn)
			bottomItem = item;
		// If the thing wants to deposit onto here, it can only deposit on top
		else
			topItem = item;
	}
	public override void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC == leftOut)
			MoveItemLeftHelper();
		else if (pullingIC == frontOut)
			MoveItemFrontHelper();
	}
	private void MoveItemLeftHelper()
	{
		if (leftOut && bottomItem && leftOut.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, bottomItem, bottomItem.transform.position, leftOut.transform.position));
			leftOut.setItemSlot(this, bottomItem);
			bottomItem = null;
		}
		if (rightIn)
			rightIn.MoveItem(this);
	}
	private void MoveItemFrontHelper()
	{
		if (frontOut && topItem && frontOut.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, topItem, topItem.transform.position, frontOut.transform.position));
			frontOut.setItemSlot(this, topItem);
			topItem = null;
		}
		if (backIn)
			backIn.MoveItem(this);
	}
	public void BeltCycle(object sender, EventArgs e)
	{
		if (leftOut == null)
			MoveItemLeftHelper();
		if (frontOut == null)
			MoveItemFrontHelper();
	}
}