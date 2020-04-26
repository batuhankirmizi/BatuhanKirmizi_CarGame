using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// sealed class --> because other classes should not be inherited from this one
// <T> --> generic class
public sealed class Path<T>
{
    // List that holds all the path points
    private LinkedList<T> pathPoints;

    // Whether a path has been succesfully created for a car or not
    private bool isCreated;
    public bool IsCreated { get => isCreated; set => isCreated = value; }

    // Number of points in the path
    private int pointCount;
    public int PointCount { get => pathPoints.Count; set => pointCount = value; }

    // Loosely coupled with Car object
    private Car car;

    // Indicates the interval of path creation
    private float pathTimer;

    public Path(Car car)
    {
        this.car = car;
        pathPoints = new LinkedList<T>();
        pathTimer = car.speed * .02f;
    }

    private float pathTimePassed = 0f;
    /// <summary>
    ///     Adds points to the path
    /// </summary>
    /// <param name="t"> object of type T </param>
    public void CreatePath(T t)
    {
        pathTimePassed += Time.deltaTime;
        if (pathTimePassed >= pathTimer) {
            AddPoint(t);

            pathTimePassed = 0f;
        }
    }

    public void AddPoint(T t)
    {
        pathPoints.AddLast(t);
    }

    internal T GetPoint(int index)
    {
        return pathPoints.ElementAt(index);
    }

    internal void Reset()
    {
        pathPoints.Clear();
    }
}
