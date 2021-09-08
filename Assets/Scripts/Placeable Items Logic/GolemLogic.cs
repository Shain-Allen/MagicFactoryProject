using System;
using UnityEngine;
using static PlaceableHelpers;

public class GolemLogic : ItemControl
{
	// movingDir == -1 means its heading to inputIC
	// movingDir == 0 means it's not moving, waiting for something else
	// movingDir == 1 means it's moving to outputIC
	int movingDir = 0;
	const float REACH_DISTANCE = 1.0f;
	public float MOVE_SPEED = 2.5f;

	public override void PlacedAction(GridControl grid_)
	{
		grid.OnBeltTimerCycle += BeltCycle;
		base.PlacedAction(grid_);
	}
	public override void RemovedAction()
	{
		RemoveFromWorld(grid, this, Vector2.zero);
		Destroy(gameObject);
		if (itemSlots[0])
			Destroy(itemSlots[0]);
		itemSlots[0] = null;
		outputICs[0] = null;
		inputICs[0] = null;
	}

	public void MoveGolem()
	{
		if (itemSlots[0])
			movingDir = 1;
		else
			movingDir = -1;

		if (movingDir == 1)
		{
			if (outputICs[0])
			{
				if (!atLocation(outputICs[0]))
				{
					// Move to output
				}
				else
				{
					// Try to deposit item
				}
			}
			// Else, wait for output to exist
		}
		else if (movingDir == -1)
		{
			if (inputICs[0])
			{
				if (!atLocation(inputICs[0]))
				{
					// Move to input
				}
				else
				{
					// Grab and item if it has it
				}
			}
			// Else, wait for input to exist
		}
	}
	public override void BeltCycle(object sender, EventArgs e)
	{
		MoveGolem();
	}

	public override bool AllowItem(ItemControl askingIC) { return !itemSlots[0] && askingIC && askingIC == inputICs[0]; }
	public override void setItemSlot(ItemControl askingIC, GameObject item) { itemSlots[0] = item; }

	private bool atLocation(ItemControl IC) { return atLocation(IC.transform.position); }
	private bool atLocation(Vector3 pos) { return Vector3.Distance(transform.position, pos) < REACH_DISTANCE; }

	public override void MoveItem(ItemControl pullingIC) { }
	public override void TryAttachOutputs() { }
	public override bool AllowOutputTo(ItemControl askingIC) { return false; }
	public override void setOutput(ItemControl newIC) { }
	public override void TryAttachInputs() { }
	public override bool AllowInputFrom(ItemControl askingIC) { return false; }
	public override void setInput(ItemControl newIC) { }
}
