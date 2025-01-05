using KogamaTools.Behaviours;
using KogamaTools.Config;
using KogamaTools.Helpers;
using UnityEngine;
using static System.Environment;

namespace KogamaTools.Tools.Graphics;

[Section("Graphics")]
internal static class ScreenshotUtil
{
    [Bind] internal static float SuperSize = 2f;

    [InvokeOnInit]
    internal static void SubscribeHotkeys()
    {
        HotkeySubscriber.Subscribe(KeyCode.F2, CaptureScreenshot);
    }

    // Application.TakeScreenshot is stripped??
    internal static void CaptureScreenshot()
    {
        int width = (int)(Screen.width * SuperSize);
        int height = (int)(Screen.height * SuperSize);

        Texture2D screenshot = RenderScreenshot(width, height);

        string fileName = GetScreenshotPath();
        byte[] data = screenshot.EncodeToPNG();

        WriteDataToDisk(fileName, data);
    }

    private static Texture2D RenderScreenshot(int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        MVGameControllerBase.MainCameraManager.MainCamera.targetTexture = rt;
        MVGameControllerBase.MainCameraManager.SecondaryCamera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        MVGameControllerBase.MainCameraManager.MainCamera.Render();
        MVGameControllerBase.MainCameraManager.SecondaryCamera.Render();

        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        MVGameControllerBase.MainCameraManager.MainCamera.targetTexture = null;
        MVGameControllerBase.MainCameraManager.SecondaryCamera.targetTexture = null;

        RenderTexture.active = null;
        UnityEngine.Object.Destroy(rt);

        return screenshot;
    }

    private static string GetScreenshotPath()
    {
        string screenshotsFolder = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), KogamaTools.ModName, "Screenshots");

        if (!Directory.Exists(screenshotsFolder))
        {
            Directory.CreateDirectory(screenshotsFolder);
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return Path.Combine(screenshotsFolder, $"{timestamp}.png");
    }

    private static void WriteDataToDisk(string fileName, byte[] data)
    {
        try
        {
            File.WriteAllBytes(fileName, data);
            NotificationHelper.NotifySuccess($"Screenshot saved to {fileName}.");
        }
        catch (Exception ex)
        {
            NotificationHelper.NotifyError($"Failed to save screenshot: {ex.ToString()}");
        }
    }
}
