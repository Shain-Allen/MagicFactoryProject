using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceableHelpers;

/* An IC currently is an item in the world that has precisely 1 slot for an item in it
 * These slots typically have a frontBelt and/or a backBelt, signalled by allow Front/Back Belt
 * UpdateSprite is necessary for Belts to change their sprits when the world changes
 * TryAttach Front/Back Belt will try to attach that side, otherwise it will set that belt reference to null
 * MoveItem is necessary so belts can chain together with other ICs/Belts so movement can be synced up
 */
public abstract class ItemControl : Placeable
{
	protected bool allowBackBelt = true;
	protected bool allowFrontBelt = true;
	public ItemControl frontBelt = null; // output to IC
	public ItemControl backBelt = null; // input from IC
	protected GameObject itemSlot = null;
	protected GridControl grid;

	public virtual void setFrontBelt(ItemControl newIC) { frontBelt = newIC; }
	public virtual void setFrontBeltToNull(ItemControl deletingIC) { frontBelt = null; }
	public virtual void setBackBelt(ItemControl newIC) { backBelt = newIC; }
	public virtual void setBackBeltToNull(ItemControl deletingIC) { backBelt = null; }
	public virtual void setItemSlot(ItemControl askingIC, GameObject item) { itemSlot = item; }

	public virtual List<Vector3> getAllPositions()
	{
		List<Vector3> toReturn = new List<Vector3>();
		toReturn.Add(transform.position);
		return toReturn;
	}

	/* PlacedAction initializes the variables inside this IC, and attaches to nearby ICs
	 * No special Preconditions
	 * POSTCONDITIONS: The IC will attach to nearby ICs if possible as per the rules
	 * cont.: Only ICs directly next to this one might be altered in any way
	 */
	// public abstract void PlacedAction();

	/* RemovedAction removes this IC, then has its front and back belt Re-TryAttachBelts
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this IC is completely destroyed
	 * cont.: This IC will be removed from the dictionary and the world
	 * cont.: This belt's front and back belts will try to find their new attachments, which might cause issues potentially
	 */
	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this);

		if (backBelt)
		{
			backBelt.setFrontBeltToNull(this);
			backBelt.TryAttachFrontBelt();
		}
		backBelt = null;

		if (frontBelt)
		{
			frontBelt.setBackBeltToNull(this);
			frontBelt.TryAttachBackBelt();
		}
		frontBelt = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}

	/* Pairs this IC's frontBelt with the backBelt of the IC in front of it, if possible
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * cont.: Direction should be the direction this IC is facing
	 * POSTCONDITIONS: Only the IC directly in front of this one can be altered
	 */
	public virtual void TryAttachFrontBelt() { }

	/* Attaches the currently placing belt the best it can
	 * BackBelt is prioritized to be directly behind it, then its left side, then its right
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * POSTCONDITIONS: Only ICs pointing to this one might be altered
	 */
	public virtual void TryAttachBackBelt() { }

	/* (Only applies to children that have multiple sprites)
	 * UpdateSprite will ensure that this sprite has the correct sprite for its current connections
	 * PRECONDTIONS: The sprites are defined and initialized
	 * POSTCONDITIONS: Only this IC's sprite will be modified, nothing else
	 */
	public virtual void UpdateSprite() { }

	/* Moves the Item in an IC forward one if it can, then calls its BackBelt behind it to do the same
	 * PRECONDITIONS: There cannot be a loop
	 * cont.: The frontBelt of this one has already done MoveItem, or is null
	 * POSTCONDITIONS: Each item will only move forward up to 1 belt per cycle
	 * cont.: An item will never move if its frontBelt already has an item
	 */
	public abstract void MoveItem(ItemControl pullingIC);

	// Takes a EulerAngle for the side this is being asked from
	public virtual bool AllowFrontBeltTo(ItemControl askingIC)
	{
		if (!allowFrontBelt || frontBelt)
			return false;
		foreach (Vector3 pos in askingIC.getAllPositions())
		{
			int relativeAngle = getRelativeAngle(this, pos);
			if (relativeAngle == 0)
				return true;
		}
		return false;
	}
	public virtual bool AllowBackBeltFrom(ItemControl askingIC)
	{
		if (!allowBackBelt || backBelt)
			return false;
		foreach (Vector3 pos in askingIC.getAllPositions())
		{
			int relativeAngle = getRelativeAngle(this, pos);
			if (relativeAngle == 180)
				return true;
		}
		return false;
	}
	// This one is a Placeable instead of IC because MysticDrillLogic is an IC
	public virtual bool AllowItem(ItemControl askingIC)
	{
		if (itemSlot)
			return false;
		return true;
	}
}

