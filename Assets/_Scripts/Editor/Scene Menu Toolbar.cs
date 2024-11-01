using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneMenuToolbar
{
    [MenuItem("Level Loader/Load Main Menu #&1")]
    public static void LoadMainMenu()
    {
        LoadScene("Assets/_Scenes/MainMenu.unity");
    }

    [MenuItem("Level Loader/Load Level 1 #&2")]
    public static void LoadLevel_1()
    {
        LoadScene("Assets/_Scenes/Level 1.unity");
    }

    [MenuItem("Level Loader/Load Level 2 #&3")]
    public static void LoadLevel_2()
    {
        LoadScene("Assets/_Scenes/Level 2.unity");
    }

    [MenuItem("Level Loader/Load Showcase #&4")]
    public static void LoadShowcase()
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