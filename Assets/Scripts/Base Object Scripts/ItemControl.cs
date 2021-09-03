using System;
using System.Collections.Generic;
using UnityEngine;
using static ICHelpers;
using static PlaceableHelpers;

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
	 * allowOutputs will be turned false if this IC cannot have outputs
	 * allowInputs will be turned false if this IC cannot have inputs
	 * The transform of this IC should never have a non-integer position, or a rotation that isn't a multiple of 90
	 */
	protected GameObject itemSlot = null;
	protected ItemControl inputIC = null;
	protected ItemControl outputIC = null;
	protected bool allowInputs = true;
	protected bool allowOutputs = true;
	protected List<Vector3> inputValidRelPoses = new List<Vector3>();
	protected List<Vector3> outputValidRelPoses = new List<Vector3>();

	// Generic Placeable Functions

	public override void PlacedAction(GridControl grid_)
	{
		base.PlacedAction(grid_);
		inputValidRelPoses.Add(-transform.up);
		outputValidRelPoses.Add(transform.up);
		TryAttachInputs();
		TryAttachOutputs();
		if (allowInputs)
			grid.OnBeltTimerCycle += BeltCycle;
	}

	/* RemovedAction removes this IC, then has its output and input IC update their connections
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this IC is completely destroyed
	 * cont.: This IC will be removed from the dictionary and the world
	 * cont.: This IC's output and input ICs will try to find their new attachments now that this is removed
	 * cont.: The item in this IC will be deleted
	 */
	public override void RemovedAction()
	{
		base.RemovedAction();

		if (inputIC)
			inputIC.TryAttachOutputs();
		inputIC = null;

		if (outputIC)
			outputIC.TryAttachInputs();
		outputIC = null;

		if (itemSlot)
			Destroy(itemSlot);
		itemSlot = null;
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
	public virtual void TryAttachOutputs()
	{
		outputIC = null;
		if (allowOutputs)
			foreach (Vector3 validRelPos in outputValidRelPoses)
				TryAttachOutputHelper(grid, this, validRelPos);
	}

	// Returns true if this IC can attach to the askingIC as the output IC of this
	public virtual bool AllowOutputTo(ItemControl askingIC)
	{
		if (!allowOutputs || outputIC)
			return false;
		foreach (Vector3 pos in outputValidRelPoses)
			if (askingIC.transform.position - transform.position == pos)
				return true;
		return false;
	}

	// Sets the outputIC of this IC to be newIC
	public virtual void setOutput(ItemControl newIC) { outputIC = newIC; }

	// Input IC Functions

	/* Attaches this IC's inputIC
	 * No special preconditions
	 * POSTCONDITIONS: Only the inputIC of this IC and this IC will be altered
	 */
	public virtual void TryAttachInputs()
	{
		inputIC = null;
		if (allowInputs)
			foreach (Vector3 validRelPos in inputValidRelPoses)
				TryAttachInputHelper(grid, this, validRelPos);
	}

	// Returns if this IC can attach to the askingIC as the input IC of this
	public virtual bool AllowInputFrom(ItemControl askingIC)
	{
		if (!allowInputs || inputIC)
			return false;
		foreach (Vector3 pos in inputValidRelPoses)
			if (askingIC.transform.position - transform.position == pos)
				return true;
		return false;
	}

	// Sets the inputIC of this IC to be newIC
	public virtual void setInput(ItemControl newIC) { inputIC = newIC; }

	// Item slot things

	/* Moves the Item in an IC forward one if it can, then calls its InputIC behind it to do the same
	 * PRECONDITIONS: There cannot be a loop
	 * cont.: The outputIC of this one has already done MoveItem, or this has no outputIC
	 * POSTCONDITIONS: Each item will only move forward up to 1 IC per cycle
		(Currently broken in the event a Splitter's output-left side calls MoveItem before the output-right side)
	 * cont.: An item will never move if its outputIC already has an item
	 */
	public virtual void MoveItem(ItemControl pullingIC)
	{
		// If this IC can move its item forward legally and immediately
		if (pullingIC && pullingIC == outputIC && itemSlot && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlot, itemSlot.transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemSlot);
			itemSlot = null;
		}
	}

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

	// If this IC is at the front of the line, start a chain reaction backwards of movement
	public virtual void BeltCycle(object sender, EventArgs e)
	{
		if (!allowOutputs || outputIC == null)
			MoveItem(null);
	}
}

