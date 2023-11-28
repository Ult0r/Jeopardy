using Jeopardy.Model;
using Prism.Events;

namespace Jeopardy.Events;

public class QuestionSelectedEvent : PubSubEvent<JeopardyQuestionModel>
{
}