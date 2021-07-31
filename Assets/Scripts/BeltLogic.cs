using System;
using System.Collections.Generic;
using UnityEngine;

public class BeltLogic : InvSlot
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

		allowBackBelt = true;
		allowBackBelt = true;

		TryAttachFrontBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));
		TryAttachBackBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));

		grid.OnBeltTimerCycle += BeltCycle;
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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

	/* [Copy Documentation from Parent Class InvSlot.cs] */
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
			frontBelt.TryAttachBackBelt(HelpFuncs.EulerToVector(frontBelt.transform.rotation.eulerAngles.z));
			frontBelt.UpdateSprite();
		}
		frontBelt = null;
		itemSlot = null;

		Destroy(gameObject);
	}
}
