using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadedSceneWithVideo : MonoBehaviour
{
    public void Load(string value)
    {
        PlayerPrefs.SetString("VideoName", value);
        SceneManager.LoadScene("Anchor Sample");
    }
}
