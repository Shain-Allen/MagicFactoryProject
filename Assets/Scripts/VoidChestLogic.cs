using System;
using System.Collections.Generic;
using UnityEngine;

public class VoidChestLogic : InvSlot
{
    GridControl grid;

    public override void PlacedAction(GridControl grid_)
    {
        grid = grid_;
        backBelt = null;
        allowBackBelt = true;
        frontBelt = null;
        allowFrontBelt = false;

        grid.OnBeltTimerCycle += BeltCycle;
    }

    public void BeltCycle(object sender, EventArgs e)
	{
	    MoveItem();
	}

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

    public override void TryAttachBackBelt(Vector3 direction)
    {
        GameObject temp = null;

		// Attaches backBelt to a belt directly behind this one if possible
		backBelt = null;
		InvSlot tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position - direction), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && temp.transform.rotation.eulerAngles.z == transform.rotation.eulerAngles.z)
			{
				if(tempInvSlot.allowFrontBelt && tempInvSlot.frontBelt == null)
				{
					backBelt = tempInvSlot;
					backBelt.frontBelt = this;
					backBelt.UpdateSprite();
				}
			}
		}
    }

	public override void RemovedAction()
    {
        grid.placeObjects.Remove(transform.position);
        
        if(backBelt != null)
        {
            backBelt.frontBelt = null;
            backBelt.UpdateSprite();
        }
        backBelt = null;

		Destroy(gameObject);
    }

    public override void UpdateSprite(){}
    public override void TryAttachFrontBelt(Vector3 direction){}
}
