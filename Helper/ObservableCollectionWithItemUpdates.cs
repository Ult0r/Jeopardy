using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Jeopardy.Helper;

public sealed class ObservableCollectionWithItemUpdates<T> : ObservableCollection<T>
    where T : INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollectionWithItemUpdates()
    {
        CollectionChanged += OnCollectionChanged;
    }

    private readonly List<T> _cache = new();

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (T item in e.OldItems)
            {
                item.PropertyChanged -= OnPropertyChanged;
                _cache.Remove(item);
            }
        }

        if (e.NewItems is not null)
        {
            foreach (T item in e.NewItems)
            {
                item.PropertyChanged += OnPropertyChanged;
                _cache.Add(item);
            }
        }

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            foreach (var item in _cache)
            {
                item.PropertyChanged -= OnPropertyChanged;
            }
        }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(sender, e);
    }
}