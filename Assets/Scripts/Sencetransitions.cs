using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Sencetransitions : MonoBehaviour
{
    public Image fadePanel; // Assign in Inspector
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadGameScene()
    {
        Debug.Log("sence");
        StartCoroutine(LoadAsync("SampleScene"));
    }

    IEnumerator FadeIn()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            fadePanel.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
            //yield return new WaitForSeconds(fadeDuration);
           
            yield return null;
        }
            this.gameObject.SetActive(false);
    }

    public void loadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
