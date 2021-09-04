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
		inputValidRelPoses.Add(-transform.up);
		inputValidRelPoses.Add(-transform.right);
		inputValidRelPoses.Add(transform.right);
		outputValidRelPoses.Add(transform.up);
		base.PlacedAction(grid_);
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

		if (inputICs[0] == null)
			return;

		// This gets the closest position of inputIC to this belt, therefore being the connection probably
		// Note that this is incredibly jenk, but the best possible solution at the moment
		List<Vector3> backPositions = inputICs[0].getAllPositions();
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
		if (inputICs[0])
			inputICs[0].MoveItem(this);
	}

	public void InsertItem(GameObject newItem)
	{
		if (itemSlots[0])
			return;

		itemSlots[0] = Instantiate(newItem, transform.position, Quaternion.identity, transform.parent);
	}
}