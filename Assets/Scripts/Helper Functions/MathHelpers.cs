using System;
using UnityEngine;

public class MathHelpers : MonoBehaviour
{
	// Calls Math.Round, but turns it into a float instead of it being a double 
	private static float Round(float input, int degree)
	{
		return (float)Math.Round(input, degree);
	}

	// Returns true if the X,Y position is within the given bounds
	public static bool insideBorder(Vector2 pos, Vector2 bottomLeft, Vector2 topRight)
	{
		return insideBorder(pos.x, pos.y, bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
	}
	public static bool insideBorder(float x, float y, float left, float right, float bottom, float top)
	{
		return !(x < left || y < bottom || x > right || y > top);
	}

	// Takes a Euler Angle and converts it to a 2D Vector3 of length 1, like a unit circle
	public static Vector3 EulerToVector(float eulerAngle)
	{
		eulerAngle *= -1;
		float x = Round(Mathf.Sin(eulerAngle * Mathf.PI / 180), 3);
		float y = Round(Mathf.Cos(eulerAngle * Mathf.PI / 180), 3);
		return new Vector3(x, y, 0);
	}
	// Returns the Euler Angle of any Vector3 (ignores the Z axis)
	public static float VectorToEuler(Vector3 input)
	{
		float toReturn = (-Mathf.Atan(input.y / input.x) * 180 / Mathf.PI) + 90;
		if (input.x > 0)
			toReturn += 180;
		toReturn = Mathf.Abs(toReturn);
		return Round(toReturn, 1);
	}

	// Returns the distance between 2 points using the pythagorean theorem
	public static float GetDistance(Vector2 a, Vector2 b)
	{
		return GetDistance(a.x, b.x, a.y, b.y);
	}
	public static float GetDistance(float x1, float x2, float y1, float y2)
	{
		return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
	}
}
