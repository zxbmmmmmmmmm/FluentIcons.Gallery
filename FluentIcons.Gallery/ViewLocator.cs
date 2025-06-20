using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using FluentIcons.Gallery.ViewModels;

namespace FluentIcons.Gallery;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type is not null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
