using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCPath : MonoBehaviour
{
    [SerializeField] Transform[] quadraticBezierPath;
    [SerializeField] float resolution = 10;

    private void OnDrawGizmos()
    {
        if (!IsCountOdd())
        {
            print("Info: NPCPath should have a odd number of points to work");
            return;
        }

        for (int i = 0; i + 2 < quadraticBezierPath.Length; i += 2)
        {
            var startP = quadraticBezierPath[i].position;
            var mildBezierP = quadraticBezierPath[i + 1].position;
            var endP = quadraticBezierPath[i + 2].position;

            var lastLine = startP;
            Vector3 pr = Vector3.zero;
            for (float t = 0; t <= 1; t += 1f / resolution)
            {
                pr = ComputeBezierPoint(startP, mildBezierP, endP, t);
                Debug.DrawLine(lastLine, pr, Color.green);
                lastLine = pr;
            }

            pr = ComputeBezierPoint(startP, mildBezierP, endP, 1);
            Debug.DrawLine(lastLine, pr, Color.green);
        }
    }

    private static Vector3 ComputeBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        var pa = Vector3.Lerp(p1, p2, t);
        var pb = Vector3.Lerp(p2, p3, t);
        var pr = Vector3.Lerp(pa, pb, t);
        return pr;
    }

    private bool IsCountOdd()
    {
        return (quadraticBezierPath.Length - 1) % 2 == 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
