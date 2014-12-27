using DemoParser_Core;

namespace DemoParser_Model.Observers
{
	public abstract class Observer
	{
		protected DemoParser parser;

		public Observer(DemoParser demoParser)
		{
			this.parser = demoParser;
		}
	}
}
