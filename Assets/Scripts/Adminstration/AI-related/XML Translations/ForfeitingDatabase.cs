using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("ForfeitingCollection")]
public class ForfeitingDatabase  
{
	[XmlArray("Forfeitings"),XmlArrayItem("Forfeiting")]
	public List<Forfeiting> Forfeitings = new List<Forfeiting>();
	
	public static ForfeitingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(ForfeitingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as ForfeitingDatabase;
		}
	}
}
