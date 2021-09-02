using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* An IC is a placeable that has at least 1 input and/or output, and holds at least 1 item
 * Some ICs can't have an output, signalled by allowFrontBelt, or input, signalled by allowBackBelt
 * UpdateSprite is necessary for ICs to change their sprits when the world changes around them
 * TryAttach|Front/Back|Belt will try to attach that side, if it can't, that reference will be null
 * MoveItem is necessary so belts can chain together with other ICs so movement can be synced up
 */
public abstract class ItemControl : Placeable
{
	/* Variable Information:
	 * frontBelt refers to the output IC, ICs with multiple outputs might not use this variable
	 * backBelt refers to the input IC, ICs with multiple inputs might not use this variable
	 * itemSlot refers to the item in this IC, ICs with multiple items might not use this variable 
	 * allowFrontBelt will be turned false if this IC cannot have outputs
	 * allowBackBelt will be turned false if this IC cannot have inputs
	 * The transform of this IC should never have a non-integer position, or a rotation that isn't a multiple of 90
	 */
	protected GridControl grid;
	protected bool allowFrontBelt = true;
	protected ItemControl frontBelt = null;
	protected bool allowBackBelt = true;
	protected ItemControl backBelt = null;
	protected GameObject itemSlot = null;

	// Generic Placeable Functions

	/* PlacedAction initializes the variables inside this IC and adds this to the world grid
	 * PRECONDITIONS: There shouldn't be another placeable at this IC's location
	 * POSTCONDITIONS: This IC will be placed into the world grid at this IC's location
	 */
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		AddToWorld(grid, this);
	}

	/* RemovedAction removes this IC, then has its front and back belt update their connections
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this IC is completely destroyed
	 * cont.: This IC will be removed from the dictionary and the world
	 * cont.: This IC's front and back belts will try to find their new attachments now that this is removed
	 * cont.: The item in this IC will be deleted
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

	/* (Only applies to children that have multiple sprites)
	 * UpdateSprite will ensure that this sprite has the correct sprite for its current connections
	 * PRECONDTIONS: The sprites are defined and initialized
	 * POSTCONDITIONS: Only this IC's sprite will be modified, nothing else
	 */
	public virtual void UpdateSprite() { }

	// Front Belt Functions

	/* Pairs this IC's frontBelt with the backBelt of the IC in front of it, if possible
	 * No special preconditions
	 * POSTCONDITIONS: Only the frontBelt of this IC and this IC will be altered
	 */
	public virtual void TryAttachFrontBelt() { }

	// Returns true if this IC can attach to the askingIC as the front belt of this
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

	// Sets the frontBelt of this IC to be newIC
	public virtual void setFrontBelt(ItemControl newIC) { frontBelt = newIC; }

	/* Sets the frontBelt of this IC to be null
	 * This input deletingIC is necessary if this IC has multiple frontBelts
	 */
	public virtual void setFrontBeltToNull(ItemControl deletingIC) { frontBelt = null; }

	// Back Belt Functions

	/* Attaches this IC's backBelt
	 * No special preconditions
	 * POSTCONDITIONS: Only the backBelt of this IC and this IC will be altered
	 */
	public virtual void TryAttachBackBelt() { }

	// Returns if this IC can attach to the askingIC as the back belt of this
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

	// Sets the backBelt of this IC to be newIC
	public virtual void setBackBelt(ItemControl newIC) { backBelt = newIC; }

	/* Sets the backBelt of this IC to be null
	 * This input deletingIC is necessary if this IC has multiple backBelts
	 */
	public virtual void setBackBeltToNull(ItemControl deletingIC) { backBelt = null; }

	// Item slot things

	/* Moves the Item in an IC forward one if it can, then calls its BackBelt behind it to do the same
	 * PRECONDITIONS: There cannot be a loop
	 * cont.: The frontBelt of this one has already done MoveItem, or this has no frontBelt
	 * POSTCONDITIONS: Each item will only move forward up to 1 belt per cycle
		(Currently broken in the event a Splitter's front-left side calls MoveItem before the front-right side)
	 * cont.: An item will never move if its frontBelt already has an item
	 */
	public abstract void MoveItem(ItemControl pullingIC);

	// Returns if this IC can allow an item to be placed in it
	public virtual bool AllowItem(ItemControl askingIC)
	{
		if (itemSlot)
			return false;
		return true;
	}

	/* Sets the itemSlot of this IC to be item
	 * This input askingIC is necessary if this IC has multiple backBelts
	 */
	public virtual void setItemSlot(ItemControl askingIC, GameObject item) { itemSlot = item; }
}

