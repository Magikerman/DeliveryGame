using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject begginButton;
    [SerializeField] private GameObject mapPlanTexture;
    public void SceneConfirmButton()
    {
        begginButton.SetActive(true);
        mapPlanTexture.SetActive(true);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
