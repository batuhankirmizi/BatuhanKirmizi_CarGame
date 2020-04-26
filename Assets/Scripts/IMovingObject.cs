using UnityEngine;

public interface IMovingObject
{
    /// <summary>
    ///     Moves with a constant speed
    /// </summary>
    /// <returns> returns the direction vector </returns>
    Vector2 Move();
    /// <summary>
    ///     Stops the object
    /// </summary>
    void Stop();
}
