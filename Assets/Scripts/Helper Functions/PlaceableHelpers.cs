using System;
using System.Collections;
using UnityEngine;
using static HelpFuncs;

public class PlaceableHelpers
{
	// Returns the Placeable at the provided location, or null if there isn't a Placeable there
	public static Placeable GetPlaceableAt(GridControl grid, Vector2 pos)
	{
		GameObject objAtPos, chunkParent;
		Placeable PlaceableAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk(pos), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<Placeable>(out PlaceableAtPos)))
				return PlaceableAtPos;
		}
		return null;
	}

	// Places the given IC into the world and the chunk array
	public static void AddToWorld(GridControl grid, Placeable Placeable)
	{
		GameObject chunkPlaceholder;
		if (grid.worldChunks.TryGetValue(GetChunk(Placeable.transform.position), out chunkPlaceholder))
			chunkPlaceholder.GetComponent<Chunk>().placeObjects[PosToPosInChunk(Placeable.transform.position).x, PosToPosInChunk(Placeable.transform.position).y] = Placeable.gameObject;
	}

	// Remove the IC from any references to it
	public static void RemoveFromWorld(GridControl grid, Placeable Placeable)
	{
		GameObject chunkPlaceholder;
		if (grid.worldChunks.TryGetValue(GetChunk(Placeable.transform.position), out chunkPlaceholder))
			chunkPlaceholder.GetComponent<Chunk>().placeObjects[PosToPosInChunk(Placeable.transform.position).x, PosToPosInChunk(Placeable.transform.position).y] = null;
	}

	// Returns the IC at the provided location, or null if there isn't an IC there
	public static ItemControl GetICAt(GridControl grid, Vector2 pos)
	{
		GameObject objAtPos, chunkParent;
		ItemControl itemControlAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk(pos), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<ItemControl>(out itemControlAtPos)))
			{
				return itemControlAtPos;
			}
		}
		return null;
	}

	// Attaches the front belt if possible, copy documentation from ItemControl.cs
	public static void TryAttachFrontBeltHelper(GridControl grid, ItemControl IC)
	{
		Vector3 direction = EulerToVector(IC.transform.rotation.eulerAngles.z);
		ItemControl ICFront = GetICAt(grid, IC.transform.position + direction);
		IC.setFrontBelt(null);

		// Make sure the IC in front exists, allows this to attach, and isn't occupied
		if (ICFront != null && ICFront.getAllowBackBelt() && ICFront.getBackBelt() == null)
		{
			// Prevent a situation where they point right at each other |>>| |<<|
			if (!ICFront.getAllowFrontBelt() || ICFront.transform.rotation.eulerAngles.z != (IC.transform.rotation.eulerAngles.z + 180) % 360)
			{
				IC.setFrontBelt(ICFront);
				IC.getFrontBelt().setBackBelt(IC);
				IC.getFrontBelt().UpdateSprite();
			}
		}
	}

	// Attaches the back belt if possible from the relative angle, copy documentation from ItemControl.cs
	public static void TryAttachBackBeltHelper(GridControl grid, ItemControl IC, int relativeAngle)
	{
		int connectionAngle = (int)(IC.transform.rotation.eulerAngles.z + relativeAngle) % 360;
		Vector3 deltaPos = EulerToVector(connectionAngle);
		ItemControl ICSide = GetICAt(grid, IC.transform.position + deltaPos);
		IC.setBackBelt(null);

		// If IC on the given side exists, allows this to attach, and isn't occupied
		if (ICSide != null && ICSide.getAllowFrontBelt() && ICSide.getFrontBelt() == null)
		{
			// And if it is pointing to this IC
			if ((ICSide.transform.rotation.eulerAngles.z + 180) % 360 == connectionAngle)
			{
				IC.setBackBelt(ICSide);
				IC.getBackBelt().setFrontBelt(IC);
				IC.getBackBelt().UpdateSprite();
			}
		}
	}

	// Smoothly moves the item from slot one to slot two
	public static IEnumerator SmoothMove(GridControl grid, GameObject Item, Vector3 startingPOS, Vector3 EndingPOS)
	{
		float timeElapsed = 0;

		while (timeElapsed < grid.beltCycleTime)
		{
			if (!Item)
				continue;

			Item.transform.position = Vector3.Lerp(startingPOS, EndingPOS, timeElapsed / grid.beltCycleTime);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		if (Item)
			Item.transform.position = EndingPOS;
	}
}