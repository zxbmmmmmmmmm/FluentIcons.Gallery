using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentIcons.Gallery.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial IReadOnlyList<IconInfo> Icons { get; set; }

    [ObservableProperty]
    public partial IReadOnlyList<IconInfo> DisplayedIcons { get; set; }

    [ObservableProperty]
    public partial string Query { get; set; } = string.Empty;

    [ObservableProperty]
    public partial IconVariant IconVariant { get; set; } = IconVariant.Regular;

    [ObservableProperty]
    public partial IconInfo Selected { get;
        set; }

    [RelayCommand]
    public void SelectIcon(IconInfo icon)
    {
        Selected.IsSelected = false;
        icon.IsSelected = true;
        Selected = icon;
    }
    public MainViewModel()
    {
        DisplayedIcons = Icons = Enum.GetValues<Icon>().Select(
            p => new IconInfo { Name = p.ToString(), Value = p }
            ).ToList();
        Selected = Icons.First();
    }

    partial void OnQueryChanged(string value)
    {
        DisplayedIcons = string.IsNullOrWhiteSpace(value)
            ? Icons
            : Icons.Where(icon => icon.Name.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    private static bool HasAttribute<TAttribute>(Type enumType, string enumValueName)
        where TAttribute : Attribute
    {
        var memberInfo = enumType.GetMember(enumValueName);
        if (memberInfo.Length == 0) return false;

        var field = memberInfo[0] as FieldInfo;
        return field.IsDefined(typeof(TAttribute), false);
    }
}
public partial class IconInfo : ObservableObject
{
    public required string Name { get; init; }
    public required Icon Value { get; init; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }

}

