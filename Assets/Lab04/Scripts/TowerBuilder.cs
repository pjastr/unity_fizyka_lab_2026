using UnityEngine;

// Buduje wieżę: na każdym piętrze dwa filary, cel i belka (strop).
// ROZWIĄZANIE - Lab04, krok 3
public class TowerBuilder : MonoBehaviour
{
    [SerializeField, Tooltip("Materiał kolejnych pięter (cyklicznie)")]
    private Block[] floorPrefabs;
    [SerializeField] private Target targetPrefab;

    [SerializeField, Range(1, 8)] private int floors = 3;
    [SerializeField, Range(1f, 4f)] private float span = 2f;   // rozstaw osi filarów [m]
    [SerializeField] private Vector3 pillarSize = new Vector3(0.3f, 1.5f, 1f);
    [SerializeField, Range(0.1f, 0.6f)] private float beamHeight = 0.3f;

    void Start() => Build();

    [ContextMenu("Przebuduj wieżę")]
    public void Build()
    {
        Clear();
        float y = 0f;   // wysokość podłogi bieżącego piętra (lokalnie)

        for (int i = 0; i < floors; i++)
        {
            Block prefab = floorPrefabs[i % floorPrefabs.Length];

            // filary
            float py = y + pillarSize.y / 2f;
            SpawnBlock(prefab, new Vector3(-span / 2f, py, 0f), pillarSize, $"P{i}_Filar_L");
            SpawnBlock(prefab, new Vector3( span / 2f, py, 0f), pillarSize, $"P{i}_Filar_P");

            // TODO 3 (rozwiązanie): cel leży na podłodze piętra - środek kulki o promień wyżej
            float radius = targetPrefab.transform.localScale.y / 2f;
            Target t = Instantiate(targetPrefab, transform);
            t.name = $"P{i}_Cel";
            t.transform.localPosition = new Vector3(0f, y + radius, 0f);
            t.transform.localRotation = Quaternion.identity;

            // belka (strop) na filarach
            y += pillarSize.y;
            Vector3 beamSize = new Vector3(span + pillarSize.x, beamHeight, pillarSize.z);
            SpawnBlock(prefab, new Vector3(0f, y + beamHeight / 2f, 0f), beamSize, $"P{i}_Belka");
            y += beamHeight;
        }
    }

    private void SpawnBlock(Block prefab, Vector3 localPos, Vector3 size, string blockName)
    {
        Block b = Instantiate(prefab, transform);
        b.name = blockName;
        b.transform.localPosition = localPos;
        b.transform.localRotation = Quaternion.identity;
        b.transform.localScale = size;
        b.UpdateMass();   // Awake() policzył masę jeszcze dla skali 1×1×1!
    }

    private void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);   // Destroy działa dopiero na końcu klatki
            Destroy(child);
        }
    }
}
