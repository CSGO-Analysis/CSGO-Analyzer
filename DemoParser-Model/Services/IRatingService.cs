using DemoParser_Model.Models;

namespace DemoParser_Model.Services
{
	public interface IRatingService
	{
		double ComputeRating(Game game, Player player);

		PlayerRatingData ComputeRatingData(Game game, Player player);
	}
}
