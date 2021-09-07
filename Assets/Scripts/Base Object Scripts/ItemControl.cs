using System;
using System.Collections.Generic;
using UnityEngine;
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
	protected List<Vector3> inputValidRelPoses = new List<Vector3>();
	protected List<Vector3> outputValidRelPoses = new List<Vector3>();
	public ItemControl[] inputICs = new ItemControl[1];
	public ItemControl[] outputICs = new ItemControl[1];
	public GameObject[] itemSlots = new GameObject[1];

	// Generic Placeable Functions

	public override void PlacedAction(GridControl grid_)
	{
		base.PlacedAction(grid_);
		TryAttachInputs();
		TryAttachOutputs();
		if (inputValidRelPoses.Count > 0)
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

		for (int i = 0; i < inputICs.Length; i++)
		{
			if (inputICs[i])
				inputICs[i].TryAttachOutputs();
			inputICs[i] = null;
		}

		for (int i = 0; i < outputICs.Length; i++)
		{
			if (outputICs[i])
				outputICs[i].TryAttachInputs();
			outputICs[i] = null;
		}

		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (itemSlots[i])
				Destroy(itemSlots[i]);
			itemSlots[i] = null;
		}
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
		if (outputValidRelPoses.Count > 0)
		{
			// Resets all connections this currently has
			for (int i = 0; i < outputICs.Length; i++)
			{
				if (outputICs[i])
				{
					outputICs[i].setInput(null);
					outputICs[i] = null;
				}
			}
			// Tries to attach a connection at every valid position
			foreach (Vector3 validRelPos in outputValidRelPoses)
				TryAttachOutputHelper(grid, this, validRelPos);
		}
	}

	// Returns true if this IC can attach to the askingIC as the output IC of this
	public virtual bool AllowOutputTo(ItemControl askingIC)
	{
		// If every slot is full, then this can't accept an output
		for (int i = 0; i < outputICs.Length + 1; i++)
		{
			if (i == outputICs.Length)
				return false;
			if (outputICs[i] == null)
				break;
		}
		// Tries to see if askingIC has any position in a valid spot
		foreach (Vector3 validRelPos in outputValidRelPoses)
			foreach (Vector3 askingPos in askingIC.getAllPositions())
				if (askingPos - transform.position == validRelPos)
					return true;
		return false;
	}

	// Sets the outputIC of this IC to be newIC
	public virtual void setOutput(ItemControl newIC) { outputICs[0] = newIC; }

	// Input IC Functions

	/* Attaches this IC's inputIC
	 * No special preconditions
	 * POSTCONDITIONS: Only the inputIC of this IC and this IC will be altered
	 */
	public virtual void TryAttachInputs()
	{
		if (inputValidRelPoses.Count > 0)
		{
			// Resets all connections this currently has
			for (int i = 0; i < inputICs.Length; i++)
				if (inputICs[i])
				{
					inputICs[i].setOutput(null);
					inputICs[i] = null;
				}
			// Tries to attach a connection at every valid position
			foreach (Vector3 validRelPos in inputValidRelPoses)
				TryAttachInputHelper(grid, this, validRelPos);
		}
	}

	// Returns if this IC can attach to the askingIC as the input IC of this
	public virtual bool AllowInputFrom(ItemControl askingIC)
	{
		// If every slot is full, then this can't accept an input
		for (int i = 0; i < inputICs.Length + 1; i++)
		{
			if (i == inputICs.Length)
				return false;
			if (inputICs[i] == null)
				break;
		}
		// Tries to see if askingIC has any position in a valid spot
		foreach (Vector3 validRelPos in inputValidRelPoses)
			foreach (Vector3 askingPos in askingIC.getAllPositions())
				if (askingPos - transform.position == validRelPos)
					return true;
		return false;
	}

	// Sets the inputIC of this IC to be newIC
	public virtual void setInput(ItemControl newIC) { inputICs[0] = newIC; }

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
		if (pullingIC && pullingIC == outputICs[0])
			MoveItemHelper(pullingIC, false);
	}
	protected void MoveItemHelper(ItemControl pullingIC, bool doChain)
	{
		// If this IC can move its item forward legally and immediately
		if (pullingIC && itemSlots[0] && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlots[0], itemSlots[0].transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemSlots[0]);
			itemSlots[0] = null;
		}
		if (doChain && inputICs[0])
			inputICs[0].MoveItem(this);
	}

	// Returns if this IC can allow an item to be placed in it
	public virtual bool AllowItem(ItemControl askingIC)
	{
		for (int i = 0; i < itemSlots.Length; i++)
			if (!itemSlots[i])
				return true;
		return false;
	}

	/* Sets the itemSlots[0] of this IC to be item
	 * This input askingIC is necessary if this IC has multiple inputICs
	 */
	public virtual void setItemSlot(ItemControl askingIC, GameObject item) { itemSlots[0] = item; }

	// If this IC is at the front of the line, start a chain reaction backwards of movement
	public virtual void BeltCycle(object sender, EventArgs e)
	{
		if (outputValidRelPoses.Count == 0 || outputICs[0] == null)
			MoveItem(null);
	}
}

