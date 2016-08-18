using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("FoldingCollection")]
public class FoldingDatabase
{
	[XmlArray("Foldings"),XmlArrayItem("Folding")]
	public List<Folding> Foldings = new List<Folding>();
	
	public static FoldingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(FoldingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as FoldingDatabase;
		}
	}
}
