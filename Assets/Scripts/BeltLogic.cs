using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : Placeable
{
	GridControl grid;
	// Reference to the Belt in front of this one
	public BeltLogic frontBelt;
	// Reference to the Belt pointing to this one
	public BeltLogic backBelt;
	// Reference to the Item currently in this belt
	public GameObject itemSlot;

	public Sprite straightBelt;

	public Sprite cornerBelt;

	SpriteRenderer spriteRenderer;

	enum Direction
	{
		UP = 0,
		LEFT = 90,
		DOWN = 180,
		RIGHT = 270
	}

	public override void PlacedAction(GridControl grid_)
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		grid = grid_;

		TryAttachBelts(HelperFunctions.EulerToUnitCircle(transform.rotation.eulerAngles.z));

		grid.OnBeltTimerCycle += BeltCycle;
	}

	private void TryAttachBelts(Vector3 direction)
	{
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;
		GameObject temp = null;
		frontBelt = null;
		if (grid.placeObjects.TryGetValue((transform.position + direction), out temp))
		{
			if (temp.GetComponent<BeltLogic>() != null && temp.transform.rotation.eulerAngles.z != (transform.rotation.eulerAngles.z + 180) % 360)
			{
				frontBelt = temp.GetComponent<BeltLogic>();
				frontBelt.backBelt = this;
			}
		}

		backBelt = null;
		if (grid.placeObjects.TryGetValue((transform.position - direction), out temp))
		{
			if (temp.GetComponent<BeltLogic>() != null && temp.transform.rotation.eulerAngles.z == transform.rotation.eulerAngles.z)
			{
				backBelt = temp.GetComponent<BeltLogic>();
				backBelt.frontBelt = this;
			}
		}
		if (backBelt == null)
		{
			float angle = transform.rotation.eulerAngles.z;
			float angleLeft = (angle + 90) % 360;
			float angleRight = (angle + 270) % 360;
			TryAttachCorners(HelperFunctions.EulerToUnitCircle(angleLeft), HelperFunctions.EulerToUnitCircle(angleRight), (int)angleRight, (int)angleLeft);
		}
	}

	private void TryAttachCorners(Vector3 leftSide, Vector3 rightSide, int connectionAngleLeftSide, int connectionAngleRightSide)
	{
		GameObject temp = null;

		if (grid.placeObjects.TryGetValue((transform.position + leftSide), out temp))
		{
			if (temp.GetComponent<BeltLogic>() != null && temp.transform.rotation.eulerAngles.z == connectionAngleLeftSide)
			{
				backBelt = temp.GetComponent<BeltLogic>();
				backBelt.frontBelt = this;
				spriteRenderer.sprite = cornerBelt;
			}
		}
		if (backBelt == null && grid.placeObjects.TryGetValue((transform.position + rightSide), out temp))
		{
			if (temp.GetComponent<BeltLogic>() != null && temp.transform.rotation.eulerAngles.z == connectionAngleRightSide)
			{
				backBelt = temp.GetComponent<BeltLogic>();
				backBelt.frontBelt = this;
				spriteRenderer.sprite = cornerBelt;
				spriteRenderer.flipX = true;
			}
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
