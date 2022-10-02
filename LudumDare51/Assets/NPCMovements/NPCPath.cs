using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCPath : MonoBehaviour
{
    public event Action OnPathChanged;

    [SerializeField] Transform[] quadraticBezierPath;
    [SerializeField] float resolution = 10;

    private void OnDrawGizmos()
    {
        GetChildren();
        var path = GeneratePath();
        Vector3 lastPoint = Vector3.one;
        foreach (var point in path)
        {
            if (lastPoint == Vector3.one)
            {
                lastPoint = point;
                continue;
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(lastPoint, point);
                lastPoint = point;
            }
        }
    }

    private void Start()
    {
        GetChildren();
    }

    private void GetChildren()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }
        quadraticBezierPath = children.ToArray();
    }

    private void Update()
    {
        bool pathChanged = false;
        foreach (var pathTransform in quadraticBezierPath)
        {
            if (pathTransform.hasChanged)
            {
                pathChanged = true;
                pathTransform.hasChanged = false;
            }
        }

        if (pathChanged)
        {
            OnPathChanged?.Invoke();
        }
    }

    public Vector3[] GeneratePath()
    {
        if (!IsCountOdd())
        {
            print("Info: NPCPath should have a odd number of points to work");
            return new Vector3[0];
        }

        if (resolution < 0)
        {
            print("Info: NPCPath can't have a negative resolution");
            return new Vector3[0];
        }

        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i + 2 < quadraticBezierPath.Length; i += 2)
        {
            var startP = quadraticBezierPath[i].position;
            var mildBezierP = quadraticBezierPath[i + 1].position;
            var endP = quadraticBezierPath[i + 2].position;

            var lastLine = startP;
            path.Add(startP);
            Vector3 pr = Vector3.zero;
            for (float t = 0; t <= 1; t += 1f / resolution)
            {
                pr = ComputeBezierPoint(startP, mildBezierP, endP, t);
                path.Add(pr);
                lastLine = pr;
            }

            path.Add(endP);
        }
        return path.ToArray();
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
}
