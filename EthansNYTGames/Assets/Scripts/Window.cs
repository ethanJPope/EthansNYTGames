using UnityEngine;

public class Window : MonoBehaviour, IWindow
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
