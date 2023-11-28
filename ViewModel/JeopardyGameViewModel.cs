using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Jeopardy.Configuration;
using Jeopardy.Events;
using Jeopardy.Helper;
using Jeopardy.Model;
using Jeopardy.Validation;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace Jeopardy.ViewModel;

public class JeopardyGameViewModel : ViewModelBase
{
    private readonly IEventAggregator _eventAggregator;
    private JeopardyQuestionModel? _selectedQuestion;
    private bool _showBoard;
    private bool _showQuestion;
    private bool _showAnswer;
    private bool _showEndCard;
    private bool _hasGoNext;

    private readonly MediaPlayer _mediaPlayer;
    private bool _isPlayingMedia;
    
    private bool _isImageRevealed;
    
    public JeopardyGameViewModel(IEventAggregator eventAggregator, JeopardyConfiguration configuration, JeopardyQuestions questions)
    {
        _eventAggregator = eventAggregator;

        GameModel = new JeopardyGameModel(configuration, questions);
        foreach (var team in GameModel.Teams)
        {
            Teams.Add(new JeopardyTeamViewModel(_eventAggregator, team));
        }

        foreach (var category in GameModel.Categories)
        {
            Categories.Add(new JeopardyCategoryViewModel(_eventAggregator, category));
        }

        EndCardText = configuration.EndCardText ?? "Thanks for playing!";
        _selectedQuestion = null;
        _showBoard = true;
        _showQuestion = false;
        _showAnswer = false;
        _showEndCard = false;
        _hasGoNext = _showQuestion || _showAnswer;
        
        _isPlayingMedia = false;
        _mediaPlayer = new MediaPlayer();

        PropertyChanged += OnPropertyChanged;
        OnGoNextCommand = new RelayCommand(OnGoNext);
        OnPlayMediaCommand = new RelayCommand(OnPlayMedia);
        OnRevealImageCommand = new RelayCommand(OnRevealImage);
        
        _eventAggregator.GetEvent<QuestionSelectedEvent>().Subscribe(OnQuestionSelected);
        _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
    }

    private JeopardyGameModel GameModel { get; }

    public ObservableCollectionWithItemUpdates<JeopardyCategoryViewModel> Categories { get; } = new();
    public ObservableCollectionWithItemUpdates<JeopardyTeamViewModel> Teams { get; } = new();
    
    public string EndCardText { get; }
    public bool ShowBoard { get => _showBoard; private set => SetField(ref _showBoard, value); }
    public bool ShowQuestion { get => _showQuestion; private set => SetField(ref _showQuestion, value); }
    public bool ShowAnswer { get => _showAnswer; private set => SetField(ref _showAnswer, value); }
    public bool ShowEndCard { get => _showEndCard; private set => SetField(ref _showEndCard, value); }
    public bool HasGoNext { get => _hasGoNext; private set => SetField(ref _hasGoNext, value); }
    public JeopardyQuestionModel SelectedQuestion { get => _selectedQuestion!; private set => SetField(ref _selectedQuestion, value); }
    public bool HasImageMedia => _selectedQuestion?.ImageMedia is not null;
    public bool HasSoundMedia => _selectedQuestion?.SoundMedia is not null;
    public bool IsImageRevealed { get => _isImageRevealed; private set => SetField(ref _isImageRevealed, value); }

    public ICommand OnGoNextCommand { get; }
    public ICommand OnPlayMediaCommand { get; }
    public ICommand OnRevealImageCommand { get; }
    
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ShowQuestion) or nameof(ShowAnswer))
        {
            HasGoNext = ShowQuestion || ShowAnswer;
        }
    }
    
    private void OnQuestionSelected(JeopardyQuestionModel question)
    {
        ShowBoard = false;
        ShowQuestion = true;
        ShowAnswer = false;
        SelectedQuestion = question;
        OnPropertyChanged(nameof(HasImageMedia));
        OnPropertyChanged(nameof(HasSoundMedia));
        IsImageRevealed = false;
    }

    private void OnGoNext()
    {
        if (ShowQuestion)
        {
            ShowBoard = false;
            ShowQuestion = false;
            ShowAnswer = true;
            _mediaPlayer.Stop();
        }
        else if (ShowAnswer)
        {
            ShowBoard = true;
            ShowQuestion = false;
            ShowAnswer = false;
            
            _eventAggregator.GetEvent<QuestionAnsweredEvent>().Publish();

            var allAnswered = GameModel.Categories.SelectMany(c => c.Questions).Aggregate(true, (current, q) => current && !q.IsPickable);
            if (allAnswered)
            {
                ShowBoard = false;
                ShowEndCard = true;
            }
        }
    }

    private void OnPlayMedia()
    {
        if (!_isPlayingMedia)
        {
            _selectedQuestion.AssertNotNull(nameof(_selectedQuestion));
            _selectedQuestion.SoundMedia.AssertNotNull(nameof(_selectedQuestion.SoundMedia));
            _mediaPlayer.Open(new Uri(_selectedQuestion.SoundMedia));
            _mediaPlayer.Play();
        }
        else
        {
            _mediaPlayer.Stop();
        }
        _isPlayingMedia = !_isPlayingMedia;
    }

    private void MediaPlayerOnMediaEnded(object? sender, EventArgs e)
    {
        _isPlayingMedia = false;
        _mediaPlayer.Stop();
    }

    private void OnRevealImage()
    {
        IsImageRevealed = true;
    }
}