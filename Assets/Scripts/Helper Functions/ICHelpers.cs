using System.Collections;
using UnityEngine;
using static PlaceableHelpers;

public class ICHelpers : MonoBehaviour
{
	// Attaches the output IC if possible, copy documentation from ItemControl.cs
	public static void TryAttachOutputHelper(GridControl grid, ItemControl IC, Vector3 relativePos)
	{
		ItemControl outputIC = GetPlaceableAt<ItemControl>(grid, IC.transform.position + relativePos);
		if (!outputIC || !IC.AllowOutputTo(outputIC) || !outputIC.AllowInputFrom(IC))
			return;

		IC.setOutput(outputIC);
		outputIC.setInput(IC);
	}

	// Attaches the input IC if possible from the relative angle, copy documentation from ItemControl.cs
	public static void TryAttachInputHelper(GridControl grid, ItemControl IC, Vector3 relativePos)
	{
		ItemControl inputIC = GetPlaceableAt<ItemControl>(grid, IC.transform.position + relativePos);
		if (!inputIC || !IC.AllowInputFrom(inputIC) || !inputIC.AllowOutputTo(IC))
			return;

		IC.setInput(inputIC);
		inputIC.setOutput(IC);
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
