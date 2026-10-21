using UnityEngine;

// Wspólna klasa bazowa: wszystko, co może pęknąć od uderzenia.
// ROZWIĄZANIE - Lab04, krok 2.1
[RequireComponent(typeof(Rigidbody))]
public class Breakable : MonoBehaviour
{
    [SerializeField, Tooltip("Impuls [N·s], powyżej którego obiekt pęka")]
    private float strength = 6f;

    [SerializeField, Tooltip("Czas [s] po pojawieniu się, w którym uderzenia są ignorowane")]
    private float graceTime = 0.5f;

    protected Rigidbody rb;
    private float spawnTime;
    private bool broken;

    public bool IsBroken => broken;
    public float Strength => strength;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time;
    }

    void OnCollisionEnter(Collision c)
    {
        // Wieża "osiada" chwilę po zbudowaniu - te kontakty pomijamy
        if (Time.time - spawnTime < graceTime) return;

        float j = c.impulse.magnitude;   // [N·s]

        // TODO 2 (rozwiązanie)
        if (j > strength)
        {
            Debug.Log($"[{name}] pęka od {c.gameObject.name}: impuls {j:0.00} N·s (próg {strength:0.#} N·s)");
            Break();
        }
    }

    public void Break()
    {
        if (broken) return;   // kilka kontaktów w jednym kroku = kilka wywołań!
        broken = true;
        OnBreak();
        Destroy(gameObject);
    }

    // Miejsce na reakcję klas pochodnych (log, punkty, odłamki...)
    protected virtual void OnBreak() { }

    [ContextMenu("TEST / Pęknij")]
    private void TestBreak() => Break();
}
