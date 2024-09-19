using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneMenuToolbar : MonoBehaviour
{
    [MenuItem("Level Loader/Load Main Menu #&1")]
    public static void LoadMainMenu()
    {
        LoadScene("Assets/Scenes/main_menu.unity");
    }

    private static void LoadScene(string scenePath)
    {
        // Save just in case
        EditorSceneManager.SaveOpenScenes();
        // Load the given scene
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }
}
