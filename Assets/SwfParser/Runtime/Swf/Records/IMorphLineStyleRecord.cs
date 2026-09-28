using System.Xml;

namespace SwfParserRuntime {

    public interface IMorphLineStyleRecord {

        XmlElement ToXml(XmlDocument doc);
    }
}