using System;
using System.Collections.Generic;
using UnityEngine;

/* For all overriding methods, check ItemControl.cs for further documentation*/
public class BeltLogic : ItemControl
{
	public Sprite straightBelt;
	public Sprite cornerBelt;
	SpriteRenderer spriteRenderer;

	public override void PlacedAction(GridControl grid_)
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		inputValidRelPoses.Add(-transform.up);
		inputValidRelPoses.Add(-transform.right);
		inputValidRelPoses.Add(transform.right);
		outputValidRelPoses.Add(transform.up);
		base.PlacedAction(grid_);
	}

	public override void TryAttachInputs()
	{
		base.TryAttachInputs();
		UpdateSprite();
	}
	public override void MoveItem(ItemControl pullingIC)
	{
		MoveItemHelper(pullingIC, true);
	}

	public void UpdateSprite()
	{
		// Belts should default to straight when it has no inputIC or inputIC is right behind
		spriteRenderer.sprite = straightBelt;
		spriteRenderer.flipX = false;
		if (inputICs[0] == null)
			return;

		// This gets the closest position of inputIC to this belt, therefore being the connection probably. Notably Jenk
		List<Vector3> backPositions = inputICs[0].getAllPositions();
		Vector3 backConnectionPos = backPositions[0];
		foreach (Vector3 pos in inputICs[0].getAllPositions())
			if (Vector3.Distance(backConnectionPos, this.transform.position) > Vector3.Distance(pos, this.transform.position))
				backConnectionPos = pos;

		if (transform.position - transform.up == backConnectionPos)
			return;
		// Otherwise, it must be a corner belt; if it's turning right, it needs to be flipped
		spriteRenderer.sprite = cornerBelt;
		spriteRenderer.flipX = (transform.position + transform.right == backConnectionPos);
	}
}