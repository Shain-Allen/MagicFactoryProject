using System;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;
using static ItemControlHelpers;

public class BeltLogic : ItemControl
{
	public Sprite straightBelt;
	public Sprite cornerBelt;
	SpriteRenderer spriteRenderer;

	/* [Copy Documentation from Parent Class InvSlot.cs]
	 * For Belts, it also initializes the sprites and subscribes to the BeltCycle
	 */
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

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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

	/* UpdateSprite will ensure that this belt has the correct sprite for its current connections
	 * PRECONDTIONS: The sprites are defined and initialized
	 * POSTCONDITIONS: Only this belt's sprite will be modified, nothing else
	 */
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

	/* BeltCycle subscribes to the cycle for belt movement found in GridControl.Update
	 * Only belts at the front of a line will do anything using this subscription
	 */
	public void BeltCycle(object sender, EventArgs e)
	{
		if (frontBelt == null)
			MoveItem();
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);

		if (backBelt)
		{
			backBelt.frontBelt = null;
			backBelt.UpdateSprite();
		}
		backBelt = null;

		if (frontBelt)
		{
			frontBelt.TryAttachBackBelt();
			frontBelt.UpdateSprite();
		}
		frontBelt = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}
}