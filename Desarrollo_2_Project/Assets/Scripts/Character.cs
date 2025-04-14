using UnityEngine;

/// <summary>
/// Moves the character, controls everything related to the world posiition
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    private ForceRequest _instantForceRequest;
    private ForceRequest _continuousForceRequest;
    private Rigidbody _rigidBody;

    public void RequestForce(ForceRequest forceRequest)
    {
        _instantForceRequest = forceRequest;
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_continuousForceRequest != null)
        {
            //_rigidBody.AddForce(_continuousForceRequest.direction * _continuousForceRequest.force, ForceMode.Force);
        }
    }

}
