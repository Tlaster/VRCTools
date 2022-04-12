using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using VRChatCreatorTools.UI.Model;

namespace VRChatCreatorTools.UI.Controls;

internal class PackageListView : TemplatedControl
{
    public static readonly DirectProperty<PackageListView, IEnumerable<UiPackageModel>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<PackageListView, IEnumerable<UiPackageModel>>(
            nameof(Items),
            o => o.Items,
            (o, v) => o.Items = v);

    private IEnumerable<UiPackageModel> _items = new AvaloniaList<UiPackageModel>();

    public IEnumerable<UiPackageModel> Items
    {
        get => _items;
        set => SetAndRaise(ItemsProperty, ref _items, value);
    }
    
    public static readonly DirectProperty<PackageListView, UiPackageModel?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<PackageListView, UiPackageModel?>(
            nameof(SelectedItem),
            o => o.SelectedItem,
            (o, v) => o.SelectedItem = v);

    private UiPackageModel? _selectedItem;

    public UiPackageModel? SelectedItem
    {
        get => _selectedItem;
        set => SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
    }
}