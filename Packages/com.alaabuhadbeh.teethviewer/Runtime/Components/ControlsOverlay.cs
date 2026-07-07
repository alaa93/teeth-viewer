using UnityEngine;

namespace AlaAbuhadbeh.TeethViewer
{
    public sealed class ControlsOverlay : MonoBehaviour
    {
        private static readonly string[] Lines =
        {
            "Left Mouse Button  —  Select a tooth",
            "Right Mouse Drag  —  Rotate the selected tooth",
            "Mouse Wheel Scroll  —  Zoom in / out",
            "Hold Mouse Wheel + Drag  —  Orbit around the model",
        };

        private GUIStyle _label;

        private void OnGUI()
        {
            if (_label == null)
            {
                _label = new GUIStyle(GUI.skin.label);
                _label.fontSize = 14;
                _label.normal.textColor = Color.white;
                _label.wordWrap = false;
            }

            const float pad = 12f;
            const float lineH = 22f;
            const float w = 380f;
            float h = pad * 2f + lineH * Lines.Length;
            var panel = new Rect(pad, pad, w, h);

            var prev = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.55f);
            GUI.DrawTexture(panel, Texture2D.whiteTexture);
            GUI.color = prev;

            for (int i = 0; i < Lines.Length; i++)
            {
                var r = new Rect(panel.x + pad, panel.y + pad + i * lineH, w - 2f * pad, lineH);
                GUI.Label(r, Lines[i], _label);
            }
        }
    }
}
