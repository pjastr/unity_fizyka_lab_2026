using System.Collections;
using UnityEngine;

// Sędzia: ogłasza wynik, gdy wszystkie ciała zasną (IsSleeping) albo minie maxWait.
// ROZWIĄZANIE - Lab04, krok 5 (z poprawionym błędem wykrywanym przez test T8)
public class LevelJudge : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private TowerBuilder tower;
    [SerializeField, Tooltip("Maks. czas czekania na uspokojenie sceny [s]")]
    private float maxWait = 10f;

    private bool judging;

    // tylko do odczytu - dla HUD-a (LabHud)
    public string Status { get; private set; } = "";

    // GetComponentsInChildren pomija obiekty nieaktywne (patrz TowerBuilder.Clear)
    public int TargetsLeft => tower.GetComponentsInChildren<Target>().Length;

    void Update()
    {
        if (judging) return;
        if (TargetsLeft == 0 || launcher.ShotsLeft == 0)
            StartCoroutine(Judge());
    }

    private IEnumerator Judge()
    {
        judging = true;
        Status = "Czekam, aż scena się uspokoi...";
        Debug.Log(Status);

        float t = 0f;
        // TODO 7 (rozwiązanie)
        while (t < maxWait && !SceneAsleep())
        {
            t += Time.deltaTime;
            yield return null;
        }

        int left = TargetsLeft;
        if (left == 0)
        {
            // Poprawka T8: bez Mathf.Min wygrana bez strzału dawała 4 gwiazdki
            int stars = Mathf.Min(3, 1 + launcher.ShotsLeft);
            Status = $"WYGRANA! Gwiazdki: {stars}";
            Debug.Log($"<color=yellow>{Status}</color>");
        }
        else
        {
            Status = $"PRZEGRANA - zostało celów: {left}";
            Debug.Log($"<color=red>{Status}</color>");
        }
    }

    private bool SceneAsleep()
    {
        foreach (Rigidbody rb in tower.GetComponentsInChildren<Rigidbody>())
            if (!rb.IsSleeping()) return false;

        // TODO 8 (rozwiązanie): operator != Unity zwraca false także dla zniszczonego obiektu
        Rigidbody p = launcher.LastProjectile;
        if (p != null && !p.IsSleeping()) return false;

        return true;
    }

    [ContextMenu("TEST / Czy scena śpi?")]
    private void TestAsleep() => Debug.Log($"Scena śpi: {SceneAsleep()}, celów: {TargetsLeft}");

    [ContextMenu("TEST / Nowa runda")]
    public void NewRound()
    {
        StopAllCoroutines();
        judging = false;
        Status = "";
        tower.Build();
        launcher.ResetShots();
    }
}
