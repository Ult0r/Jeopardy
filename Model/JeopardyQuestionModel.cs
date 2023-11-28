using System.IO;
using System.Linq;
using Jeopardy.Configuration;
using Jeopardy.Validation;

namespace Jeopardy.Model;

public class JeopardyQuestionModel : ModelBase
{
    private readonly string[] _imageFileExtensions = { "bmp", "gif", "ico", "jpg", "png", "wdp", "tiff" };
    private readonly string[] _soundFileExtensions = { "mp3" };
    
    private bool _isPickable;
    
    public JeopardyQuestionModel(QuestionObj question, int defaultPoints)
    {
        question.Question.AssertNotNull(nameof(question.Question));
        question.Answer.AssertNotNull(nameof(question.Answer));

        QuestionText = question.Question;
        Answer = question.Answer;
        PointValue = question.Points ?? defaultPoints;
        SetMedia(question.Media);
        IsPickable = true;
    }

    private void SetMedia(string? media)
    {
        if (media is not null && media.Count(c => c == '.') == 1)
        {
            var fileExtension = media.Split('.')[1];
            var path = Path.GetFullPath($".\\Assets\\{media}");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            
            if (_imageFileExtensions.Contains(fileExtension))
            {
                ImageMedia = path;
            }
            else if (_soundFileExtensions.Contains(fileExtension))
            {
                SoundMedia = path;
            }
            else
            {
                throw new FileFormatException($"Can't process file {media}");
            }
        }
    }

    public string QuestionText { get; }
    public string Answer { get; }
    public int PointValue { get; }
    public string? ImageMedia { get; private set; }
    public string? SoundMedia { get; private set; }

    public bool IsPickable { get => _isPickable; set => SetField(ref _isPickable, value); }

    public void OnQuestionSelected()
    {
        IsPickable = false;
    }
}