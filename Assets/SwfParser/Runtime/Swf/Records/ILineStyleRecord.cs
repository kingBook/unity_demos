using System.Xml;

namespace SwfParserRuntime {

    public interface ILineStyleRecord {

        XmlElement ToXml(XmlDocument doc);
    }
}