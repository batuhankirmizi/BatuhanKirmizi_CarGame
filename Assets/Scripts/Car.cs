using System;
using UnityEngine;

public class Car : FixedSpeedObject, IUserControllable, IRotatable
{
    // 'rotation' variable is actually the angle which the object will turn for each second
        // but "Rotation Speed" makes it easier to understand
    [Header("Rotation Speed")]
    public float rotation = 25f;

    // Initially, 0 rotation
    private float rotate = 0f;

    // Whether this is the active, user controlling car among other cars in the scene or not
    public bool isActive = true;

    // Path of the car
        // If user is controlling the car, path becomes created
    public Path<Vector2> path;

    // Car's entrance and target point GOs
        // set in ObjectPooler when the car is instantiated at the beginning
    [NonSerialized] public GameObject entrancePoint;
    [NonSerialized] public GameObject targetPoint;

    private new void Start()
    {
        base.Start();
        name = "Car - Active";
        path = new Path<Vector2>(this);
    }

    private void Update()
    {
        if (!GameManager.Instance.GetGameStopped()) {
            if (isActive) {
                Move();
                Rotate();
                path.CreatePath(transform.position);
            } else {
                MoveAlongPath();
            }
        }
    }

    // Stores the current point that the car moves on the path
    private int currentPathPoint = 0;
    private void MoveAlongPath()
    {
        MoveTo(path.GetPoint(currentPathPoint));
    }

    private float rotationAngle;
    private void MoveTo(Vector3 position)
    {
        if (currentPathPoint < path.PointCount - 1) {
            Vector2 v = (position - transform.position).normalized * speed * Time.deltaTime;
            rb.MovePosition(new Vector2(transform.position.x + v.x, transform.position.y + v.y));
            if (Vector2.Distance(position, transform.position) <= .1f)
                currentPathPoint++;

            // Angle of the car's rotation using tangent formula
            rotationAngle = -Mathf.Atan2(position.x - transform.position.x, position.y - transform.position.y);
            // Lerp for smoother rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotationAngle * Mathf.Rad2Deg), (rotation / (speed * 4)) * Time.deltaTime);
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    public void Rotate()
    {
        rotate = 0f;
        if (GameManager.Instance.rightBClicked)
            TurnRight();
        else if (GameManager.Instance.leftBClicked)
            TurnLeft();

        rb.MoveRotation(rb.rotation + rotate * Time.deltaTime);
    }

    /// <summary>
    ///     Turning the object right actually means to rotate it with a specific amount of degrees on z axis, clockwise
    /// </summary>
    public void TurnRight()
    {
        rotate = -rotation;
    }

    /// <summary>
    ///     Turning the object left actually means to rotate it with a specific amount of degrees on z axis, counterclockwise
    /// </summary>
    public void TurnLeft()
    {
        rotate = rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Previously driven car
        //if (collision.collider.CompareTag("Obstacle") ||
        //    collision.collider.CompareTag("Edge")     ||
        //    (collision.collider.CompareTag("Player") && collision.collider.gameObject.Equals(ObjectPooler.Instance.GetCar(GameManager.Instance.currentPoint - 1))))
        //    CollisionReset();

        // Previously driven cars
        if (collision.collider.CompareTag("Obstacle") ||
            collision.collider.CompareTag("Edge") ||
            (collision.collider.CompareTag("Player")))
            CollisionReset();
    }

    private void CollisionReset()
    {
        GameManager.Instance.SetGameStopped(true);
        currentPathPoint = 0;
        if(!path.IsCreated)
            path.Reset();
        GameManager.Instance.ResetAllCars();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Target Point") && isActive &&
            targetPoint.gameObject.Equals(collision.gameObject)) {
            path.AddPoint(transform.position);
            path.IsCreated = true;
            isActive = false;
            rb.isKinematic = true;
            name = "Car";

            if (GameManager.Instance.currentPoint == ObjectPooler.Instance.totalCarCount - 1)
                GameManager.Instance.gameOver = true;
            else {
                GameManager.Instance.ResetAllCars();
                GameManager.Instance.NextPoint();
            }
        }
    }

    /// <summary>
    ///     Resets the car's: position, rotation, velocity and instance variables
    /// </summary>
    public void Reset()
    {
        transform.position = entrancePoint.transform.position;
        transform.rotation = entrancePoint.transform.rotation;
        rotate = 0f;
        rb.velocity = Vector2.zero;

        if (!rb.isKinematic) {
            rb.bodyType = RigidbodyType2D.Static;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (isActive)
            path.Reset();
        else {
            rb.freezeRotation = true;
            currentPathPoint = 0;
        }
    }

    public override void OnGameStopped()
    {
        Stop();
    }

    public override void OnGameRun()
    {
        Move();
    }
}
