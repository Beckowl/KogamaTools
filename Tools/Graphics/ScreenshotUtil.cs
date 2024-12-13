using static System.Environment;
using KogamaTools.Behaviours;
using UnityEngine;
using KogamaTools.Helpers;

namespace KogamaTools.Tools.Graphics;
internal static class ScreenshotUtil
{
    internal static float SuperSize = 2f;
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
        MVGameControllerBase.MainCameraManager.mainCamera.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        Camera.main.Render();

        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        MVGameControllerBase.MainCameraManager.mainCamera.targetTexture = null;
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
