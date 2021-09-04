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

	// Returns the Euler Angle of any Vector3 (ignores the Z axis)
	public static float VectorToEuler(Vector3 input)
	{
		float toReturn = (-Mathf.Atan(input.y / input.x) * 180 / Mathf.PI) + 90;
		if (input.x > 0)
			toReturn += 180;
		toReturn = Mathf.Abs(toReturn);
		return (float)Math.Round(toReturn, 1);
	}
}
