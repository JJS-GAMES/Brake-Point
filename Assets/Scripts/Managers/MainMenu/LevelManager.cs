using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float Progress { get; private set; }
    public AsyncOperation LoadSceneAsyncOperation { get; private set; }

    public void Load(int index)
    {
        StartCoroutine(LoadAsync(index));
    }
    private IEnumerator LoadAsync(int index)
    {
        LoadSceneAsyncOperation = SceneManager.LoadSceneAsync(index);
        LoadSceneAsyncOperation.allowSceneActivation = false;

        while (!LoadSceneAsyncOperation.isDone)
        {
            Progress = Mathf.Clamp01(LoadSceneAsyncOperation.progress / 0.9f);

            if (LoadSceneAsyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                LoadSceneAsyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentScene.buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
