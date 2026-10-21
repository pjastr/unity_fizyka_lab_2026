using UnityEngine;

// Cel ("świnia"): pęka od uderzenia albo po spadnięciu z planszy.
// ROZWIĄZANIE - Lab04, krok 2.3
public class Target : Breakable
{
    [SerializeField, Tooltip("Poniżej tej wysokości cel uznajemy za strącony")]
    private float fallLimit = -3f;

    void FixedUpdate()
    {
        if (transform.position.y < fallLimit) Break();
    }

    protected override void OnBreak()
    {
        Debug.Log($"<color=lime>[{name}] CEL TRAFIONY</color>");
    }
}
