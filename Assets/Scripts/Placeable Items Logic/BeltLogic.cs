using System;
using System.Collections;
using UnityEngine;
using static PlaceableHelpers;

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
		TryAttachFrontBelt();
		TryAttachBackBelt();
	}

	public override void TryAttachFrontBelt()
	{
		TryAttachFrontBeltHelper(grid, this);
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
		// Belts should default to straight when it has no backbelt or backbelt is right behind
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;

		if (backBelt == null)
			return;

		float angle = (transform.rotation.eulerAngles.z - backBelt.transform.rotation.eulerAngles.z + 360) % 360;
		if (angle == 0)
			return;
		// Otherwise, it must be a corner belt; if it's turning right, it needs to be flipped
		spriteRenderer.sprite = cornerBelt;
		spriteRenderer.flipX = (angle == 270);
	}

	public override void MoveItem()
	{
		// If this belt can move its item forward legally and immediately
		if (frontBelt && itemSlot && !frontBelt.getItemSlot())
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, frontBelt.transform.position));
			frontBelt.setItemSlot(itemSlot);
			itemSlot = null;
		}

		// Chain reaction backwards
		if (backBelt)
			backBelt.MoveItem();
	}

	public override bool AllowBackBeltFrom(ItemControl askingIC)
	{
		int relativeAngle = getRelativeAngle(this, askingIC);
		if (backBelt || relativeAngle == 0)
			return false;
		return true;
	}

	// If this belt is in front, start a chain reaction backwards for movement
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
			MoveItem();
	}

	public void InsertItem(GameObject newItem)
	{
		if (itemSlot)
			return;

		itemSlot = Instantiate(newItem, transform.position, Quaternion.identity, transform.parent);
	}
}