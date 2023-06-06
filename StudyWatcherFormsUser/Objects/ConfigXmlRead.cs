using System.Xml;

namespace StudyWatcherFormsUser;

public class ConfigXmlRead
{
    public string Ip { get; set; }
    public string Location { get; set; }

    public ConfigXmlRead()
    {
        var xmlFilePath = Path.Combine("Settings", "config.xml");
        var settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        settings.IgnoreWhitespace = true;
        
        using (var reader = XmlReader.Create(xmlFilePath, settings))
        {
            reader.MoveToContent();
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "root")
            {
                reader.Read();
                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "parameter")
                    {
                        var name = reader.GetAttribute("name");
                        var value = reader.GetAttribute("value");
                        if (name == "server" && value != null)
                            Ip = value;
                        if (name == "location" && value != null)
                            Location = value;
                    }
                    reader.Read();
                }
            }
        }
    }
}