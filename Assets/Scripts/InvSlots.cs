using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InvSlot : Placeable
{
    public InvSlot frontBelt;
    public InvSlot backBelt;
    public GameObject itemSlot;

    public bool allowBackBelt = true;
    public bool allowFrontBelt = true;

    public abstract void UpdateSprite();
    public abstract void TryAttachFrontBelt(Vector3 direction);
    public abstract void TryAttachBackBelt(Vector3 direction);
	public abstract void MoveItem();
}

