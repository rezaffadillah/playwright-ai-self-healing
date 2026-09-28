using System.Diagnostics;
using Demo.Tests.Config;

namespace Demo.Tests.Native;

public static class ApkInfo
{
    public static (string Package, string Activity) Read()
    {
        var apk =
            Path.GetFullPath(
                TestConfigNative.AppPath);

        var sdk =
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);

        var aapt =
            Path.Combine(
                sdk,
                "Android",
                "Sdk",
                "build-tools",
                "37.0.0",
                "aapt.exe");

        if (!File.Exists(aapt))
            throw new Exception("aapt.exe not found");

        var process = new Process();

        process.StartInfo.FileName = aapt;

        process.StartInfo.Arguments =
            $"dump badging \"{apk}\"";

        process.StartInfo.RedirectStandardOutput = true;

        process.StartInfo.UseShellExecute = false;

        process.Start();

        var text =
            process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        var package =
            "";

        var activity =
            "";

        foreach (var line in text.Split('\n'))
        {
            if (line.StartsWith("package:"))
            {
                package =
                    line.Split("name='")[1]
                        .Split("'")[0];
            }

            if (line.StartsWith("launchable-activity"))
            {
                activity =
                    line.Split("name='")[1]
                        .Split("'")[0];
            }
        }

        return (package, activity);
    }
}