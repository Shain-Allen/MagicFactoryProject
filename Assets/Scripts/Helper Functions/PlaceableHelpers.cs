using System;
using System.Collections;
using UnityEngine;
using static HelpFuncs;

public class PlaceableHelpers
{
	// Returns the Placeable of type T at the provided location, or null if there isn't anything there
	public static T GetPlaceableAt<T>(GridControl grid, Vector2 pos) where T : Placeable
	{
		GameObject objAtPos, chunkParent;
		T placeableAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk(pos), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<T>(out placeableAtPos)))
			{
				return placeableAtPos;
			}
		}
		return default(T);
	}

	// Places the given Placeable into the world and the chunk array. PosOffSet alters the position of the placeable, used for large placeables
	public static void AddToWorld(GridControl grid, Placeable placeable)
	{
		AddToWorld(grid, placeable, Vector3.zero);
	}
	public static void AddToWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(placeable.transform.position + posOffSet).x, PosToPosInChunk(placeable.transform.position + posOffSet).y] = placeable.gameObject;
	}

	// Remove the Placeable from any references to it. PosOffSet alters the position of the placeable, used for large placeables
	public static void RemoveFromWorld(GridControl grid, Placeable placeable)
	{
		RemoveFromWorld(grid, placeable, Vector3.zero);
	}
	public static void RemoveFromWorld(GridControl grid, Placeable placeable, Vector3 posOffSet)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, placeable.transform.position + posOffSet);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(placeable.transform.position + posOffSet).x, PosToPosInChunk(placeable.transform.position + posOffSet).y] = null;
	}

	/* The Below Function Belongs in its own ResourceHelpers.cs */

	// Returns the Ore or Gas at the provided location, or null if there isn't an anything there
	public static T GetResourceAt<T>(GridControl grid, Vector2 pos) where T : BaseResource
	{
		GameObject objAtPos, chunkParent;
		T oreAtPos;

		if (grid.worldChunks.TryGetValue(GetChunk(pos), out chunkParent))
		{
			objAtPos = chunkParent.GetComponent<Chunk>().oreObjects[PosToPosInChunk(pos).x, PosToPosInChunk(pos).y];
			if (objAtPos != null && (objAtPos.TryGetComponent<T>(out oreAtPos)))
			{
				return oreAtPos;
			}
		}
		return default(T);
	}
	public static void RemoveResourceFromWorld(GridControl grid, BaseResource resource)
	{
		GameObject chunkParent = GetChunkParentByPos(grid, resource.transform.position);
		if (chunkParent)
			chunkParent.GetComponent<Chunk>().placeObjects[PosToPosInChunk(resource.transform.position).x, PosToPosInChunk(resource.transform.position).y] = null;
	}

	/* Everything Below Belongs in an ItemControlHelpers.cs */

	// Attaches the front belt if possible, copy documentation from ItemControl.cs
	public static void TryAttachFrontBeltHelper(GridControl grid, ItemControl IC)
	{
		Vector3 direction = EulerToVector(IC.transform.rotation.eulerAngles.z);
		ItemControl ICFront = GetPlaceableAt<ItemControl>(grid, IC.transform.position + direction);
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
		ItemControl ICSide = GetPlaceableAt<ItemControl>(grid, IC.transform.position + deltaPos);
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

	// Returns the relative angle of MainIC's facing direction to the angle of AskingIC
	public static int getRelativeAngle(ItemControl CenterIC, ItemControl deltaIC)
	{
		Vector3 deltaPos = deltaIC.transform.position - CenterIC.transform.position;
		float absoluteAngle = VectorToEuler(deltaPos);
		float relativeAngle = (CenterIC.transform.rotation.eulerAngles.z - absoluteAngle + 360) % 360;
		return (int)Mathf.Round(relativeAngle);
	}

	// Smoothly moves the item from slot one to slot two
	public static IEnumerator SmoothMove(GridControl grid, GameObject Item, Vector3 startingPOS, Vector3 EndingPOS)
	{
		float timeElapsed = 0;

		while (timeElapsed < grid.beltCycleTime)
		{
			if (!Item)
				yield break;

			Item.transform.position = Vector3.Lerp(startingPOS, EndingPOS, timeElapsed / grid.beltCycleTime);
			timeElapsed += Time.deltaTime;

			yield return null;
		}

		if (Item)
			Item.transform.position = EndingPOS;
	}
}