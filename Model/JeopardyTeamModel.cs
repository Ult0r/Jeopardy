using Jeopardy.Configuration;
using Jeopardy.Validation;

namespace Jeopardy.Model;

public class JeopardyTeamModel : ModelBase
{
    private int _pointTotal;

    public JeopardyTeamModel(JeopardyConfiguration configuration, Team team)
    {
        team.Name.AssertNotNull(nameof(team.Name));

        WrongAnswerDeducesPoints = configuration.WrongAnswerDeducesPoints ?? JeopardyConfiguration.DefaultWrongAnswerDeducesPoints;
        WrongAnswerHalfPoints = configuration.WrongAnswerHalfPoints ?? JeopardyConfiguration.DefaultWrongAnswerHalfPoints;
        
        Name = team.Name;
        PointTotal = team.StartingPoints ?? 0;
    }

    private bool WrongAnswerDeducesPoints { get; }
    private bool WrongAnswerHalfPoints { get; }


    public string Name { get; }
    public int PointTotal { get => _pointTotal; private set => SetField(ref _pointTotal, value); }

    public void AddPoints(int pointsToAdd) => PointTotal += pointsToAdd;
    public void DeducePoints(int pointsToDeduce)
    {
        var pts = WrongAnswerHalfPoints ? pointsToDeduce / 2 : pointsToDeduce;
        PointTotal -= WrongAnswerDeducesPoints ? pts : 0;
    }
}