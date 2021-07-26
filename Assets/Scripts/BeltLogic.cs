using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : Placeable
{
	GridControl grid;
	public BeltLogic frontBelt;
	public BeltLogic backBelt;
	public GameObject itemSlot;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		GameObject temp = null;

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

		if (grid.placeObjects.TryGetValue((transform.position - transform.right), out temp))
		{
			if (temp.GetComponent<BeltLogic>() == null)
			{
				backBelt = null;
			}
			else
			{
				backBelt = temp.GetComponent<BeltLogic>();
				backBelt.frontBelt = this;
			}
		}

		grid.OnBeltTimerCycle += BeltCycle;
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
