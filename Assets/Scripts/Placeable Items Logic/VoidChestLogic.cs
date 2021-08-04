using System;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;

public class VoidChestLogic : ItemControl
{
	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void PlacedAction(GridControl grid_)
	{
		grid = grid_;
		backBelt = null;
		allowBackBelt = true;
		frontBelt = null;
		allowFrontBelt = false;

		TryAttachBackBelt(EulerToVector(transform.rotation.eulerAngles.z));

		grid.OnBeltTimerCycle += BeltCycle;
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public void BeltCycle(object sender, EventArgs e)
	{
		MoveItem();
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void MoveItem()
	{
		if (backBelt)
		{
			if (backBelt.itemSlot)
			{
				Destroy(backBelt.itemSlot);
				backBelt.itemSlot = null;
			}
			backBelt.MoveItem();
		}
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void TryAttachBackBelt(Vector3 direction)
	{
		GameObject temp = null;

		// Attaches backBelt to a belt directly behind this one if possible
		backBelt = null;
		ItemControl tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position - direction), out temp))
		{
			tempInvSlot = temp.GetComponent<ItemControl>();
			if (tempInvSlot != null && temp.transform.rotation.eulerAngles.z == transform.rotation.eulerAngles.z)
			{
				if (tempInvSlot.allowFrontBelt && tempInvSlot.frontBelt == null)
				{
					backBelt = tempInvSlot;
					backBelt.frontBelt = this;
					backBelt.UpdateSprite();
				}
			}
		}
	}

	/* [Copy Documentation from Parent Class InvSlot.cs] */
	public override void RemovedAction()
	{
		grid.placeObjects.Remove(transform.position);

		if (backBelt != null)
		{
			backBelt.frontBelt = null;
			backBelt.UpdateSprite();
		}
		backBelt = null;

		Destroy(gameObject);
	}

	/* The remaining methods are left empty on purpose, as they are unncessary for this InvSlot */
	public override void UpdateSprite() { }
	public override void TryAttachFrontBelt(Vector3 direction) { }
}
