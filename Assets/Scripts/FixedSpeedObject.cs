using UnityEngine;

// Requires the following components to derive from this class
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class FixedSpeedObject : GameStopObserver, IMovingObject
{
    // Setting speed and direction to something random initially
    [Header("Variables")]
    public float speed = 5f;

    // For inherited classes to use, set it to protected
    protected Rigidbody2D rb;

    protected FixedSpeedObject() : this(GameManager.Instance) { }

    private FixedSpeedObject(GameStopNotifier gameStopNotifier) : base(GameManager.Instance) { }

    protected new void Start()
    {
        base.Start();

        // Assign 'rb' variable in the beginning
        rb = GetComponent<Rigidbody2D>();
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    public Vector2 Move()
    {
        Vector2 dir = (transform.GetChild(0).transform.position - transform.position).normalized;
        rb.MovePosition(Vector2.MoveTowards(transform.position, transform.GetChild(0).transform.position, speed * Time.deltaTime));
        return dir;
    }
}
