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

	public bool hasRecived = false;

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
		MoveItem();
	}

	public void MoveItem()
	{
		if (frontBelt == null)
		{
			return;
		}

		hasRecived = false;

		if (!CheckForward())
		{
			PulseBack();
			frontBelt.hasRecived = true;
			return;
		}

		frontBelt.itemSlot = itemSlot;

		itemSlot.transform.position = frontBelt.transform.position;

		itemSlot = null;

		frontBelt.hasRecived = true;

		Debug.Log("Item Moved");

		PulseBack();
	}

	public void PulseBack()
	{
		if (backBelt != null)
		{
			backBelt.MoveItem();
		}
	}

	public bool CheckForward()
	{
		if (itemSlot != null && frontBelt.itemSlot == null && frontBelt.hasRecived == false)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
