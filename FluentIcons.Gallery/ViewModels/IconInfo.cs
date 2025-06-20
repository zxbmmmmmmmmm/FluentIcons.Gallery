using System;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentIcons.Common;

namespace FluentIcons.Gallery.ViewModels;

public partial class IconInfo : ViewModelBase, IComparable<IconInfo>
{
    public required string Name { get; init; }

    public required Symbol Value { get; init; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    /// <inheritdoc />
    public int CompareTo(IconInfo? other)
    {
        return ReferenceEquals(this, other)
            ? 0
            : other is null 
                ? 1 
                : Value.CompareTo(other.Value);
    }
}
