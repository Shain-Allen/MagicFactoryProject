using System.Collections;
using UnityEngine;
using static MathHelpers;
using static PlaceableHelpers;

public class ICHelpers : MonoBehaviour
{
	// Attaches the output IC if possible, copy documentation from ItemControl.cs
	public static void TryAttachOutputHelper(GridControl grid, ItemControl IC)
	{
		TryAttachOutputHelper(grid, IC, 0);
	}
	public static void TryAttachOutputHelper(GridControl grid, ItemControl IC, int relativeAngle)
	{
		Vector3 dirVector = EulerToVector(relativeAngle + IC.transform.rotation.eulerAngles.z);
		ItemControl askingIC = GetPlaceableAt<ItemControl>(grid, dirVector + IC.transform.position);
		if (askingIC)
			TryAttachOutputHelper(grid, IC, askingIC);
	}
	public static void TryAttachOutputHelper(GridControl grid, ItemControl IC, ItemControl askingIC)
	{
		if (!IC.AllowOutputTo(askingIC) || !askingIC.AllowInputFrom(IC))
			return;

		IC.setOutput(askingIC);
		askingIC.setInput(IC);
		IC.UpdateSprite();
		askingIC.UpdateSprite();
	}

	// Attaches the input IC if possible from the relative angle, copy documentation from ItemControl.cs
	public static void TryAttachInputHelper(GridControl grid, ItemControl IC)
	{
		TryAttachInputHelper(grid, IC, 180);
	}
	public static void TryAttachInputHelper(GridControl grid, ItemControl IC, int relativeAngle)
	{
		Vector3 dirVector = EulerToVector(relativeAngle + IC.transform.rotation.eulerAngles.z);
		ItemControl askingIC = GetPlaceableAt<ItemControl>(grid, dirVector + IC.transform.position);
		if (askingIC)
			TryAttachInputHelper(grid, IC, askingIC);
	}
	public static void TryAttachInputHelper(GridControl grid, ItemControl IC, ItemControl askingIC)
	{
		if (!IC.AllowInputFrom(askingIC) || !askingIC.AllowOutputTo(IC))
			return;

		IC.setInput(askingIC);
		askingIC.setOutput(IC);
		IC.UpdateSprite();
		askingIC.UpdateSprite();
	}

	// Returns the relative angle of MainIC's facing direction to the angle of AskingIC
	public static int getRelativeAngle(ItemControl IC, ItemControl askingIC)
	{
		return getRelativeAngle(IC, askingIC.transform.position);
	}
	public static int getRelativeAngle(ItemControl IC, Vector3 askingICPos)
	{
		Vector3 deltaPos = askingICPos - IC.transform.position;
		float absoluteAngle = VectorToEuler(deltaPos);
		float relativeAngle = (IC.transform.rotation.eulerAngles.z - absoluteAngle + 360) % 360;
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
