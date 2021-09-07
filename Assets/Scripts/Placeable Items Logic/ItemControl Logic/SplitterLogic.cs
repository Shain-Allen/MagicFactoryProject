using System;
using UnityEngine;

public class SplitterLogic : ItemControl
{
	// outputICs[0] is the left-side output, outputICs[1] is the right-side output
	int nextOutput = 0;

	public override void PlacedAction(GridControl grid_)
	{
		outputICs = new ItemControl[2];
		relativePositions.Add(transform.right);
		inputValidRelPoses.Add(-transform.up);
		outputValidRelPoses.Add(transform.up);
		outputValidRelPoses.Add(transform.right + transform.up);
		base.PlacedAction(grid_);
	}

	public override void setOutput(ItemControl newIC)
	{
		if (newIC == null)
			TryAttachOutputs();
		else if (newIC.transform.position == transform.position + transform.up)
			outputICs[0] = newIC;
		else if (newIC.transform.position == transform.position + transform.up + transform.right)
			outputICs[1] = newIC;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		// Prevents MoveItem from being called twice each BeltCycle by only responding to calls from left side
		if (pullingIC && pullingIC != outputICs[0])
			return;
		MoveItemHelper(chooseOutputIC(), true);
	}
	private ItemControl chooseOutputIC()
	{
		// Try one side, if that fails then try the other
		// Should perfectly alternate between outputting left and right if allowed
		for (int i = 0; i < 2; i++)
		{
			nextOutput = (nextOutput + 1) % 2;
			if (outputICs[nextOutput] && outputICs[nextOutput].AllowItem(this))
				return outputICs[nextOutput];
		}
		// If that fails, return null
		return null;
	}
}