using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLogin : MonoBehaviour
{
    public string sceneToLoad;

    public void SceneLoad()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
