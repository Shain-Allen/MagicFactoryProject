using System;
using System.Collections.Generic;
using UnityEngine;
using static MathHelpers;
using static PlaceableHelpers;
using static ICHelpers;

/* For all overriding methods without documentation, check ItemControl.cs */
public class BeltLogic : ItemControl
{
	public Sprite straightBelt;
	public Sprite cornerBelt;
	SpriteRenderer spriteRenderer;

	public override void PlacedAction(GridControl grid_)
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		grid = grid_;
		grid.OnBeltTimerCycle += BeltCycle;

		AddToWorld(grid, this);
		TryAttachOutputs();
		TryAttachInputs();
	}

	public override void TryAttachOutputs()
	{
		TryAttachOutputHelper(grid, this);
	}

	public override void TryAttachInputs()
	{
		// If it can't attach to the one behind it, try its left side, then its right
		TryAttachInputHelper(grid, this, 180);
		if (inputIC == null)
			TryAttachInputHelper(grid, this, 90);
		if (inputIC == null)
			TryAttachInputHelper(grid, this, 270);

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
		// If this belt can move its item forward legally and immediately
		if (pullingIC && itemSlot && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemSlot);
			itemSlot = null;
		}

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

	// If this belt is in output, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (outputIC == null)
			MoveItem(null);
	}

	public void InsertItem(GameObject newItem)
	{
		if (itemSlot)
			return;

		itemSlot = Instantiate(newItem, transform.position, Quaternion.identity, transform.parent);
	}
}