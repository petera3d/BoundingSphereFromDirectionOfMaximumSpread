using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoundingSphereFromDirectionOfMaximumSpread))]
public class BoundingSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BoundingSphereFromDirectionOfMaximumSpread bs = (BoundingSphereFromDirectionOfMaximumSpread)target;

        GUILayout.Space(10);
        GUILayout.Label("Set Points Amount");

        float sValue = GUILayout.HorizontalSlider(bs.PointsAmount, 0, 100);
        bs.PointsAmount = math.clamp(Mathf.RoundToInt(sValue), 0, 100);

        GUILayout.Space(15);

        if (GUILayout.Button("Generate Random Points"))
        {
            bs.GenerateRandomPoints();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("CreateBoundShere"))
        {
            if (bs.PointsAmount == 0) return;
            bs.CreateBoundShere();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Reset"))
        {
            if (bs.PointsAmount == 0) return;
            GUILayout.HorizontalSlider(bs.PointsAmount, 0, 100);
            bs.Reset();
        }
    }
}
