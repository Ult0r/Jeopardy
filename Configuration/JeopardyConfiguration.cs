using System;
using System.Collections.Generic;

namespace Jeopardy.Configuration;

[Serializable]
public class JeopardyConfiguration
{
    public const int DefaultPointsFactor = 200;
    public const bool DefaultWrongAnswerDeducesPoints = true;
    public const bool DefaultWrongAnswerHalfPoints = false;
    
    public List<Team>? Teams { get; set; }
    public string? EndCardText { get; set; }
    public int? PointsFactor { get; set; }
    public bool? WrongAnswerDeducesPoints { get; set; }
    public bool? WrongAnswerHalfPoints { get; set; }
}

[Serializable]
public class Team
{
    public string? Name { get; set; }
    public int? StartingPoints { get; set; }
}