using System.Xml;

namespace SwfParserRuntime {
    /// <summary>
    /// 子类：
    /// <see cref="ColorMapDataRecord"/>
    /// <see cref="BitmapDataRecord"/>
    /// </summary>
    public interface IMapData {

        XmlElement ToXml(XmlDocument doc);

    }
}