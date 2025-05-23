using System.Security.Principal;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingFloor : MonoBehaviour
{
    public Transform[] movePos;
    public float moveSpeed = 2f;
    public float waitTime = 0.5f;
    public int index;
    private bool isMoving = true;


    // 순회할 목적지
    private Transform targetMovePos;

    void Awake()
    {
        
    }
    void Start()
    {
        StartSetting();
        SetNewDestination();
    }

    void Update()
    {
        if(isMoving)
        {
            GoToTargetMovePos();
        }
        
    }

    // watiTime 만큼 쉬고 SetNewDestination 함수 불러옴    
    void WaitAndResume()
    {
        Invoke("SetNewDestination", waitTime);
    }

    // 스타트셋팅 - 포지션을 movePos[0]의 포지션으로.
    void StartSetting()
    {
        transform.position = movePos[0].position;
        index = 0;
    }

    // 새로운 목적지 찾기
    void SetNewDestination()
    {
        index++;
        if(index >= movePos.Length)
        {
            index = 0;
        }

        targetMovePos = movePos[index];
        isMoving = true;
    }

    // 목적지로 움직임
    void GoToTargetMovePos()
    {   
        // targetMovepos로 가라.
        transform.position = Vector3.MoveTowards(transform.position, targetMovePos.position, moveSpeed * Time.deltaTime);

        if (transform.position == targetMovePos.position)
        {
            isMoving = false;
            WaitAndResume();
        }
    }

        void OnCollisionStay(Collision other)       
        {
            Vector3 newPos = other.transform.position;
            newPos.x = transform.position.x;
            newPos.z = transform.position.z;
            other.transform.position = newPos;

        }

}
