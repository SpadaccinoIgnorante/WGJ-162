using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierUtils
{
    public static Vector3 GetLinearCurve(Vector3 p0, Vector3 p1,float t)
    {
        return p0 + t * (p1-p0);
    }

    /// <summary>
    /// Get a bezier curve with 3 points
    /// </summary>
    /// <param name="p0">Is the initial point</param>
    /// <param name="p1">Is the middle point, which infuence the curve</param>
    /// <param name="p2">Is the final point</param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 GetQuadraticCurve(Vector3 p0,Vector3 p1,Vector3 p2,float t)
    {
        float unit = 1 - t;
        float uSquare = unit * unit;
        float tSquare = t * t;

        return (uSquare * p0) + 2 * (unit * t * p1) + (tSquare * p2);
    }

    public static Vector3 CenterOfVectors(Vector3[] vectors)
    {
        Vector3 sum = Vector3.zero;
        
        if (vectors == null || vectors.Length == 0)
            return sum;
        
        foreach (Vector3 vec in vectors)
            sum += vec;
        
        return sum / vectors.Length;
    }

    public static Vector3[] CreateCurve(int numPoints,Vector3 p0,Vector3 p1,Vector3 p2)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            points.Add(BezierUtils.GetQuadraticCurve(p0, p1, p2, t));
        }

        return points.ToArray();
    }
}
