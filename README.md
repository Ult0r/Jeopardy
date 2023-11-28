# Jeopardy
Custom Jeopardy implementation

Download latest release, update config.yml/questions.yml and add assets to the 'Assets' directory.

General Usage:
- Start Jeopardy.exe (Windows Defender might complain about missing publisher and you might have to install .NET runtime)
- Click categories to reveal category names at the beginning of the game
- Click on questions to open them up, click again to reveal answer
- If question has media: for images, click '?' to reveal. for sound, click volume icon to start/stop playback
- Click the corresponding buttons on team scores to update them for correct/wrong answers (only available in question/answer view)

If application doesn't start, check Jeopardy.log file. Most likely cause is a missing media file.

## questions.yml

categories: [Categories] (ordered)<br>

### Categories

name: "Category name"<br/>
questions: [Questions] (ordered, 5 per category)<br>

### Questions

question: "Question?"<br>
answer: "Answer"<br>
points: 123 (optional, defaults otherwise to normal point scale)<br>
media: "image.jpg" (optional, supported formats: bmp, ico, jpg, png, mp3)<br>

## config.yml

teams: [Team] (ordered)<br>
endCardText: "This text displays at the end of the game" (optional, defaults to "Thanks for playing!")<br>
pointsFactor: 200 (default factor for point value scaling, value X means that first questions is worth X points, second question 2X, etc.)<br>
wrongAnswerDeducesPoints: [true|false] (when enabled: on wrong answer deduce point value of question from team)<br>
wrongAnswerHalfPoints: [true|false] (when enabled and wrongAnswerDeducesPoints enabled: on wrong answer deduce only half of the point value)<br>

### Teams

name: "Team name"<br>
startingPoints: 123 (optional, to give teams a (dis-)advantage)<br>
