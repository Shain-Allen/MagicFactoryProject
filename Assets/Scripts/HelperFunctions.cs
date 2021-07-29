using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelperFunctions : MonoBehaviour
{
    // Simple Function to ensure X and Y are both within the given bounds
    public static bool insideBorder(int x, int y, int left, int right, int bottom, int top)
    {
        return !(x < left || y < bottom || x > right || y > top);
    }

    // Takes an angle in Degrees and converts it to a 2D Vector3, like a unit circle
    public static Vector3 EulerToUnitCircle(float eulerAngle)
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
}
