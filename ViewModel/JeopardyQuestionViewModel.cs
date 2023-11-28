using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Jeopardy.Events;
using Jeopardy.Model;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace Jeopardy.ViewModel;

public class JeopardyQuestionViewModel : ViewModelBase
{
    private readonly IEventAggregator _eventAggregator;
    private readonly JeopardyQuestionModel _questionModel;

    public JeopardyQuestionViewModel(IEventAggregator eventAggregator, JeopardyQuestionModel question)
    {
        _eventAggregator = eventAggregator;
        _questionModel = question;
        
        _questionModel.PropertyChanged += QuestionModelOnPropertyChanged; 

        OnQuestionSelectedCommand = new RelayCommand(OnQuestionSelected);
    }

    public int PointValue => _questionModel.PointValue;
    
    public ICommand OnQuestionSelectedCommand { get; }

    public bool IsPickable => _questionModel.IsPickable;

    private void OnQuestionSelected()
    {
        _questionModel.OnQuestionSelected();
        _eventAggregator.GetEvent<QuestionSelectedEvent>().Publish(_questionModel);
    }

    private void QuestionModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_questionModel.IsPickable))
        {
            OnPropertyChanged(nameof(IsPickable));
        }
    }
}