using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFade : MonoBehaviour {

    public string loadLevel;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Fade(0, 1, 4));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Fade(1, 0, 4));
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(loadLevel);
    }

    IEnumerator Fade(float start, float end, float rate)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * rate;
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(start, end, t));
            yield return null;
        }
    }
}

