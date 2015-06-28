Fork of https://github.com/moritzuehling/demoinfo-public/

The goal of this fork is to separate demo parsing and data processing. To achieve this, it will use events triggered by the parser when things happened (e.g. new team parsed, player infos update parsed, round end event parsed, ...)

Demo parsing
============
CSGO protobuf updated definition : 
Protobuf used to parse .dem.info file : CDataGCCStrike15_v2_MatchInfo

Application Architecture
============
- DemoParser-Core : CSGO .dem file parser triggering parsing events
- DemoParser-Model : contains data models and repos/services to store or compute stats of a game
- DemoParser-UI : little WPF application made to see what's possible with this library

Screenshots
=========================

DemoParser-UI
-
![demoparserui](https://cloud.githubusercontent.com/assets/1845905/5427374/235fb24e-839a-11e4-9877-41b7cfe76214.PNG)
![demoparserui-scoreboard](https://cloud.githubusercontent.com/assets/1845905/5427373/235bb176-839a-11e4-9909-349e996eb036.PNG)
