using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelEnd : MonoBehaviour
{
    public string sceneName;
    public SceneSwitcher SceneSwitcher;
    void OnTriggerEnter(Collider col)
    { 
        if (col.gameObject.tag == "Player") 
        { 
            SceneSwitcher.SwitchScene(sceneName);
        }
    }
}
