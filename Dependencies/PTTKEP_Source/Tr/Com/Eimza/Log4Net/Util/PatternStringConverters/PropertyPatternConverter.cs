using System.IO;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal sealed class PropertyPatternConverter : PatternConverter
	{
		protected override void Convert(TextWriter writer, object state)
		{
			CompositeProperties compositeProperties = new CompositeProperties();
			PropertiesDictionary properties = LogicalThreadContext.Properties.GetProperties(false);
			if (properties != null)
			{
				compositeProperties.Add(properties);
			}
			PropertiesDictionary properties2 = ThreadContext.Properties.GetProperties(false);
			if (properties2 != null)
			{
				compositeProperties.Add(properties2);
			}
			compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
			if (Option != null)
			{
				PatternConverter.WriteObject(writer, null, compositeProperties[Option]);
			}
			else
			{
				PatternConverter.WriteDictionary(writer, null, compositeProperties.Flatten());
			}
		}
	}
}
