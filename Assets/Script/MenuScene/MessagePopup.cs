using System.Collections;
using TMPro;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] float fadeTime = 0.3f;
    [SerializeField] float displayTime = 1.5f;

    Coroutine currentCoroutine;

    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            HideImmediate();
        }
    }

    public void Show(string message)
    {
        gameObject.SetActive(true);
        messageText.text = message;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(displayTime);
        yield return Fade(1f, 0f);

        gameObject.SetActive(false);
        currentCoroutine = null;
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        canvasGroup.alpha = from;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
    public void HideImmediate()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        StartCoroutine(HideRoutine());
    }

    IEnumerator HideRoutine()
    {
        yield return Fade(canvasGroup.alpha, 0f);
        gameObject.SetActive(false);
    }
}
