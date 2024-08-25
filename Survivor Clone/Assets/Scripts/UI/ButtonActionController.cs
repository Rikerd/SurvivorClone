using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonActionController : MonoBehaviour {
    public TransitionController transition;

    public Texture2D swirlTexture;
    public Texture2D sideTexture;
    public Texture2D topDownTexture;
    public Texture2D angleTexture;

    private GameObject eventSystem;

    void Awake()
    {
        eventSystem = GameObject.Find("EventSystem");
    }

    private void Start()
    {
        eventSystem.SetActive(true);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        eventSystem.SetActive(false);
        StartCoroutine(FadeLoadSceneByIndexeWait(sceneIndex));
    }

    IEnumerator FadeLoadSceneByIndexeWait(int sceneIndex)
    {
        transition.FadeToBlack(swirlTexture);

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1;

        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
        eventSystem.SetActive(false);
        StartCoroutine(FadeQuitWait());
    }

    IEnumerator FadeQuitWait()
    {
        transition.FadeToBlack(topDownTexture);

        yield return new WaitForSecondsRealtime(2f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
