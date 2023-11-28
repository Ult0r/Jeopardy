using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Jeopardy.Helper;
using Jeopardy.Model;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace Jeopardy.ViewModel;

public class JeopardyCategoryViewModel : ViewModelBase
{
    private readonly JeopardyCategoryModel _categoryModel;
    
    public JeopardyCategoryViewModel(IEventAggregator eventAggregator, JeopardyCategoryModel model)
    {
        _categoryModel = model;

        foreach (var question in _categoryModel.Questions)
        {
            var questionVm = new JeopardyQuestionViewModel(eventAggregator, question);
            Questions.Add(questionVm);
        }
        
        _categoryModel.PropertyChanged += CategoryModelOnPropertyChanged;
        RevealNameCommand = new RelayCommand(OnRevealName);
    }

    public string CategoryName => _categoryModel.Name;
    public bool IsNameRevealed => _categoryModel.IsNameRevealed;
    public ICommand RevealNameCommand { get; }
    
    public ObservableCollectionWithItemUpdates<JeopardyQuestionViewModel> Questions { get; } = new();

    private void CategoryModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(IsNameRevealed));
    }

    private void OnRevealName() => _categoryModel.RevealName();
}