using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("WorthRaising")]
public class RaisingDatabase
{
	[XmlArray("Raisings"),XmlArrayItem("Raising")]
	public List<Raising> Raisings = new List<Raising>();

	public static RaisingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(RaisingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as RaisingDatabase;
		}
	}
}
