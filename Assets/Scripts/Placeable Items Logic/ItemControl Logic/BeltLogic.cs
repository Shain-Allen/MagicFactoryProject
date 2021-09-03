using System;
using System.Collections.Generic;
using UnityEngine;
using static MathHelpers;
using static ICHelpers;

/* For all overriding methods, check ItemControl.cs for further documentation*/
public class BeltLogic : ItemControl
{
	public Sprite straightBelt;
	public Sprite cornerBelt;
	SpriteRenderer spriteRenderer;

	public override void PlacedAction(GridControl grid_)
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		// The addition of an extra back and the removeAt are done to give priority to attaching behind first
		inputValidRelPoses.Add(-Vector3.up);
		inputValidRelPoses.Add(-Vector3.right);
		inputValidRelPoses.Add(Vector3.right);
		base.PlacedAction(grid_);
		inputValidRelPoses.RemoveAt(3);
	}

	public override void TryAttachInputs()
	{
		base.TryAttachInputs();
		UpdateSprite();
	}

	public override void UpdateSprite()
	{
		// Belts should default to straight when it has no inputIC or inputIC is right behind
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;

		if (inputIC == null)
			return;

		// This gets the closest position of inputIC to this belt, therefore being the connection probably
		// Note that this is incredibly jenk, but the best possible solution at the moment
		List<Vector3> backPositions = inputIC.getAllPositions();
		Vector3 backConnectionPos = backPositions[0];
		foreach (Vector3 pos in backPositions)
			if (GetDistance(backConnectionPos, this.transform.position) > GetDistance(pos, this.transform.position))
				backConnectionPos = pos;

		float angle = getRelativeAngle(this, backConnectionPos);
		if (angle == 180)
			return;
		// Otherwise, it must be a corner belt; if it's turning right, it needs to be flipped
		spriteRenderer.sprite = cornerBelt;
		spriteRenderer.flipX = (angle == 90);
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		base.MoveItem(pullingIC);

		// Chain reaction backwards
		if (inputIC)
			inputIC.MoveItem(this);
	}

	public override bool AllowInputFrom(ItemControl askingIC)
	{
		int relativeAngle = getRelativeAngle(this, askingIC);
		if (inputIC || relativeAngle == 0)
			return false;
		return true;
	}

	public void InsertItem(GameObject newItem)
	{
		if (itemSlot)
			return;

		itemSlot = Instantiate(newItem, transform.position, Quaternion.identity, transform.parent);
	}
}