using System;
using Jeopardy.ViewModel;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace Jeopardy.View;

public partial class MainWindow
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor using ILogger without type parameter will break DI
    public MainWindow(IEventAggregator eventAggregator, ILogger<MainWindow> logger)
    {
        try
        {
            logger.LogDebug("Creating MainWindow...");
            InitializeComponent();
            DataContext = new MainWindowViewModel(eventAggregator, logger);
            logger.LogDebug("MainWindow created");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception during MainWindow setup");
            throw;
        }
    }
}