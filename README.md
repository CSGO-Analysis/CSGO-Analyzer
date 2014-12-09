Fork of https://github.com/moritzuehling/demoinfo-public/

The goal of this fork is to separate demo parsing and data processing. To achieve this, it will use events triggered by the parser when things happened (e.g. new team parsed, player infos update parsed, round end event parsed, ...)

Architecture
============

DemoParser : CSGO .dem file parser triggering parsing events
DemoParser-Model : IN PROGRESS - contains data models and repos/services to store or computer stats of a game
DemoParser-UI : little WPF application use to see what's possible with this library
