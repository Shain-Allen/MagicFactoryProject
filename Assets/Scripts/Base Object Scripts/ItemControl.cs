using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;

/* An IC currently is an item in the world that has precisely 1 slot for an item in it
 * These slots typically have a frontBelt and/or a backBelt, signalled by allow Front/Back Belt
 * UpdateSprite is necessary for Belts to change their sprits when the world changes
 * TryAttach Front/Back Belt will try to attach that side, otherwise it will set that belt reference to null
 * MoveItem is necessary so belts can chain together with other ICs/Belts so movement can be synced up
 */
public abstract class ItemControl : Placeable
{
	// frontBelt will be the output belt for this IC
	public ItemControl frontBelt = null;
	// backBelt will be the input belt for this IC
	public ItemControl backBelt = null;
	public GameObject itemSlot = null;
	public GridControl grid;

	public bool allowBackBelt = true;
	public bool allowFrontBelt = true;

	/* PlacedAction initializes the variables inside this IC, and attaches to nearby ICs
	 * No special Preconditions
	 * POSTCONDITIONS: The IC will attach to nearby ICs if possible as per the rules
	 * cont.: Only ICs directly next to this one might be altered in any way
	 */
	// public abstract void PlacedAction();

	/* RemovedAction removes this IC, then has its front and back belt Re-TryAttachBelts
	 * No special Preconditions
	 * POSTCONDITIONS: The item in this IC is completely destroyed
	 * cont.: This IC will be removed from the dictionary and the world
	 * cont.: This belt's front and back belts will try to find their new attachments, which might cause issues potentially
	 */
	// public abstract void RemovedAction();

	/* Pairs this IC's frontBelt with the backBelt of the IC in front of it, if possible
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * cont.: Direction should be the direction this IC is facing
	 * POSTCONDITIONS: Only the IC directly in front of this one can be altered
	 */
	public virtual void TryAttachFrontBelt() { }

	/* Attaches the currently placing belt the best it can
	 * BackBelt is prioritized to be directly behind it, then its left side, then its right
	 * PRECONDITIONS: Direction must be precisely equal to Vector3.UP, .LEFT, .RIGHT, or .DOWN
	 * POSTCONDITIONS: Only ICs pointing to this one might be altered
	 */
	public virtual void TryAttachBackBelt() { }

	/* This allows ICs to change sprites based on their connections, mostly affects BeltLogic */
	public virtual void UpdateSprite() { }
	/* Moves the Item in an IC forward one if it can, then calls its BackBelt behind it to do the same
	 * PRECONDITIONS: There cannot be a loop
	 * cont.: The frontBelt of this one has already done MoveItem, or is null
	 * POSTCONDITIONS: Each item will only move forward up to 1 belt per cycle
	 * cont.: An item will never move if its frontBelt already has an item
	 */
	public abstract void MoveItem();
}

