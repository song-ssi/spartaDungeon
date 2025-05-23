using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    enum State { Idle, Patrol, Detect, Attack};

    public Transform[] movePos;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float reachDistance = 0.8f;
    public float waitTime = 0.5f;

    [Header("Detect Settings")] //발견셋팅
    public Transform player;
    public float detectDistance = 5f;
    public float forgetDistance = 7f;
    public float attactDistance = 1.5f;
    
    // 순찰 할 목적지
    private Transform targetMovePos;
    private bool isWaiting = false;
    private State currentState = State.Patrol;

    void Start()
    {
        SetNewDestination();
    }

    void Update()
    {
        switch(currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Detect:
                Detect();
                break;
            case State.Attack:
                break;
        }

    }

    void Idle()
    {
        if(InRange(detectDistance)) ChangeState(State.Detect);
        
    }

    void Patrol()
    {
        if( isWaiting || targetMovePos == null)
        {
            return;
        }

        MoveTowards(targetMovePos.position);

        if(Vector3.Distance(transform.position, targetMovePos.position) <= reachDistance)
        {
            WaitAndResume();
        }

        if(InRange(detectDistance)) ChangeState(State.Detect);

    }

    void Detect()
    {
        // 1. 이동타겟 변경(플레이어)
        MoveTowards(player.position);

        // 2. 공격범위 또는 플레이어 잃어버리면 다시 순찰
        // if (InRange(attactDistance))
        //     ChangeState(State.Attack);
        if (InRange(forgetDistance) == false) ChangeState(State.Patrol);

    }

    void Attack()
    {
        // if (InRange(detectDistance))

        if(InRange(attactDistance) == false)
        {
            ChangeState(State.Detect);
        }
    }
    void ChangeState(State newState)
    {
        if( currentState != newState)
            currentState = newState;
    }

    bool InRange(float range)
    {
        if( player == null) return false;

        return Vector3.Distance(transform.position, player.position) <= range;
    }

    void WaitAndResume()
    {
        isWaiting = true;
        Invoke("SetNewDestination", waitTime);
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if(dir != Vector3.zero)
        {
            dir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void SetNewDestination()
    {
        Transform next;
        do
        {
            next = movePos[1];
        }
        while(next == targetMovePos);


        isWaiting = false;
        targetMovePos = next;
    }
    
}
