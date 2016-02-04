using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEditor.SceneManagement;

/*
app_icon.png
drawable 48
drawable-hdpi 72
drawable-large-mdpi 200
drawable-ldpi 36
drawable-mdpi 48
drawable-xhdpi 96
drawable-xxhdpi 144
drawable-xxxhdpi 192
 */
public class EditorToolsObject : ScriptableObject
{
    [SerializeField]
    public Sprite
        exampleProperty;
    public string findTextProp = "e";
    public List<string> resultObjectsProp = new List<string>();
    public string componentName = "LoadGameImage";
}

public class EditorTools : EditorWindow
{
    SerializedProperty exampleProperty;
    SerializedProperty findTextProp;
    //SerializedProperty resultObjectsProp;
    EditorToolsObject editorToolsObject;
    SerializedProperty componentNameProp;

    [MenuItem("Window/EditorTools")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorTools));
    }

    void OnEnable()
    {
        if (editorToolsObject == null || findTextProp == null)
        {
            editorToolsObject = ScriptableObject.CreateInstance<EditorToolsObject>();
            SerializedObject serializedObj = new SerializedObject(editorToolsObject);
            exampleProperty = serializedObj.FindProperty("exampleProperty");
            findTextProp = serializedObj.FindProperty("findTextProp");
            componentNameProp = serializedObj.FindProperty("componentName");
            //resultObjectsProp = serializedObj.FindProperty("resultObjectsProp");
        }
    }

    void OnGUI()
    {
        EditorGUILayout.PropertyField(exampleProperty);

        if (GUILayout.Button("PlayerPrefs.DeleteAll()"))
        {
            PlayerPrefs.DeleteAll();
        }

        EditorGUILayout.PropertyField(findTextProp);
        if (GUILayout.Button("Find in tk2d meshes"))
        {
            // editorToolsObject.resultObjectsProp.Clear();
            // foreach (var c in FindObjectsOfType<tk2dTextMesh>())
            // {
            //     if (c.text.Contains(findTextProp.stringValue))
            //         editorToolsObject.resultObjectsProp.Add(c.gameObject.name);
            // }
            // foreach (var c in FindObjectsOfType<TextMesh>())
            // {
            //     if (c.text.Contains(findTextProp.stringValue))
            //         editorToolsObject.resultObjectsProp.Add(c.gameObject.name);
            // }

            string result = "RESULT: \n";
            // foreach (var it in editorToolsObject.resultObjectsProp)
            //     result += it+"\n";
            Debug.Log(result);

            EditorUtility.SetDirty(editorToolsObject);
        }
        //EditorGUILayout.PropertyField(resultObjectsProp);


        if (GUILayout.Button("SaveScreenshot"))
        {
            Application.CaptureScreenshot("screenshot_" + EditorSceneManager.GetActiveScene().name + ".png");
        }
        if (GUILayout.Button("GeneratePrism"))
        {
            var prism = new Mesh();
            const int count = 3;
            var verts = new Vector3[count];
            var indices = new int[3 * 2 * count];
            for (int i = 0; i < count; i++)
            {
                float step = Mathf.PI * 2 / count;
                verts[i] = new Vector3(Mathf.Cos(step * i), Mathf.Sin(step * i), 0);
                verts[i] = new Vector3(Mathf.Cos(step * i), Mathf.Sin(step * i), 1);
            }

            prism.vertices = verts;
            prism.SetIndices(indices, MeshTopology.LineStrip, 0);
        }

        GUILayout.Label("---------");
        //EditorGUILayout.LabelField("Application.persistentDataPath");
        //EditorGUILayout.LabelField(Application.persistentDataPath);
    }

    static void SwitchBrand()
    {
        var path = EditorUtility.OpenFolderPanel("Find custom brand dir", "~/Dropbox", "default name");

    }

    private static string[] FillLevels()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }

    [MenuItem("MyTool/ScreenshotAllScenes")]
    public static void ScreenshotAllScenes()
    {
        var origScene = EditorApplication.currentScene;
        StringBuilder sb = new StringBuilder();
        string[] levels = FillLevels();
        foreach (string level in levels)
        {
            EditorApplication.OpenScene(level);
            SceneView.RepaintAll();
            HandleUtility.Repaint();
            Application.CaptureScreenshot("screenshot_" + EditorApplication.currentScene + ".png");
            Application.CaptureScreenshot("screenshot_" + Application.loadedLevelName + ".png");
        }
        Debug.Log(sb);
        EditorApplication.OpenScene(origScene);
    }



    public static void FindAllTextures()
    {
        StringBuilder sb = new StringBuilder();
        string[] levels = FillLevels();
        foreach (string level in levels)
        {
            EditorApplication.OpenScene(level);
            sb.Append("\n=== scene: " + EditorApplication.currentScene + " ===\n");
            {
                var comps = FindObjectsOfType<Image>();
                foreach (var comp in comps)
                    if (comp.sprite)
                        sb.Append("go: " + comp.gameObject.name + " tex: " + comp.sprite.texture.name + "\n");
            }
            //{
            //    var comps = FindObjectsOfType<Renderer>();
            //    foreach(var comp in comps)
            //        sb.Append( "key: "+comp.material.mainTexture.name + "\n");
            //}
        }
        Debug.Log(sb);
    }

    public static void FindAcrossScenes(System.Type componentType)
    {
        var origScene = EditorApplication.currentScene;
        StringBuilder sb = new StringBuilder();
        string[] levels = FillLevels();
        foreach (string level in levels)
        {
            EditorApplication.OpenScene(level);
            sb.Append("\n=== scene: " + EditorApplication.currentScene + " ===\n");
            {
                var comps = FindObjectsOfType(componentType);
                foreach (var comp in comps)
                    sb.Append("go: " + comp.name + "\n");
            }
        }
        Debug.Log(sb);
        EditorApplication.OpenScene(origScene);
    }



    public static IEnumerable<GameObject> EnumerateSceneRootObjects()
    {
        var prop = new HierarchyProperty(HierarchyType.GameObjects);
        var expanded = new int[0];
        while (prop.Next(expanded))
        {
            yield return prop.pptrValue as GameObject;
        }
    }


    /// <summary>
    /// Not working very well
    /// </summary>
    public static void PrintAllObjects()
    {
        StringBuilder accumulatingMessage = new StringBuilder();
        foreach (var rootGo in EnumerateSceneRootObjects())
        {
            //StaticHelper.RecursePrint(rootGo.transform, accumulatingMessage, 0);
        }
        Debug.Log(accumulatingMessage.ToString());
    }
}