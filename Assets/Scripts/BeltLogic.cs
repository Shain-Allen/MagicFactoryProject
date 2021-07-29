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

	enum Direction
	{
		UP = 0,
		LEFT = 90,
		DOWN = 180,
		RIGHT = 270
	}

	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;

		AttachBelts();

		grid.OnBeltTimerCycle += BeltCycle;
	}

	public void AttachBelts()
	{
		switch (transform.rotation.eulerAngles.z)
		{
			case (int)Direction.UP:
				TryAttachBelts(Vector3.up);
				break;
			case (int)Direction.LEFT:
				TryAttachBelts(Vector3.left);
				break;
			case (int)Direction.DOWN:
				TryAttachBelts(Vector3.down);
				break;
			case (int)Direction.RIGHT:
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
			if (temp.GetComponent<BeltLogic>() == null || temp.transform.rotation.eulerAngles.z == (transform.rotation.eulerAngles.z + 180) % 360)
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
			if (temp.GetComponent<BeltLogic>() == null || temp.transform.rotation.eulerAngles.z != transform.rotation.eulerAngles.z)
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
			switch (transform.rotation.eulerAngles.z)
			{
				case (int)Direction.UP:
					TryAttachCorners(Vector3.left, Vector3.right, (int)Direction.RIGHT, (int)Direction.LEFT);
					break;
				case (int)Direction.LEFT:
					TryAttachCorners(Vector3.down, Vector3.up, (int)Direction.UP, (int)Direction.DOWN);
					break;
				case (int)Direction.DOWN:
					TryAttachCorners(Vector3.right, Vector3.left, (int)Direction.LEFT, (int)Direction.RIGHT);
					break;
				case (int)Direction.RIGHT:
					TryAttachCorners(Vector3.up, Vector3.down, (int)Direction.DOWN, (int)Direction.UP);
					break;
			}
		}
	}

	private void TryAttachCorners(Vector3 leftSide, Vector3 rightSide, int connectionAngleLeftSide, int connectionAngleRightSide)
	{
		GameObject temp = null;

		if (grid.placeObjects.TryGetValue((transform.position + leftSide), out temp))
		{
			if (temp.GetComponent<BeltLogic>() == null || temp.transform.rotation.eulerAngles.z != connectionAngleLeftSide)
			{
				backBelt = null;
			}
			else
			{
				backBelt = temp.GetComponent<BeltLogic>();
				backBelt.frontBelt = this;
			}
		}
		else if (grid.placeObjects.TryGetValue((transform.position + rightSide), out temp))
		{
			if (temp.GetComponent<BeltLogic>() == null || temp.transform.rotation.eulerAngles.z != connectionAngleRightSide)
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
			//Debug.Log("Am Front Belt");
			MoveItem();
		}
	}

	public void MoveItem()
	{
		if (frontBelt && itemSlot && !frontBelt.itemSlot)
		{
			itemSlot.transform.position = frontBelt.transform.position;
			frontBelt.itemSlot = itemSlot;
			itemSlot = null;
		}

		if (backBelt)
		{
			backBelt.MoveItem();
		}
	}

	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);
		//Debug.Log($"{gameObject.name} Deleted from Dictionary");

		if (backBelt)
			backBelt.AttachBelts();

		backBelt = null;
		if (frontBelt)
			frontBelt.AttachBelts();

		frontBelt = null;
		itemSlot = null;

		Destroy(gameObject);
	}
}
