using System;
using System.Collections.Generic;
using UnityEngine;
using static ICHelpers;

/* ItemControl (IC) are used to move items around the world
 * An IC should have at least 1 input or output IC, and should hold at least 1 item
 * TryAttach[In/Out]puts will try to attach an in-out pairing with an eligible legal IC
 * Before attaching in/outputs, AllowOutputTo/AllowInputFrom should be called to ensure it is legal
 * MoveItem will move the item from this IC to the output IC
 * MoveItem is called by the beltCycle, but it synced with other ICs by travels back by chaining calls
 */

/* See Base Class for further documentation for all override functions */
public abstract class ItemControl : Placeable
{
	// [in/out]putValidRel(ative)Pos(itions) store the positions relative to the position that are eligible to be an in/output
	protected List<Vector3> inputValidRelPoses = new List<Vector3>();
	protected List<Vector3> outputValidRelPoses = new List<Vector3>();
	protected ItemControl[] inputICs = new ItemControl[1];
	protected ItemControl[] outputICs = new ItemControl[1];
	protected GameObject[] itemSlots = new GameObject[1];

	// Generic Placeable Functions

	public override void PlacedAction(GridControl grid_)
	{
		base.PlacedAction(grid_);
		TryAttachInputs();
		TryAttachOutputs();
		// If this can have inputs, then this might be the front of a chain
		if (inputValidRelPoses.Count > 0)
			grid.OnBeltTimerCycle += BeltCycle;
	}

	// Note: The items inside this IC will be deleted along with this currently
	public override void RemovedAction()
	{
		base.RemovedAction();

		if (inputValidRelPoses.Count > 0)
			grid.OnBeltTimerCycle -= BeltCycle;

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

	// outputIC Functions

	/* Pairs this IC's outputIC with the inputIC of an eligible IC
	 * No special preconditions
	 * POSTCONDITIONS: Only this IC, the new outputICs, and the former outputICs could be altered
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

	// Returns true if askingIC is an eligible outputIC of this
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

	// Sets newIC to be an outputIC of this
	public virtual void setOutput(ItemControl newIC) { outputICs[0] = newIC; }

	// inputIC Functions

	/* Pairs this IC's inputIC with the outputIC of an eligible IC
	 * No special preconditions
	 * POSTCONDITIONS: Only this IC, the new inputICs, and the former inputICs could be altered
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

	// Returns true if askingIC is an eligible inputIC of this
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

	// Sets newIC to be an inputIC of this
	public virtual void setInput(ItemControl newIC) { inputICs[0] = newIC; }

	// itemSlot Functions

	/* Moves an item forward
	 * PRECONDITIONS: There cannot be a loop, at all
	 * cont.: The outputIC of this one has already done MoveItem, or this has no outputIC
	 * POSTCONDITIONS: Moves an item to the pullingIC if it is legal
		(Currently broken in the event a Splitter's output-left side calls MoveItem before the output-right side)
	 * cont.: Inheriting ICs should chain MoveItem backwards if they have an input IC
	 */
	public virtual void MoveItem(ItemControl pullingIC)
	{
		if (pullingIC && pullingIC == outputICs[0])
			MoveItemHelper(pullingIC, 0, false, 0);
	}
	// Moves the item in itemSlots[slotNumber] to pullingIC if eligible, then calls MoveItem for inputICs[inputNumberForChain] is doChain is true
	// Doing this allows more customizability for inheriting ICs without repeated code
	protected void MoveItemHelper(ItemControl pullingIC, bool doChain) { MoveItemHelper(pullingIC, 0, doChain, 0); }
	protected void MoveItemHelper(ItemControl pullingIC, int slotNumber, bool doChain, int inputNumberForChain)
	{
		// If this IC can move its item forward legally and immediately
		if (pullingIC && itemSlots[slotNumber] && pullingIC.AllowItem(this))
		{
			StartCoroutine(SmoothMove(grid, itemSlots[slotNumber], itemSlots[slotNumber].transform.position, pullingIC.transform.position));
			pullingIC.setItemSlot(this, itemSlots[slotNumber]);
			itemSlots[slotNumber] = null;
		}
		if (doChain && inputICs[inputNumberForChain])
			inputICs[inputNumberForChain].MoveItem(this);
	}

	// Returns true if askingIC can place an item into this
	public virtual bool AllowItem(ItemControl askingIC)
	{
		for (int i = 0; i < itemSlots.Length; i++)
			if (!itemSlots[i])
				return true;
		return false;
	}

	/* Sets an itemSlot to be the item being given to it
	 * This input askingIC is necessary if this IC has multiple inputICs
	 */
	public virtual void setItemSlot(ItemControl askingIC, GameObject item) { itemSlots[0] = item; }

	// If this IC is at the front of the line, start a chain reaction backwards of MoveItem
	public virtual void BeltCycle(object sender, EventArgs e)
	{
		if (outputValidRelPoses.Count == 0 || outputICs[0] == null)
			MoveItem(null);
	}
}

