using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public static LoadScene sceneLoader;

    [SerializeField] private Image progress;
    [SerializeField] private GameObject loadPanel;

    private void Awake()
    {
        sceneLoader = this;
    }

    public void SceneLoad(int sceneIndex)
    {
        loadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!asyncOperation.isDone)
        {
            progress.fillAmount = asyncOperation.progress / 0.9f;
            yield return null;
        }
    }
}
