using System.Collections;
using UnityEngine;
using static PlaceableHelpers;

public class ICHelpers : MonoBehaviour
{
	// Pairs IC's outputIC to the inputIC of the IC at the relative position, if legal
	public static void TryAttachOutputHelper(GridControl grid, ItemControl IC, Vector3 relativePos)
	{
		ItemControl outputIC = GetPlaceableAt<ItemControl>(grid, IC.transform.position + relativePos);
		if (!outputIC || !IC.AllowOutputTo(outputIC) || !outputIC.AllowInputFrom(IC))
			return;

		IC.setOutput(outputIC);
		outputIC.setInput(IC);
	}

	// Pairs IC's inputIC to the outputIC of the IC at the relative position, if legal
	public static void TryAttachInputHelper(GridControl grid, ItemControl IC, Vector3 relativePos)
	{
		ItemControl inputIC = GetPlaceableAt<ItemControl>(grid, IC.transform.position + relativePos);
		if (!inputIC || !IC.AllowInputFrom(inputIC) || !inputIC.AllowOutputTo(IC))
			return;

		IC.setInput(inputIC);
		inputIC.setOutput(IC);
	}

	// Smoothly moves item from startPos to endPos over the time of 1 beltCycle
	public static IEnumerator SmoothMove(GridControl grid, GameObject item, Vector3 startPos, Vector3 endPos)
	{
		float timeElapsed = 0;
		while (timeElapsed < grid.beltCycleTime)
		{
			if (!item) yield break;

			item.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / grid.beltCycleTime);
			timeElapsed += Time.deltaTime;

			yield return null;
		}
		if (item) item.transform.position = endPos;
	}
}
