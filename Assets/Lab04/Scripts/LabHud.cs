using UnityEngine;

// POMOCNICZE (nie jest częścią zadania): prosty podgląd stanu gry w rogu ekranu.
// Używa IMGUI (OnGUI), więc nie wymaga Canvasa.
public class LabHud : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private LevelJudge judge;

    private GUIStyle style;

    void OnGUI()
    {
        if (launcher == null || judge == null) return;
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 15,
                richText = true,
                padding = new RectOffset(10, 10, 8, 8)
            };
        }

        string text =
            "<b>Lab04 - proca i wieża</b>\n" +
            $"Kąt: {launcher.Pitch:0}°   Kierunek: {launcher.Yaw:0}°\n" +
            $"Impuls: {launcher.Impulse:0} N·s   v0 = {launcher.MuzzleSpeed:0.0} m/s\n" +
            $"Strzały: {launcher.ShotsLeft}/{launcher.MaxShots}   Cele: {judge.TargetsLeft}\n" +
            $"Fixed Timestep: {Time.fixedDeltaTime:0.###} s\n" +
            (string.IsNullOrEmpty(judge.Status) ? "" : $"<b>{judge.Status}</b>\n") +
            "\n←/→ ↑/↓ celowanie, W/S siła, Spacja strzał";

        GUI.Box(new Rect(10, 10, 380, 165), text, style);
    }
}
