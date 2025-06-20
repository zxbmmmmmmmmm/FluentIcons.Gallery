using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;

namespace FluentIcons.Gallery.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static IReadOnlyList<IconInfo> Icons { get; } = Enum.GetValues<Symbol>()
        .Select(p => new IconInfo
        {
            Name = p.ToString(),
            Value = p
        })
        .ToArray();

    [ObservableProperty]
    public partial IReadOnlyList<IconInfo> DisplayedIcons { get; set; } = Icons;

    [ObservableProperty]
    public partial string Query { get; set; } = string.Empty;

    [ObservableProperty]
    public partial IconVariant IconVariant { get; set; } = IconVariant.Regular;

    [ObservableProperty]
    public partial IconInfo Selected { get; set; } = Icons[0].Let(t => t.IsSelected = true);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ItemMinSize))]
    [NotifyPropertyChangedFor(nameof(ItemPadding))]
    [NotifyPropertyChangedFor(nameof(FontSize))]
    public partial bool IsCompact { get; set; }

    public int ItemMinSize => IsCompact ? 30 : 80;

    public int FontSize => IsCompact ? 18 : 40;

    public Thickness ItemPadding => new(IsCompact ? 0 : 4);

    [RelayCommand]
    public void SelectIcon(IconInfo icon)
    {
        Selected.IsSelected = false;
        icon.IsSelected = true;
        Selected = icon;
    }

    async partial void OnQueryChanged(string value)
    {
        await Task.Yield();
        if (string.IsNullOrWhiteSpace(value))
        {
            DisplayedIcons = Icons;
            return;
        }

        var view = new ConcurrentBag<IconInfo>();
        Parallel.ForEach(Icons, kvp =>
        {
            if (kvp.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
                view.Add(kvp);
        });
        DisplayedIcons = view.ToImmutableSortedSet();
    }
}
