using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("RaisingTypeCollection")]
public class RaisingTypeDatabase
{
	[XmlArray("RaiseTypes"),XmlArrayItem("RaiseType")]
	public List<RaisingType> RaiseTypes = new List<RaisingType>();
	
	public static RaisingTypeDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(RaisingTypeDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as RaisingTypeDatabase;
		}
	}
}
