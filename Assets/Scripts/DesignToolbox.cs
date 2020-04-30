#if UNITY_EDITOR_WIN
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class DesignToolbox : EditorWindow
{
    bool restartB;

    [MenuItem("Window/Design Toolbox")]
    public static void ShowWindow()
    {
        GetWindow(typeof(DesignToolbox), false, "Design Toolbox");
    }

    void OnGUI()
    {
        GUILayout.Label("Game Objects", EditorStyles.boldLabel);
        if (GUILayout.Button("Select Entrance Points"))
            SelectEntrancePoints();
        if (GUILayout.Button("Select Target Points"))
            SelectTargetPoints();
        if (GUILayout.Button("Create Obstacle"))
            CreateObstacle();

        try {
            GUILayout.Label("Car Settings", EditorStyles.boldLabel);
            Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().speed = EditorGUILayout.FloatField("Car Speed", (Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().GetComponent<Car>().speed));
            Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().rotation = EditorGUILayout.FloatField("Rotation Speed", (Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().GetComponent<Car>().rotation));
            SetCarsSpeedRotation(Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().speed, Resources.Load<GameObject>("Prefabs/Car").GetComponent<Car>().rotation);
        }
        catch(NullReferenceException) { }
        catch(InvalidCastException) { }
        
        GUILayout.Label("UI Settings", EditorStyles.boldLabel);
        //restartB = EditorGUILayout.Toggle("Restart Button", restartB);
        //GameObject.Find("Button - Restart").transform.GetChild(0).GetComponent<Text>().enabled = restartB;
        //GameObject.Find("Button - Restart").GetComponent<Image>().enabled = restartB;
    }

    private void SetCarsSpeedRotation(float speed, float rotation)
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            go.GetComponent<Car>().speed = speed;
            go.GetComponent<Car>().rotation= rotation;
        }
    }

    private void CreateObstacle()
    {
        Selection.activeObject = Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"), GameObject.Find("Obstacles").transform);
    }

    private void SelectEntrancePoints()
    {
        Selection.activeObject = GameObject.Find("Entrance Points");
    }

    private void SelectTargetPoints()
    {
        Selection.activeObject = GameObject.Find("Target Points");
    }
}
#endif