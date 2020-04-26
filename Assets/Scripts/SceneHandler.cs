using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    /// <summary>
    ///     Loads the scene with the next build index. Scenes must be added to the build
    /// </summary>
    public static void NextScene()
    {
        try {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } catch(UnityException e) {
            Debug.Log(e.StackTrace);
        }
    }
}
