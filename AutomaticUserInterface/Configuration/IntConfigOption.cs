﻿using System;
using System.Reflection;
using ImGuiNET;

namespace KamiLib.AutomaticUserInterface;

public class IntConfigOption : RightLabeledTabledDrawableAttribute
{
    public int MinValue { get; init; }
    public int MaxValue { get; init; }

    public IntConfigOption(string labelLocKey, int minValue, int maxValue) : base(labelLocKey)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }
    
    protected override void DrawLeftColumn(object obj, FieldInfo field, Action? saveAction = null)
    {
        var intValue = GetValue<int>(obj, field);
        
        if (ImGui.SliderInt($"##{field.Name}", ref intValue, MinValue, MaxValue))
        {
            SetValue(obj, field, intValue);
            saveAction?.Invoke();
        }
    }
}