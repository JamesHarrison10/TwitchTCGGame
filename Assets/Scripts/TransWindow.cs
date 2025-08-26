using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    // Window style constants
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_LAYERED = 0x00080000;
    private const uint LWA_COLORKEY = 0x00000001;

    private IntPtr hWnd;

    private void Start()
    {
#if !UNITY_EDITOR
        SetupTransparentWindow();
#endif
        // Ensure game runs when not focused
        Application.runInBackground = true;
    }

    private void SetupTransparentWindow()
    {
        hWnd = GetActiveWindow();

        // Get current window style
        int currentStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

        // Add the layered window style
        SetWindowLong(hWnd, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED);

        // Set black as the transparency color key
        // Any black pixels in your game will appear transparent in OBS
        SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
    }

    private void Update()
    {
        // Optional: Press T to toggle transparency (for testing)
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTransparency();
        }
    }

    private void ToggleTransparency()
    {
#if !UNITY_EDITOR
        int currentStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        
        if ((currentStyle & WS_EX_LAYERED) == WS_EX_LAYERED)
        {
            // Remove transparency
            SetWindowLong(hWnd, GWL_EXSTYLE, currentStyle & ~WS_EX_LAYERED);
        }
        else
        {
            // Add transparency
            SetWindowLong(hWnd, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED);
            SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
        }
#endif
    }

    private void OnApplicationQuit()
    {
        // Clean up: Remove transparency when quitting
#if !UNITY_EDITOR
        int currentStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, currentStyle & ~WS_EX_LAYERED);
#endif
    }
}