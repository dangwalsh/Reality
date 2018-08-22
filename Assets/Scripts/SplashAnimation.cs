using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashAnimation : MonoBehaviour
{
    public string loadLevel;
    public Image splash;
    public float delayTime = 1.0f;
    public float fadeInTime = 2.5f;
    public float fadOutTime = 2.5f;

    void Awake()
    {
        splash.canvasRenderer.SetAlpha(0);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
        FadeIn();
        yield return new WaitForSeconds(fadeInTime);
        FadeOut();
        yield return new WaitForSeconds(fadOutTime);
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(loadLevel);
    }

    private void FadeIn()
    {
        splash.CrossFadeAlpha(1.0f, fadeInTime, false);
    }

    private void FadeOut()
    {
        splash.CrossFadeAlpha(0.0f, fadOutTime, false);
    }
}
