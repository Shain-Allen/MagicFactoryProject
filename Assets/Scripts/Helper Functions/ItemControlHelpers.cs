using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HelpFuncs;

public class ItemControlHelpers
{
	public static void AddToWorld(GridControl grid, ItemControl IC)
	{
		GameObject chunkPlaceholder;
		if (grid.worldChunks.TryGetValue(PosToChunk(IC.transform.position), out chunkPlaceholder))
		{
			chunkPlaceholder.GetComponent<Chunk>().placeObjects[PosToPosInChunk(IC.transform.position).x, PosToPosInChunk(IC.transform.position).y] = IC.gameObject;
			grid.placeObjects.Add(IC.transform.position, IC.gameObject);
		}
	}

	public static void TryAttachFrontBeltHelper(GridControl grid, ItemControl IC)
	{
		Vector3 direction = EulerToVector(IC.transform.rotation.eulerAngles.z);
		IC.frontBelt = null;
		ItemControl ICFront = getICAt(grid, IC.transform.position + direction);

		// Make sure the IC in front exists, allows this to attach, and isn't occupied
		if (ICFront != null && ICFront.allowBackBelt && ICFront.backBelt == null)
		{
			// Prevent a situation where they point right at each other |>>| |<<|
			if (!ICFront.allowFrontBelt || ICFront.transform.rotation.eulerAngles.z != (IC.transform.rotation.eulerAngles.z + 180) % 360)
			{
				IC.frontBelt = ICFront;
				IC.frontBelt.backBelt = IC;
				IC.frontBelt.UpdateSprite();
			}
		}
	}

	public static void TryAttachBackBeltHelper(GridControl grid, ItemControl IC, int relativeAngle)
	{
		int connectionAngle = (int)(IC.transform.rotation.eulerAngles.z + relativeAngle) % 360;
		Vector3 deltaPos = EulerToVector(connectionAngle);
		ItemControl ICSide = getICAt(grid, IC.transform.position + deltaPos);

		// If IC on the given side exists, allows this to attach, and isn't occupied
		if (ICSide != null && ICSide.allowFrontBelt && ICSide.frontBelt == null)
		{
			// And if it is pointing to this IC
			if (ICSide.transform.rotation.eulerAngles.z == connectionAngle)
			{
				IC.backBelt = ICSide;
				IC.backBelt.frontBelt = IC;
				IC.backBelt.UpdateSprite();
			}
		}
	}
}
