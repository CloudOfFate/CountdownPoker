using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("PurchasingCollection")]
public class PurchasingDatabase
{
	[XmlArray("Purchasings"),XmlArrayItem("Purchasing")]
	public List<Purchasing> Purchasings = new List<Purchasing>();
	
	public static PurchasingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(PurchasingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as PurchasingDatabase;
		}
	}
}
