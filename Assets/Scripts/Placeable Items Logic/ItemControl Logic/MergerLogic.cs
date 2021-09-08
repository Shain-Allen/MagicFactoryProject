/* See Base Class for further documentation for all override functions */
public class MergerLogic : ItemControl
{
	// inputICs[0] is the left-side input, inputICs[1] is the right-side input
	int sideToMoveFrom = 0;

	public override void PlacedAction(GridControl grid_)
	{
		inputICs = new ItemControl[2];
		relativePositions.Add(transform.right);
		outputValidRelPoses.Add(transform.up);
		inputValidRelPoses.Add(-transform.up);
		inputValidRelPoses.Add(transform.right - transform.up);
		base.PlacedAction(grid_);
	}

	public override void setInput(ItemControl newIC)
	{
		if (newIC == null)
			TryAttachInputs();
		else if (newIC.transform.position == transform.position - transform.up)
			inputICs[0] = newIC;
		else if (newIC.transform.position == transform.position - transform.up + transform.right)
			inputICs[1] = newIC;
	}

	public override void MoveItem(ItemControl pullingIC)
	{
		base.MoveItem(pullingIC);

		// Chain reaction backwards, prioritizing the side who's turn it is
		if (inputICs[sideToMoveFrom])
			inputICs[sideToMoveFrom].MoveItem(this);
		sideToMoveFrom = (sideToMoveFrom + 1) % 2;
		if (inputICs[sideToMoveFrom])
			inputICs[sideToMoveFrom].MoveItem(this);
	}
}
