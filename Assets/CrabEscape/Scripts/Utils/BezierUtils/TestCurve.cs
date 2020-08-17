using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurve : MonoBehaviour
{
    public LineRenderer line;

    public int numPoints = 60;

    List<Vector3> linePoints = new List<Vector3>();

    public Transform startPoint, finalPoint, middlePoint;

    [Header("Min-Max Height Param")]
    [Space(10)]
    public float y;

    public bool isCurvest;


    private void Start()
    {
        line.positionCount = numPoints;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetMiddlePosition(-y,y);
        }

        linePoints.Clear();

        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            linePoints.Add( BezierUtils.GetQuadraticCurve(startPoint.position,
                                                             middlePoint.position,
                                                             finalPoint.position,t));
                           
        }

        line.SetPositions(linePoints.ToArray());
    }

    private void SetMiddlePosition(float yMax,float yMin)
    {
        // GET CENTER
        middlePoint.position = BezierUtils.CenterOfVectors(new Vector3[]
                                                           {
                                                                   startPoint.position,
                                                                   finalPoint.position
                                                           });

        // Change the middle point position with preimposted distances
        float xCurve = Random.Range(startPoint.position.x, finalPoint.position.x);

        float xCurvest = Random.Range(0, 100) > 50 ? middlePoint.position.x + xCurve : middlePoint.position.x - xCurve;

        var xPos = isCurvest ? xCurvest : xCurve;
        var yPos = middlePoint.position.y + Random.Range(yMin, yMax);

        middlePoint.position = new Vector3(xPos, yPos, middlePoint.position.z);
    }

    private Vector3[] GetCurvePoint(int numPoints)
    {
        List<Vector3> points = new List<Vector3>();
        
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            points.Add(BezierUtils.GetQuadraticCurve(startPoint.position,
                                                     middlePoint.position,
                                                     finalPoint.position, t));

        }

        return points.ToArray();
    }
}
