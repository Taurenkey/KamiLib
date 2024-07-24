using System;
using Dalamud.Plugin;
using KamiLib.System;
using KamiLib.UserInterface;

namespace KamiLib;

public static class KamiCommon
{
    public static string PluginName { get; private set; } = string.Empty;
    public static WindowManager WindowManager { get; private set; } = null!;
    public static LocalizationWrapper? Localization { get; private set; }

    public static void Initialize(IDalamudPluginInterface pluginInterface, string pluginName)
    {
        pluginInterface.Create<Service>();

        PluginName = pluginName;

        LocalizationManager.Instance.Initialize();

        WindowManager = new WindowManager();
    }

    public static void RegisterLocalizationHandler(Func<string, string?> handler)
    {
        Localization = new LocalizationWrapper
        {
            GetTranslatedString = handler
        };
    }

    public static void Dispose()
    {
        CommandController.UnregisterMainCommands();
        DebugWindow.Cleanup();
        WindowManager.Dispose();
        LocalizationManager.Cleanup();
    }
}