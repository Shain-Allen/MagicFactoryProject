using System;
using System.Collections.Generic;
using UnityEngine;

public class FullChestLogic : InvSlot
{
    GridControl grid;
    public GameObject itemToClone;

    public override void PlacedAction(GridControl grid_)
    {
        grid = grid_;
        frontBelt = null;
        allowFrontBelt = true;
        backBelt = null;
        allowBackBelt = false;

        TryAttachFrontBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));
    }

    public override void MoveItem()
	{
		if (frontBelt && !frontBelt.itemSlot)
		{
            frontBelt.itemSlot = Instantiate(itemToClone, frontBelt.transform.position, Quaternion.identity, grid.transform);
		}
	}

    public override void TryAttachFrontBelt(Vector3 direction)
    {
        GameObject temp = null;

		// Attaches frontBelt a belt directly in front of this one if possible
		// Currently would override that belt's previous backBelt
		frontBelt = null;
		InvSlot tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position + direction), out temp))
		{
			tempInvSlot = temp.GetComponent<InvSlot>();
			if (tempInvSlot != null && (!tempInvSlot.allowFrontBelt || temp.transform.rotation.eulerAngles.z != (transform.rotation.eulerAngles.z + 180) % 360))
			{
				if(tempInvSlot.allowBackBelt && tempInvSlot.backBelt == null)
				{
					frontBelt = tempInvSlot;
					frontBelt.backBelt = this;
					frontBelt.UpdateSprite();
				}
			}
		}
    }

	public override void RemovedAction()
    {
        grid.placeObjects.Remove(transform.position);
        
        if(frontBelt != null)
        {
            frontBelt.backBelt = null;
            frontBelt.TryAttachBackBelt(HelpFuncs.EulerToVector(frontBelt.transform.rotation.eulerAngles.z));
            frontBelt.UpdateSprite();
        }
        frontBelt = null;

		Destroy(gameObject);
    }
    
    public override void UpdateSprite(){}
    public override void TryAttachBackBelt(Vector3 direction){}
}
