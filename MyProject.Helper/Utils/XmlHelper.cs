using System.Xml.Serialization;

namespace MyProject.Helper.Utils
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(List<T> data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }
        public static List<T> DeserializeFromXml<T>(string xml, string rootTag, string itemTag)
        {
            if (string.IsNullOrWhiteSpace(xml))
                return new List<T>();
            var overrides = new XmlAttributeOverrides();
            var itemAttrs = new XmlAttributes();
            itemAttrs.XmlElements.Add(new XmlElementAttribute(itemTag, typeof(T)));
            overrides.Add(typeof(XmlListWrapper<T>), nameof(XmlListWrapper<T>.Items), itemAttrs);
            var rootAttr = new XmlRootAttribute(rootTag);
            var serializer = new XmlSerializer(typeof(XmlListWrapper<T>), overrides, new Type[0], rootAttr, null);

            using (var reader = new StringReader(xml))
            {
                var wrapper = serializer.Deserialize(reader) as XmlListWrapper<T>;
                return wrapper?.Items ?? new List<T>();
            }
        }

        public class XmlListWrapper<T>
        {
            public List<T> Items { get; set; }
        }
    }
}
