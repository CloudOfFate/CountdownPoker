using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("CallingCollection")]
public class CallingDatabase 
{
	[XmlArray("Callings"),XmlArrayItem("Calling")]
	public List<Calling> Callings = new List<Calling>();

	public static CallingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(CallingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as CallingDatabase;
		}
	}
}
