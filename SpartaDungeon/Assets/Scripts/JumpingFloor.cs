using UnityEngine;


public class JumpingFloor : MonoBehaviour
{
    public float powerJumpingForce;
    Rigidbody _rigidbody;

    private void OnCollisionEnter(Collision other)
    {
        _rigidbody = other.gameObject.GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector2.up * powerJumpingForce, ForceMode.Impulse);
    }
}
