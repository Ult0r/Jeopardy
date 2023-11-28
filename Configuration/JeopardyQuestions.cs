using System;
using System.Collections.Generic;

namespace Jeopardy.Configuration;

[Serializable]
public class JeopardyQuestions
{
    public List<Category>? Categories { get; set; }
}

[Serializable]
public class Category
{
    public string? Name { get; set; }
    public List<QuestionObj>? Questions { get; set; }
}

[Serializable]
public class QuestionObj
{
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public int? Points { get; set; }
    public string? Media { get; set; }
}