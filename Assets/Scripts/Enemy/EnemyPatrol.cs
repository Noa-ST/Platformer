
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behavior ")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator amin;

    private void Awake()
    {
        initScale = enemy.localScale;
        amin = enemy.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        amin.SetBool("move", false);
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
                MoveIndirection(-1);
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
                MoveIndirection(1);
            else
            {
                DirectionChange();
            }
        }
    }
    
    private void DirectionChange()
    {
        amin.SetBool("move", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveIndirection(int _direction)
    {
        idleTimer = 0;
        amin.SetBool("move", true);

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
                                     enemy.position.y,
                                     enemy.position.z);
    }


}
