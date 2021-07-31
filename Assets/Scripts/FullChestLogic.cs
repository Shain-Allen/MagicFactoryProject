using System;
using System.Collections.Generic;
using UnityEngine;

public class FullChestLogic : ItemControl
{
    public GameObject itemToClone;

    /* [Copy Documentation from Parent Class InvSlot.cs] */
    public override void PlacedAction(GridControl grid_)
    {
        grid = grid_;
        frontBelt = null;
        allowFrontBelt = true;
        backBelt = null;
        allowBackBelt = false;

        TryAttachFrontBelt(HelpFuncs.EulerToVector(transform.rotation.eulerAngles.z));
    }

    /* [Copy Documentation from Parent Class InvSlot.cs] */
    public override void MoveItem()
	{
		if (frontBelt && !frontBelt.itemSlot)
		{
            frontBelt.itemSlot = Instantiate(itemToClone, frontBelt.transform.position, Quaternion.identity, grid.transform);
		}
	}

    /* [Copy Documentation from Parent Class InvSlot.cs] */
    public override void TryAttachFrontBelt(Vector3 direction)
    {
        GameObject temp = null;

		// Attaches frontBelt a belt directly in front of this one if possible
		// Currently would override that belt's previous backBelt
		frontBelt = null;
		ItemControl tempInvSlot;
		if (grid.placeObjects.TryGetValue((transform.position + direction), out temp))
		{
			tempInvSlot = temp.GetComponent<ItemControl>();
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

    /* [Copy Documentation from Parent Class InvSlot.cs] */
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
    
    /* The remaining methods are left empty on purpose, as they are unncessary for this InvSlot */
    public override void UpdateSprite(){}
    public override void TryAttachBackBelt(Vector3 direction){}
}
