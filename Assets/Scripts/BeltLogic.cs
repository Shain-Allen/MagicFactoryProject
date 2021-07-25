using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : Placeable
{
	GridControl grid;
	public GameObject frontBelt;
	public GameObject backBelt;
	public GameObject itemSlot;

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		grid.placeObjects.TryGetValue((transform.position + transform.right), out frontBelt);
		grid.placeObjects.TryGetValue((transform.position - transform.right), out backBelt);

		if (frontBelt.GetComponent<BeltLogic>() == null)
		{
			frontBelt = null;
		}
		else
		{
			frontBelt.GetComponent<BeltLogic>().backBelt = gameObject;
		}

		if (backBelt.GetComponent<BeltLogic>() == null)
		{
			backBelt = null;
		}
		else
		{
			backBelt.GetComponent<BeltLogic>().frontBelt = gameObject;
		}

		grid.OnBeltTimerCycle += BeltCycle;
	}

	public void PulseBack()
	{

	}

	public void CheckForward()
	{

	}

	public void BeltCycle(object sender, EventArgs e)
	{
		Debug.Log("Belt event triggered");
	}

	private void OnDestroy()
	{
		grid.OnBeltTimerCycle -= BeltCycle;
	}
}
