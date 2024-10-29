using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneMenuToolbar
{
    [MenuItem("Level Loader/Load Showcase #&1")]
    public static void LoadMainMenu()
    {
        LoadScene("Assets/_Scenes/Showcase.unity");
    }

    private static void LoadScene(string scenePath)
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }
}
