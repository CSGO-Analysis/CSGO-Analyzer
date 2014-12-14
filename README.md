Fork of https://github.com/moritzuehling/demoinfo-public/

The goal of this fork is to separate demo parsing and data processing. To achieve this, it will use events triggered by the parser when things happened (e.g. new team parsed, player infos update parsed, round end event parsed, ...)

Architecture
============
- DemoParser : CSGO .dem file parser triggering parsing events
- DemoParser-Model : IN PROGRESS - contains data models and repos/services to store or computer stats of a game
- DemoParser-UI : little WPF application use to see what's possible with this library

Screenshots
=========================

DemoParser-UI
-
![demoparserui](https://cloud.githubusercontent.com/assets/1845905/5427374/235fb24e-839a-11e4-9877-41b7cfe76214.PNG)
![demoparserui-scoreboard](https://cloud.githubusercontent.com/assets/1845905/5427373/235bb176-839a-11e4-9909-349e996eb036.PNG)
