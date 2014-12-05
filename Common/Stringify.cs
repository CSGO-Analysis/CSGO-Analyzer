using System.Reflection;
using System.Text;

namespace Common
{
	public abstract class Stringify
	{
		public override string ToString()
		{
			PropertyInfo[] propertyInfos = this.GetType().GetProperties();

			var sb = new StringBuilder();

			foreach (var info in propertyInfos)
			{
				sb.AppendLine(info.Name + ": " + info.GetValue(this, null).ToString());
			}

			return sb.ToString();
		}
	}
}
