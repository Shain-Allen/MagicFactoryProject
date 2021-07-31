using System;
using UnityEngine;

public class HelpFuncs : MonoBehaviour
{
    // Simple Function to ensure X and Y are both within the given bounds
    public static bool insideBorder(Vector2 pos, Vector2 bottomLeft, Vector2 topRight)
    {
        return insideBorder(pos.x, pos.y, bottomLeft.x, topRight.x, bottomLeft.y, topRight.y);
    }
    // Simple Function to ensure X and Y are both within the given bounds
    public static bool insideBorder(float x, float y, float left, float right, float bottom, float top)
    {
        return !(x < left || y < bottom || x > right || y > top);
    }

    // Takes an angle in Degrees and converts it to a 2D Vector3 of length 1, like a unit circle
    public static Vector3 EulerToVector(float eulerAngle)
    {
        eulerAngle *= -1;
        float x = (float)Math.Round(Math.Sin(eulerAngle * Math.PI / 180), 3);
        float y = (float)Math.Round(Math.Cos(eulerAngle * Math.PI / 180), 3);
        return new Vector3(x, y, 0);
    }
    
    // Takes any Vector3 and turns the X/Y into an Euler Angle
    public static float VectorToEuler(Vector3 input)
    {
        float toReturn = (float)(Math.Atan(input.y / input.x) * 180 / Math.PI) + 90;
        if(input.x > 0)
            toReturn += 180;
        return (float)Math.Round(toReturn, 1);
    }

    // Pythagorean Theorems two Vector2s
    public static float GetDistance(Vector2 a, Vector2 b)
    {
        return GetDistance(a.x, b.x, a.y, b.y);
    }
    public static float GetDistance(float x1, float x2, float y1, float y2)
    {
        return (float)Math.Sqrt( Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) );
    }

    // Returns the chunk as a Vector2Int
    public static Vector2Int GetChunk(Vector2 input)
    {
        return GetChunk(input.x, input.y);
    }
    public static Vector2Int GetChunk(float x, float y)
    {
        return new Vector2Int((int)(x / OreGeneration.chunkSize), (int)(y / OreGeneration.chunkSize));
    }
}
