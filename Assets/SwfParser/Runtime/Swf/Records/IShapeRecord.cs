using System.Xml;

namespace SwfParserRuntime {

    public interface IShapeRecord {

        XmlElement ToXml(XmlDocument doc);
    }
}