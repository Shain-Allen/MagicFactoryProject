using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : Placeable
{
	GridControl grid;
	// Reference to the Belt in front of this one
	public BeltLogic frontBelt;
	// Reference to the Belt in behind this one
	// Note, only stores 1 belt that is pointing to this one
	// Therefore there must be a priority system for determining which one it gets
	public BeltLogic backBelt;
	// Reference to the Item currently in this belt
	public GameObject itemSlot;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		GameObject temp = null;

		// If there is a belt in front of this one, connect them
		if (grid.placeObjects.TryGetValue((transform.position + transform.right), out temp))
		{
			if (temp.GetComponent<BeltLogic>() == null)
			{
				frontBelt = null;
			}
			else
			{
				frontBelt = temp.GetComponent<BeltLogic>();
				frontBelt.backBelt = this;
			}
		}
		
		backBelt = null;
		// If there is a belt directly behind this one and points to this, connect them
		// Tries again for left, then right, if they don't attach
		if (grid.placeObjects.TryGetValue((transform.position - transform.right), out temp))
		{
			TryToAttachBeltsHelper(temp);
		}
		if (backBelt == null && grid.placeObjects.TryGetValue((transform.position + transform.up), out temp))
		{
			TryToAttachBeltsHelper(temp);
		}
		if (backBelt == null && grid.placeObjects.TryGetValue((transform.position - transform.up), out temp))
		{
			TryToAttachBeltsHelper(temp);
		}

		grid.OnBeltTimerCycle += BeltCycle;
	}

	// Checks if the back belt exists and is pointing towards this one, then connects them
	private void TryToAttachBeltsHelper(GameObject back)
	{
		if (back.GetComponent<BeltLogic>() == null)
			return;
		if (back.transform.position + back.transform.right == transform.position)
		{
			backBelt = back.GetComponent<BeltLogic>();
			backBelt.frontBelt = this;
			return;
		}
	}

	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
		{
			MoveItem();
		}
	}

	public void MoveItem()
	{
		if (frontBelt == null)
		{
			if (backBelt != null)
			{
				backBelt.MoveItem();
			}
			return;
		}

		if (itemSlot == null)
		{
			if (backBelt != null)
			{
				backBelt.MoveItem();
			}
			return;
		}

		if (frontBelt.itemSlot != null)
		{
			if (backBelt != null)
			{
				backBelt.MoveItem();
			}
			return;
		}

		itemSlot.transform.position = frontBelt.transform.position;
		frontBelt.itemSlot = itemSlot;
		itemSlot = null;

		if (backBelt != null)
		{
			backBelt.MoveItem();
		}
	}
}
