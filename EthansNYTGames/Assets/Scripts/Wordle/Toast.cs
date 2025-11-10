using System.Collections;
using TMPro;
using UnityEngine;

public class Toast : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeIn = 0.1f;
    [SerializeField] private float displayTime = 1.2f;
    [SerializeField] private float fadeOut = 0.2f;

    public bool isShowing = false;
    private Coroutine routine;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (label != null) label.text = "";
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void ShowToast(string message, float? holdOverride = null)
    {
        if (isShowing) return;
        isShowing = true;
        routine = StartCoroutine(Run(message, holdOverride ?? displayTime));
    }

    private IEnumerator Run(string message, float holdTime)
    {
        isShowing = true;
        if (label != null)
        {
            label.text = message;
        }
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        float t = 0f;
        canvasGroup.alpha = 0f;
        while (t < fadeIn)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeIn);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(holdTime);

        float holdT = 0f;
        while (holdT < holdTime)
        {
            holdT += Time.unscaledDeltaTime;
            yield return null;
        }

        t = 0f;
        while (t < fadeOut)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOut);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        routine = null;
        isShowing = false;
    }
}
