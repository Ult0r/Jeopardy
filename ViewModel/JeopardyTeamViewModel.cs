using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Jeopardy.Events;
using Jeopardy.Model;
using Jeopardy.Validation;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace Jeopardy.ViewModel;

public class JeopardyTeamViewModel : ViewModelBase
{
    private readonly JeopardyTeamModel _teamModel;
    private bool _displayResultButtons;
    private JeopardyQuestionModel? _selectedQuestion;
    
    public JeopardyTeamViewModel(IEventAggregator eventAggregator, JeopardyTeamModel model)
    {
        _teamModel = model;
        _displayResultButtons = false;
        _selectedQuestion = null;

        eventAggregator.GetEvent<QuestionSelectedEvent>().Subscribe(OnQuestionSelected);
        eventAggregator.GetEvent<QuestionAnsweredEvent>().Subscribe(OnQuestionAnswered);

        OnCorrectAnswerCommand = new RelayCommand(OnCorrectAnswer);
        OnWrongAnswerCommand = new RelayCommand(OnWrongAnswer);
        
        _teamModel.PropertyChanged += TeamModelOnPropertyChanged;
    }

    public string TeamName => _teamModel.Name;
    public int TeamPoints => _teamModel.PointTotal;
    public bool DisplayResultButtons { get => _displayResultButtons; set => SetField(ref _displayResultButtons, value); }
    public ICommand OnCorrectAnswerCommand { get; }
    public ICommand OnWrongAnswerCommand { get; }

    private void TeamModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TeamPoints));
    }
    
    private void OnQuestionSelected(JeopardyQuestionModel question)
    {
        DisplayResultButtons = true;
        _selectedQuestion = question;
    }

    private void OnQuestionAnswered()
    {
        DisplayResultButtons = false;
        _selectedQuestion = null;
    }

    private void OnCorrectAnswer()
    {
        _selectedQuestion.AssertNotNull(nameof(_selectedQuestion));
        _teamModel.AddPoints(_selectedQuestion.PointValue);
    }

    private void OnWrongAnswer()
    {
        _selectedQuestion.AssertNotNull(nameof(_selectedQuestion));
        _teamModel.DeducePoints(_selectedQuestion.PointValue);
    }
}