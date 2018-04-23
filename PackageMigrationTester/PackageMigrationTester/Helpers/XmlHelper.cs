using System.IO;
using System.Xml.Linq;

namespace PackageMigrationTester.Helpers
{
    public static class XmlHelper
    {
        public static XDocument LoadXmlDocument(string fileName)
        {
            XDocument doc = null;

            using (FileStream xmlFile = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                doc = XDocument.Load(xmlFile);
            }
            return doc;
        }
    }
}