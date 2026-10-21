using UnityEngine;

// Kamera śledząca ostatni pocisk; gdy pocisk zwolni - wraca do widoku ogólnego.
// Lab04, krok 6.2 (kod podany w instrukcji)
public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -4f);
    [SerializeField] private float minSpeed = 0.5f;   // wolniej = wracamy do widoku ogólnego

    private Vector3 homePos;
    private Quaternion homeRot;

    void Start()
    {
        homePos = transform.position;
        homeRot = transform.rotation;
    }

    void LateUpdate()
    {
        Rigidbody p = launcher.LastProjectile;
        if (p == null || p.linearVelocity.magnitude < minSpeed)
        {
            transform.SetPositionAndRotation(homePos, homeRot);
            return;
        }

        // transform.position, a NIE p.position - tylko Transform jest interpolowany
        Vector3 target = p.transform.position;
        transform.position = target + offset;
        transform.LookAt(target);
    }
}
