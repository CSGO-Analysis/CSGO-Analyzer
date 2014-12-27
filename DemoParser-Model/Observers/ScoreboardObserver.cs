using DemoParser_Core;
using DemoParser_Core.Events;
using DemoParser_Model.Models;
using DemoParser_Model.Services;

namespace DemoParser_Model.Observers
{
	public class ScoreboardObserver : GameObserver
	{
		private IScoreBoardService scoreBoardService = new ScoreBoardService();

		public ScoreBoard scoreBoard = new ScoreBoard();

		public ScoreboardObserver(DemoParser demoParser) : base(demoParser)
		{

		}

		protected override void eventsManager_MatchEnded(object sender, MatchEndedEventArgs e)
		{
			this.scoreBoard = scoreBoardService.GetScoreBoard(this.game);
		}
	}
}
