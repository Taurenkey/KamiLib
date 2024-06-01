﻿using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace KamiLib.Window.SelectionWindows;

public class ItemUICategorySelectionWindow : SelectionWindowBase<ItemUICategory> {
    [PluginService] private ITextureProvider TextureProvider { get; set; } = null!;
    [PluginService] private IDataManager DataManager { get; set; } = null!;

    public ItemUICategorySelectionWindow(DalamudPluginInterface pluginInterface) : base(new Vector2(300.0f, 600.0f), false) {
        pluginInterface.Inject(this);

        SelectionOptions = DataManager
            .GetExcelSheet<ItemUICategory>()!
            .Where(item => !item.Name.ToString().IsNullOrEmpty())
            .OrderBy(item => item.OrderMajor)
            .ThenBy(item => item.OrderMinor)
            .ToList();
    }
    
    protected override bool AllowMultiSelect => true;
    protected override float SelectionHeight => 30.0f * ImGuiHelpers.GlobalScale;
    
    protected override void DrawSelection(ItemUICategory option) {
        if (TextureProvider.GetIcon((uint)option.Icon) is { } texture) {
            ImGui.Image(texture.ImGuiHandle, ImGuiHelpers.ScaledVector2(30.0f, 30.0f));
            ImGui.SameLine();
            ImGui.SetCursorPosY(ImGui.GetCursorPos().Y + 5.0f * ImGuiHelpers.GlobalScale);
            ImGui.Text(option.Name);
        }
    }
    
    protected override bool FilterResults(ItemUICategory option, string filter)
        => option.Name.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase);
}