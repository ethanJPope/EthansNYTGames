using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;

    [SerializeField] private List<Window> windows;
    [SerializeField] private Window startingWindow;
    private Window currentWindow;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentWindow = startingWindow;
    }

    public void OpenWindow(Window window)
    {
        if (currentWindow != window)
        {
            currentWindow.Hide();
            currentWindow = window;
            currentWindow.Show();
        }
    }
}
