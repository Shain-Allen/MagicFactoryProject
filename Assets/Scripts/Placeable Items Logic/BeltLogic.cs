using System;
using UnityEngine;
using static ItemControlHelpers;

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

		AddToWorld(grid, this);
		TryAttachFrontBelt();
		TryAttachBackBelt();

		grid.OnBeltTimerCycle += BeltCycle;
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
		UpdateSprite();
	}

	public override void TryAttachBackBelt()
	{
		// If it can't attach to the one behind it, try its left side, then its right
		TryAttachBackBeltHelper(grid, this, 180);
		if (backBelt == null)
			TryAttachBackBeltHelper(grid, this, 90);
		if (backBelt == null)
			TryAttachBackBeltHelper(grid, this, 270);

		UpdateSprite();
	}

	public override void UpdateSprite()
	{
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;

		// Belts should default to straight when it has no backbelt
		if (backBelt == null)
			return;

		float frontAngle = transform.rotation.eulerAngles.z;
		float backAngle = backBelt.transform.rotation.eulerAngles.z;

		// If this belt is facing the exact same way as the one behind it, it's straight
		if ((frontAngle - backAngle + 360) % 360 == 0)
			return;

		// Otherwise, it must be a corner belt; if it's turning right, it needs to be flipped
		spriteRenderer.sprite = cornerBelt;
		if ((frontAngle - backAngle + 360) % 360 == 270)
			spriteRenderer.flipX = true;
		else
			spriteRenderer.flipX = false;
	}

	public override void MoveItem()
	{
		// If this belt can move its item forward legally and immediately
		if (frontBelt && itemSlot && !frontBelt.itemSlot)
		{
			itemSlot.transform.position = frontBelt.transform.position;
			frontBelt.itemSlot = itemSlot;
			itemSlot = null;
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem();
	}

	/* BeltCycle subscribes to the cycle for belt movement found in GridControl.Update
	 * Only belts at the front of a line will do anything using this subscription
	 */
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
			MoveItem();
	}
}