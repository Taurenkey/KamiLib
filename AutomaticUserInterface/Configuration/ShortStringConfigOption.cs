﻿using System;
using System.Reflection;
using ImGuiNET;

namespace KamiLib.AutomaticUserInterface;

public class ShortStringConfigOption : TabledDrawableAttribute
{
    private readonly bool useAxisFont;
    
    public ShortStringConfigOption(string labelLocKey, bool axisFont = false) : base(labelLocKey)
    {
        useAxisFont = axisFont;
    }
    
    protected override void DrawLeftColumn(object obj, FieldInfo field, Action? saveAction = null)
    {
        var stringValue = GetValue<string>(obj, field);

        if (useAxisFont) ImGui.PushFont(KamiCommon.FontManager.Axis12.ImFont);
        if (ImGui.InputTextWithHint($"##{field.Name}", Label, ref stringValue, 2048))
        {
            field.SetValue(obj, stringValue);
        }
        if (useAxisFont) ImGui.PopFont();

        if (ImGui.IsItemDeactivatedAfterEdit())
        {
            saveAction?.Invoke();
        }
    }
    
    protected override void DrawRightColumn(object obj, FieldInfo field, Action? saveAction = null)
    {
        // This side empty intentionally
    }
}