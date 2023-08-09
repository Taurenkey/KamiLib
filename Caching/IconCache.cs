﻿using System;
using System.Collections.Generic;
using Dalamud.Logging;
using Dalamud.Utility;
using ImGuiScene;

namespace KamiLib.Caching;

public class IconCache : IDisposable
{
    private readonly Dictionary<uint, TextureWrap?> iconTextures = new();

    private const string IconFilePath = "ui/icon/{0:D3}000/{1:D6}_hr1.tex";
    
    private static IconCache? _instance;
    public static IconCache Instance => _instance ??= new IconCache();

    private static Func<string, TextureWrap?>? _alternateGetTextureFunc;
    
    public static void Cleanup()
    {
        _instance?.Dispose();
    }
    
    public void Dispose() 
    {
        foreach (var texture in iconTextures.Values) 
        {
            texture?.Dispose();
        }

        iconTextures.Clear();
    }
        
    private void LoadIconTexture(uint iconId) 
    {
        try
        {
            var path = IconFilePath.Format(iconId / 1000, iconId);
            var tex = _alternateGetTextureFunc is null ? Service.TextureProvider.GetIcon(iconId) : _alternateGetTextureFunc.Invoke(path);

            if (tex is not null && tex.ImGuiHandle != nint.Zero) 
            {
                iconTextures[iconId] = tex;
            } 
            else 
            {
                tex?.Dispose();
            }
        } 
        catch (Exception ex) 
        {
            PluginLog.LogError($"Failed loading texture for icon {iconId} - {ex.Message}");
        }
    }
    
    public TextureWrap? GetIcon(uint iconId) 
    {
        if (iconTextures.TryGetValue(iconId, out var value)) return value;

        iconTextures.Add(iconId, null);
        LoadIconTexture(iconId);

        return iconTextures[iconId];
    }

    public static void SetAlternativeGetTextureFunc(Func<string, TextureWrap?> getTexture) => _alternateGetTextureFunc = getTexture;
}