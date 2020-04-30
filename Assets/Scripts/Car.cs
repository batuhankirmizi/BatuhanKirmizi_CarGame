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
    public Path<float> path;

    // Object's lifetime
    private float lifetime = 0f;

    // Car's entrance and target point GOs
        // set in ObjectPooler when the car is instantiated at the beginning
    [NonSerialized] public GameObject entrancePoint;
    [NonSerialized] public GameObject targetPoint;

    private new void Start()
    {
        base.Start();
        name = "Car - Active";
        path = new Path<float>(this);
    }

    private void Update()
    {
        if (!GameManager.Instance.GetGameStopped()) {
            if (isActive)
                InputHandling();
            lifetime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GetGameStopped()) {
            if (isActive) {
                Move();
            } else {
                MoveAlongPath();
            }
            Rotate();
        }
    }

    private void InputHandling()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !GameManager.Instance.GetGameStopped()) {
            TurnLeft();
            path.CreatePath(lifetime, Direction.LEFT);
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && !GameManager.Instance.GetGameStopped()) {
            TurnRight();
            path.CreatePath(lifetime, Direction.RIGHT);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            rotate = 0f;
            path.CreatePath(lifetime, Direction.NONE);
        }

    }

    // Stores the current point that the car moves on the path
    private int currentPathPoint = 0;
    private void MoveAlongPath()
    {
        if (currentPathPoint < path.PointCount)
            MoveTo(path.GetPoint(currentPathPoint), path.GetDirection(currentPathPoint));
    }

    private void MoveTo(float time, Direction dir)
    {
        Move();

        switch (dir) {
            case Direction.LEFT:
                rotate = rotation;
                break;
            case Direction.RIGHT:
                rotate = -rotation;
                break;
            case Direction.NONE:
                rotate = 0f;
                break;
        }

        if (lifetime >= time)
            currentPathPoint++;
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, Mathf.Lerp(transform.rotation.z, rotate * Time.deltaTime, rotation * Time.deltaTime)));
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
        lifetime = 0f;
        if(!path.IsCreated)
            path.Reset();
        GameManager.Instance.ResetAllCars();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Target Point") && isActive &&
            targetPoint.gameObject.Equals(collision.gameObject)) {
            path.IsCreated = true;
            Stop();
            path.CreatePath(lifetime, Direction.NONE);
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
        lifetime = 0f;

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
