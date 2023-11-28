using System.IO;
using Jeopardy.Configuration;
using Microsoft.Extensions.Logging;
using Prism.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Jeopardy.ViewModel;

public class MainWindowViewModel
{
    public JeopardyGameViewModel GameState { get; }

    public MainWindowViewModel(IEventAggregator eventAggregator, ILogger logger)
    {
        logger.LogDebug("Initializing MainWindowViewModel...");
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

        logger.LogDebug("Loading config.yml...");
        var configFile = new StreamReader("config.yml");
        var config = deserializer.Deserialize<JeopardyConfiguration>(configFile);
        
        logger.LogDebug("Loading questions.yml...");
        var questionsFile = new StreamReader("questions.yml");
        var questions = deserializer.Deserialize<JeopardyQuestions>(questionsFile);
        
        GameState = new JeopardyGameViewModel(eventAggregator, config, questions);
    }
}