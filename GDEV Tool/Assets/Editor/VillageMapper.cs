using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VillageMapper : EditorWindow 
{
    Object customBuilding;
    Object customRoad;
    GameObject customBuildingClone;

    bool hasPlaced;
    bool buildingButtonClicked;


    [MenuItem("Window/Village Mapper")] // Window Path.
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(VillageMapper)); // Shows editor window.
    }

    void OnGUI()
    {
        GUILayout.Label("Prefabs", EditorStyles.boldLabel); // Editor header.

        customBuilding = EditorGUILayout.ObjectField("Building", customBuilding, typeof(GameObject), true); // Field to insert custom building.

        if (GUILayout.Button("Generate Building"))          // Button to generate building.
        {
            if (!EditorApplication.isPlaying) Debug.Log("You need to be in Play Mode to use Village Mapper!"); // Check if editor is in play mode.

            hasPlaced = true;                   // For some reason this doesn't work if I set it to true when I first define it up above.

            if (hasPlaced)
                buildingButtonClicked = true;
        }

        EditorGUILayout.Space();                // A little personal space has never hurt anyone.

        customRoad = EditorGUILayout.ObjectField("Road", customRoad, typeof(GameObject), true);             // Room for the custom road placed between buildings.
    }

    private void Update()
    {
        if (buildingButtonClicked) // If the user has clicked the generate building button...
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);  
            hasPlaced = false;

            if (Input.GetMouseButtonDown(0)) // ... And then clicked with the mouse...
            {
                buildingButtonClicked = false;

                Vector3 worldPos;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))                   // Shoot some lasers at the position of the mouse. 1000f Is far clipping plane of camera.
                    worldPos = hit.point;                                   // Hey pep, Daan hier. Even een tip: Als je dit wilt debuggen, probeer Debug.DrawRay of DrawLine (Een van de twee). 
                else                                                        // Dan kan je zien wat de neuk er gebeurt.
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                Debug.Log(worldPos);                // Deze klopt.
                Debug.Log(Input.mousePosition);     // Deze schiet dus heel vaak naar de rand net uit het beeld. Hier ligt de fout dus.

                customBuildingClone = Instantiate(customBuilding,                       // Spawn het object.
                                                  worldPos,                             // Op de wereldpositie (gedefinieerd als de laser wordt geschoten.
                                                  Quaternion.identity) as GameObject;

                customBuildingClone.name = "Custom Building"; // Naampie.
            }
        }
    }
}