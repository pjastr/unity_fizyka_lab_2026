using UnityEngine;

// Klocek wieży. Masa liczona z gęstości i rozmiaru (Unity nie robi tego samo).
// ROZWIĄZANIE - Lab04, krok 2.2
public class Block : Breakable
{
    [SerializeField, Tooltip("Umowna gęstość: masa = gęstość × objętość")]
    private float density = 2f;

    protected override void Awake()
    {
        base.Awake();
        UpdateMass();
    }

    // Wywołaj po każdej zmianie skali klocka!
    public void UpdateMass()
    {
        Vector3 s = transform.lossyScale;
        // TODO 1 (rozwiązanie): Cube ma wymiary 1×1×1, więc objętość = s.x · s.y · s.z
        rb.mass = density * s.x * s.y * s.z;
    }
}
