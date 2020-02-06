using UnityEngine;

public class CubeParts : MonoBehaviour
{
    public new Rigidbody rigidbody;
    private void Awake()
    {
        rigidbody.AddForce(Random.insideUnitSphere*10f,ForceMode.Impulse);
    }
}
