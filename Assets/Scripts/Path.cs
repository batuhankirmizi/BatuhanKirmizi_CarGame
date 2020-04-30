using System.Collections.Generic;
using System.Linq;

// sealed class --> because other classes should not be inherited from this one
// <T> --> generic class
public sealed class Path<T>
{
    // List that holds all the path points
    private LinkedList<T> pathList;
    private LinkedList<Direction> dirList;

    // Whether a path has been succesfully created for a car or not
    private bool isCreated;
    public bool IsCreated { get => isCreated; set => isCreated = value; }

    // Number of points in the path
    private int pointCount;
    public int PointCount { get => pathList.Count; set => pointCount = value; }

    // Loosely coupled with Car object
    private Car car;

    public Path(Car car)
    {
        this.car = car;
        pathList = new LinkedList<T>();
        dirList = new LinkedList<Direction>();
        AddDirection(Direction.NONE);
    }

    /// <summary>
    ///     Adds points to the path
    /// </summary>
    /// <param name="t"> object of type T </param>
    public void CreatePath(T t, Direction d)
    {
        AddPoint(t);
        dirList.AddLast(d);
    }

    public void AddPoint(T t)
    {
        pathList.AddLast(t);
    }

    public void AddDirection(Direction d)
    {
        dirList.AddLast(d);
    }

    internal T GetPoint(int index)
    {
        return pathList.ElementAt(index);
    }

    internal Direction GetDirection(int i)
    {
        return dirList.ElementAt(i);
    }

    internal void Reset()
    {
        pathList.Clear();
        dirList.Clear();
        dirList.AddLast(Direction.NONE);
    }
}

public enum Direction
{
    RIGHT,
    LEFT,
    NONE
}
