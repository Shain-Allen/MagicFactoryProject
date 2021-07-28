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

		AttachBelts();

		Debug.Log($"BackBelt: {backBelt}");
		Debug.Log($"FrontBelt: {frontBelt}");


		grid.OnBeltTimerCycle += BeltCycle;
	}

	public void AttachBelts()
	{

		switch (transform.rotation.eulerAngles.z)
		{
			case 0:
				TryAttachBelts(Vector3.up);
				break;
			case 90:
				TryAttachBelts(Vector3.left);
				break;
			case 180:
				TryAttachBelts(Vector3.down);
				break;
			case -90:
				TryAttachBelts(Vector3.right);
				break;
		}
	}

	// Checks if the back belt exists and is pointing towards this one, then connects them
	private void TryAttachBelts(Vector3 direction)
	{
		GameObject temp = null;
		// If there is a belt in front of this one, connect them
		if (grid.placeObjects.TryGetValue((transform.position + direction), out temp))
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
		else
		{
			frontBelt = null;
		}

		// If there is a belt behind this one Connect with it
		if (grid.placeObjects.TryGetValue((transform.position - direction), out temp))
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
		else if (grid.placeObjects.TryGetValue((transform.position + transform.right), out temp))
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
		else if (grid.placeObjects.TryGetValue((transform.position - transform.right), out temp))
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
		else
		{
			backBelt = null;
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

	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);
		//Debug.Log($"{gameObject.name} Deleted from Dictionary");

		backBelt.AttachBelts();
		backBelt = null;
		frontBelt.AttachBelts();
		frontBelt = null;
		itemSlot = null;

		Destroy(gameObject);
	}
}
