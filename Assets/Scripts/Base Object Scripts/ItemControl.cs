using UnityEngine;
using static PlaceableHelpers;
using static ICHelpers;

/* An IC is a placeable that has at least 1 input and/or output, and holds at least 1 item
 * Some ICs can't have an output, signalled by allowOutputTo, or input, signalled by allowInputFrom
 * UpdateSprite is necessary for ICs to change their sprits when the world changes around them
 * TryAttach|In/Out|puts will try to attach that side, if it can't, that reference will be null
 * MoveItem is necessary so ICs can chain together with other ICs so movement can be synced up
 */

/* For all overriding methods, check Placeable.cs for further documentation */
public abstract class ItemControl : Placeable
{
	/* Variable Information:
	 * outputIC refers to the output IC, ICs with multiple outputs might not use this variable
	 * inputIC refers to the input IC, ICs with multiple inputs might not use this variable
	 * itemSlot refers to the item in this IC, ICs with multiple items might not use this variable 
	 * allowOutputTo will be turned false if this IC cannot have outputs
	 * allowInputFrom will be turned false if this IC cannot have inputs
	 * The transform of this IC should never have a non-integer position, or a rotation that isn't a multiple of 90
	 */
	protected bool allowOutputs = true;
	protected ItemControl outputIC = null;
	protected bool allowInputs = true;
	protected ItemControl inputIC = null;
	protected GameObject itemSlot = null;

	// Generic Placeable Functions

	/* RemovedAction removes this IC, then has its output and input IC update their connections
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this IC is completely destroyed
	 * cont.: This IC will be removed from the dictionary and the world
	 * cont.: This IC's output and input ICs will try to find their new attachments now that this is removed
	 * cont.: The item in this IC will be deleted
	 */
	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this);

		if (inputIC)
		{
			inputIC.setOutputToNull(this);
			inputIC.TryAttachOutputs();
		}
		inputIC = null;

		if (outputIC)
		{
			outputIC.setInputToNull(this);
			outputIC.TryAttachInputs();
		}
		outputIC = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;

		Destroy(gameObject);
	}

	/* (Only applies to children that have multiple sprites)
	 * UpdateSprite will ensure that this IC has the correct sprite based its current inputs and outputs
	 * PRECONDTIONS: The sprites are defined and initialized
	 * POSTCONDITIONS: Only this IC's sprite will be modified, nothing else
	 */
	public virtual void UpdateSprite() { }

	// Output IC Functions

	/* Pairs this IC's outputIC with the inputIC of the IC in output of it, if possible
	 * No special preconditions
	 * POSTCONDITIONS: Only the outputIC of this IC and this IC will be altered
	 */
	public virtual void TryAttachOutputs() { }

	// Returns true if this IC can attach to the askingIC as the output IC of this
	public virtual bool AllowOutputTo(ItemControl askingIC)
	{
		if (!allowOutputs || outputIC)
			return false;
		foreach (Vector3 pos in askingIC.getAllPositions())
		{
			int relativeAngle = getRelativeAngle(this, pos);
			if (relativeAngle == 0)
				return true;
		}
		return false;
	}

	// Sets the outputIC of this IC to be newIC
	public virtual void setOutput(ItemControl newIC) { outputIC = newIC; }

	/* Sets the outputIC of this IC to be null
	 * This input deletingIC is necessary if this IC has multiple outputICs
	 */
	public virtual void setOutputToNull(ItemControl deletingIC) { outputIC = null; }

	// Input IC Functions

	/* Attaches this IC's inputIC
	 * No special preconditions
	 * POSTCONDITIONS: Only the inputIC of this IC and this IC will be altered
	 */
	public virtual void TryAttachInputs() { }

	// Returns if this IC can attach to the askingIC as the input IC of this
	public virtual bool AllowInputFrom(ItemControl askingIC)
	{
		if (!allowInputs || inputIC)
			return false;
		foreach (Vector3 pos in askingIC.getAllPositions())
		{
			int relativeAngle = getRelativeAngle(this, pos);
			if (relativeAngle == 180)
				return true;
		}
		return false;
	}

	// Sets the inputIC of this IC to be newIC
	public virtual void setInput(ItemControl newIC) { inputIC = newIC; }

	/* Sets the inputIC of this IC to be null
	 * This input deletingIC is necessary if this IC has multiple inputICs
	 */
	public virtual void setInputToNull(ItemControl deletingIC) { inputIC = null; }

	// Item slot things

	/* Moves the Item in an IC forward one if it can, then calls its InputIC behind it to do the same
	 * PRECONDITIONS: There cannot be a loop
	 * cont.: The outputIC of this one has already done MoveItem, or this has no outputIC
	 * POSTCONDITIONS: Each item will only move forward up to 1 IC per cycle
		(Currently broken in the event a Splitter's output-left side calls MoveItem before the output-right side)
	 * cont.: An item will never move if its outputIC already has an item
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
	 * This input askingIC is necessary if this IC has multiple inputICs
	 */
	public virtual void setItemSlot(ItemControl askingIC, GameObject item) { itemSlot = item; }
}

