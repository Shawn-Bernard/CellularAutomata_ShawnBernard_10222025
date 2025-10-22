using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{
    [SerializeField] public bool is2D = false;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D _rigidbody2D;
    private Rigidbody _rigidbody3D;

    private BoxCollider2D boxCollider2D;
    private BoxCollider boxCollider;

    private Vector3 moveVector;

    void Start()
    {
        if (is2D)
        {
            _rigidbody2D = GetOrAddComponent<Rigidbody2D>();
            boxCollider2D = GetOrAddComponent<BoxCollider2D>();
            _rigidbody2D.freezeRotation = true;
            _rigidbody2D.gravityScale = 0;
        }
        else
        {
            _rigidbody3D = GetOrAddComponent<Rigidbody>();
            boxCollider = GetOrAddComponent<BoxCollider>();
            _rigidbody3D.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (is2D && _rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = new Vector2(moveVector.x * moveSpeed, moveVector.y * moveSpeed);

        }
        else if (!is2D && _rigidbody3D != null)
        {
            Vector3 velocity = new Vector3(moveVector.x, _rigidbody3D.linearVelocity.y, moveVector.z);
            _rigidbody3D.linearVelocity = velocity * moveSpeed;

        }
    }

    public T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }


    public void Move(Vector3 _moveVector)
    {
        moveVector = _moveVector;
    }
}
