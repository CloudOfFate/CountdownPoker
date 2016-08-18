using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("HandValuesCollection")]
public class HandValuesDatabase 
{
	[XmlArray("HandValues"),XmlArrayItem("HandValue")]
	public List<HandValue> HandValues = new List<HandValue>();
	
	public static HandValuesDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(HandValuesDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as HandValuesDatabase;
		}
	}
}
