using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("BiddingCollection")]
public class BiddingDatabase 
{
	[XmlArray("Biddings"),XmlArrayItem("Bidding")]
	public List<Bidding> Biddings = new List<Bidding>();
	
	public static BiddingDatabase Load(string _path)
	{
		var serializer = new XmlSerializer(typeof(BiddingDatabase));
		using(var stream = new FileStream(_path,FileMode.Open))
		{
			return serializer.Deserialize(stream) as BiddingDatabase;
		}
	}
	
}
