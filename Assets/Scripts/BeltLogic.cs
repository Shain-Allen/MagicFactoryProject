using System;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : InvSlot
{
	GridControl grid;
	public Sprite straightBelt;
	public Sprite cornerBelt;
	SpriteRenderer spriteRenderer;

	/* PlacedAction initializes the sprite, attaches belt, and subscribes to the BeltCycle
	 * No special Preconditions
	 * POSTCONDITIONS: The belt will attach to nearby belts if possible as per the rules
	 * cont.: Only Belts directly next to this one will be altered in any way
	 */
	public override void PlacedAction(GridControl grid_)
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		grid = grid_;

		TryAttachFrontBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));
		TryAttachBackBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));

		grid.OnBeltTimerCycle += BeltCycle;
	}

	/* Attaches the currently placing belt the best it can
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * POSTCONDITIONS: Only the belt directly in front of this one will be altered
	 */
	public override void TryAttachFrontBelt(Vector3 direction)
	{
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;
		GameObject temp = null;

		// Attaches frontBelt a belt directly in front of this one if possible
		// Currently would override that belt's previous backBelt
		frontBelt = null;
		InvSlot tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position + direction), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && (!tempInvSlot.allowFrontBelt || temp.transform.rotation.eulerAngles.z != (transform.rotation.eulerAngles.z + 180) % 360))
			{
				if(tempInvSlot.allowBackBelt && tempInvSlot.backBelt == null)
				{
					frontBelt = tempInvSlot;
					frontBelt.backBelt = this;
					frontBelt.UpdateSprite();
				}
			}
		}
	}

	/* Attaches the currently placing belt the best it can
	 * BackBelt is prioritized to be directly behind it, then its left side, then its right
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * POSTCONDITIONS: Only belts pointing to this one might be altered
	 */
	public override void TryAttachBackBelt(Vector3 direction)
	{
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;
		GameObject temp = null;

		// Attaches backBelt to a belt directly behind this one if possible
		backBelt = null;
		InvSlot tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position - direction), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && temp.transform.rotation.eulerAngles.z == transform.rotation.eulerAngles.z)
			{
				if(tempInvSlot.allowFrontBelt && tempInvSlot.frontBelt == null)
				{
					backBelt = tempInvSlot;
					backBelt.frontBelt = this;
					backBelt.UpdateSprite();
				}
			}
		}
		// If it can't attach to the one behind it, try its left side, then its right
		if (backBelt == null)
		{
			int angleLeft = (int)(transform.rotation.eulerAngles.z + 90) % 360;
			int angleRight = (int)(transform.rotation.eulerAngles.z + 270) % 360;
			TryAttachCorners(HelpFuncs.EulerToVector(angleLeft), HelpFuncs.EulerToVector(angleRight), angleRight, angleLeft);
		}
	}

	/* If a belt does not have a valid backBelt directly behind it, this will try to attach one from a side
	 * Prioritizes its Left side over its Right side
	 * PRECONDITIONS: leftSide must be 1 unit long and have the Euler Angle in connectionAngleLeftSide
	 * cont.: Same applies for rightSide and connectionAngleRight
	 * POSTCONDITIONS: Only belts directly next to this one will be altered
	 */
	private void TryAttachCorners(Vector3 leftSide, Vector3 rightSide, int connectionAngleLeftSide, int connectionAngleRightSide)
	{
		GameObject temp = null;
		InvSlot tempInvSlot;

		if (grid.placeObjects.TryGetValue((transform.position + leftSide), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && temp.transform.rotation.eulerAngles.z == connectionAngleLeftSide)
			{
				if(tempInvSlot.allowFrontBelt && tempInvSlot.frontBelt == null)
				{
					backBelt = tempInvSlot;
					backBelt.frontBelt = this;
					backBelt.UpdateSprite();
					spriteRenderer.sprite = cornerBelt;
				}
			}
		}
		if (backBelt == null && grid.placeObjects.TryGetValue((transform.position + rightSide), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && temp.transform.rotation.eulerAngles.z == connectionAngleRightSide)
			{
				if(tempInvSlot.allowFrontBelt && tempInvSlot.frontBelt == null)
				{
					backBelt = tempInvSlot;
					backBelt.frontBelt = this;
					backBelt.UpdateSprite();
					spriteRenderer.sprite = cornerBelt;
					spriteRenderer.flipX = true;
				}
			}
		}
	}

	/* UpdateSprite will ensure that this belt has the correct sprite for its current connections
	 * PRECONDTIONS: The sprites are defined and initialized
	 * POSTCONDITIONS: Only this belt's sprite will be modified, nothing else
	 */
	public override void UpdateSprite()
	{
		// Belts should default to straight when it has no backbelt
		if(backBelt == null)
		{
			spriteRenderer.sprite = straightBelt;
			spriteRenderer.flipX = false;
			return;
		}

		float frontAngle = transform.rotation.eulerAngles.z;
		float backAngle = backBelt.transform.rotation.eulerAngles.z;
		
		// If this belt is facing the exact same way as the one behind it, it's straight
		if((frontAngle - backAngle + 360) % 360 == 0)
		{
			spriteRenderer.sprite = straightBelt;
			spriteRenderer.flipX = false;
			return;
		}

		// Otherwise, it must be a corner belt; if it's turning right, it needs to be flipped
		spriteRenderer.sprite = cornerBelt;
		if((frontAngle - backAngle + 360) % 360 == 270)
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
		{
			MoveItem();
		}
	}

	/* Moves the Item in a belt forward one if it can, then calls the belt behind it to do the same
	 * PRECONDITIONS: This belt chain is not a loop
	 * cont.: The frontBelt of this one has already done MoveItem, or is null
	 * POSTCONDITIONS: Each item will only move forward up to 1 belt per cycle
	 * cont.: An item will never move if its frontBelt already has an item
	 */
	public override void MoveItem()
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

	/* RemovedAction removes this belt, then has its front and back belt Re-TryAttachBelts
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this belt is completely destroyed
	 * cont.: This belt will be removed from the dictionary and the world
	 * cont.: This belt's front and back belts will try to find their new attachments, which might cause issues potentially
	 */
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
			frontBelt.TryAttachBackBelt(HelpFuncs.EulerToVector(frontBelt.transform.rotation.eulerAngles.z));
			frontBelt.UpdateSprite();
		}
		frontBelt = null;
		itemSlot = null;

		Destroy(gameObject);
	}
}
