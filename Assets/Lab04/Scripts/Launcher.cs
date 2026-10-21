using UnityEngine;
using UnityEngine.InputSystem;

// Wyrzutnia: celowanie strzałkami, siła W/S, strzał Spacją, podgląd toru lotu.
// ROZWIĄZANIE - Lab04, krok 4
[RequireComponent(typeof(LineRenderer))]
public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody projectilePrefab;
    [SerializeField, Tooltip("Punkt wylotu - dziecko wyrzutni")]
    private Transform muzzle;

    [Header("Celowanie")]
    [SerializeField, Range(0f, 80f)] private float pitch = 20f;      // kąt w pionie [°]
    [SerializeField, Range(-45f, 45f)] private float yaw = 0f;       // kąt w poziomie [°]
    [SerializeField, Range(1f, 300f)] private float impulse = 40f;   // [N·s]
    [SerializeField] private float aimSpeed = 30f;                   // [°/s]
    [SerializeField] private float powerSpeed = 20f;                 // [N·s na sekundę]

    [Header("Strzały")]
    [SerializeField, Range(1, 20)] private int maxShots = 3;
    [SerializeField] private float projectileLifetime = 15f;

    [Header("Trajektoria")]
    [SerializeField, Range(10, 100)] private int trajectoryPoints = 40;
    [SerializeField] private float trajectoryStep = 0.05f;           // [s] między punktami

    private LineRenderer line;

    public int ShotsLeft { get; private set; }
    public int MaxShots => maxShots;
    public Rigidbody LastProjectile { get; private set; }

    // tylko do odczytu - dla HUD-a (LabHud)
    public float Pitch => pitch;
    public float Yaw => yaw;
    public float Impulse => impulse;
    public float MuzzleSpeed => projectilePrefab != null ? impulse / projectilePrefab.mass : 0f;

    void Awake() => line = GetComponent<LineRenderer>();

    void Start() => ResetShots();

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        // TODO 4 (rozwiązanie)
        float dt = Time.deltaTime;
        if (kb.leftArrowKey.isPressed)  yaw -= aimSpeed * dt;
        if (kb.rightArrowKey.isPressed) yaw += aimSpeed * dt;
        if (kb.upArrowKey.isPressed)    pitch += aimSpeed * dt;
        if (kb.downArrowKey.isPressed)  pitch -= aimSpeed * dt;
        if (kb.wKey.isPressed) impulse += powerSpeed * dt;
        if (kb.sKey.isPressed) impulse -= powerSpeed * dt;

        yaw = Mathf.Clamp(yaw, -45f, 45f);
        pitch = Mathf.Clamp(pitch, 0f, 80f);
        impulse = Mathf.Clamp(impulse, 1f, 300f);

        transform.rotation = Quaternion.Euler(-pitch, yaw, 0f);   // minus: dodatni kąt wokół X pochyla w dół
        DrawTrajectory();

        if (kb.spaceKey.wasPressedThisFrame) Fire();
    }

    [ContextMenu("TEST / Strzał")]
    public void Fire()
    {
        if (ShotsLeft <= 0) { Debug.Log("Brak strzałów"); return; }
        ShotsLeft--;

        Rigidbody p = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
        // TODO 5 (rozwiązanie): impuls -> Δv = J / m
        p.AddForce(muzzle.forward * impulse, ForceMode.Impulse);
        Destroy(p.gameObject, projectileLifetime);
        LastProjectile = p;

        Debug.Log($"Strzał: J = {impulse:0} N·s, v0 = {impulse / p.mass:0.0} m/s, zostało {ShotsLeft}");
    }

    public void ResetShots() => ShotsLeft = maxShots;

    private void DrawTrajectory()
    {
        Vector3 p0 = muzzle.position;
        Vector3 v0 = muzzle.forward * (impulse / projectilePrefab.mass);   // Δv = J / m
        Vector3 g = Physics.gravity;

        line.positionCount = trajectoryPoints;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * trajectoryStep;
            // TODO 6 (rozwiązanie): rzut ukośny p(t) = p0 + v0·t + ½·g·t²
            Vector3 p = p0 + v0 * t + 0.5f * g * t * t;
            line.SetPosition(i, p);
        }
    }
}
