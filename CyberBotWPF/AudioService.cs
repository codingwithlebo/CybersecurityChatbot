using System;
using System.IO;
using System.Threading.Tasks;
namespace CyberBotWPF.Services;

/// <summary>Plays the WAV voice greeting on startup.</summary>
public static class AudioService
{
    private static readonly string WavPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

    public static void PlayGreeting()
    {
        if (!OperatingSystem.IsWindows() || !File.Exists(WavPath)) return;
        PlayWav(WavPath);
    }

#pragma warning disable CA1416
    private static void PlayWav(string path)
    {
        try
        {
            // Fire-and-forget so UI is not blocked
            Task.Run(() =>
            {
                using var player = new System.Media.SoundPlayer(path);
                player.PlaySync();
            });
        }
        catch { /* Silently ignore audio errors */ }
    }
#pragma warning restore CA1416
}