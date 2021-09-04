using System;
using UnityEngine;

public class MathHelpers : MonoBehaviour
{
	// Returns true if the X,Y position is within the given bounds
	public static bool insideBorder(Vector2 pos, Vector2 bottomLeft, Vector2 topRight)
	{
		return insideBorder(pos.x, pos.y, bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
	}
	public static bool insideBorder(float x, float y, float left, float right, float bottom, float top)
	{
		return !(x < left || y < bottom || x > right || y > top);
	}
}
