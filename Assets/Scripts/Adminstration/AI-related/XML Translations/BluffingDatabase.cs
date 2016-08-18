using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("WorthBluffing")]
public class BluffingDatabase
{
	[XmlArray("Bluffings"),XmlArrayItem("Bluffing")]
	public List<Bluffing> Bluffings = new List<Bluffing>();

	public static BluffingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(BluffingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as BluffingDatabase;
		}
	}
}
