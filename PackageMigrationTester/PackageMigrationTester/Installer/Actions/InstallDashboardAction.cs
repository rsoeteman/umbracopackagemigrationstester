using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PackageMigrationTester.Helpers;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;

namespace PackageMigrationTester.Installer.Actions
{
   public class InstallDashboardAction 
    {
        /// <summary>
        /// Installs a new dashboard in dashboard.config file.
        /// </summary>
        public void Install()
        {
            try
            {
                var doc = XmlHelper.LoadXmlDocument(ConfigFileName);

                if (!SectionExists(doc, SectionAlias))
                {
                    var el = new XElement("section", new XAttribute("alias", SectionAlias),
                        new XElement("areas",
                            new XElement("area", Area)),
                        new XElement("tab", new XAttribute("caption", TabCaption),
                            new XElement("control", TabControl)));

                    doc.Element("dashBoard").Add(el);

                    doc.Save(ConfigFileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<InstallDashboardAction>(string.Format("Unable install section '{0}' to the dashboard.config file", SectionAlias), ex);
            }
        }
        
        /// <summary>
        /// Will removes a dashbaord setcion from the dashboard.config file
        /// </summary>
        public void UnInstall()
        {
            try
            {
                var doc = XmlHelper.LoadXmlDocument(ConfigFileName);

                var sectionElement = doc.Root.Descendants("section").FirstOrDefault(el => el.Attribute("alias") != null && el.Attribute("alias").Value.Equals(SectionAlias));
                if (sectionElement != null)
                {
                    sectionElement.Remove();
                    doc.Save(ConfigFileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<InstallDashboardAction>(string.Format("Unable to uninstall section '{0}' from dashboard.config file", SectionAlias), ex);
            }
        }
        
        /// <summary>
        /// Checks if the section exists
        /// </summary>
        public bool SectionExists(XDocument doc, string sectionAlias)
        {
            var result = doc.Root.Descendants("section").FirstOrDefault(el => el.Attribute("alias") != null &&
                         el.Attribute("alias").Value.Equals(SectionAlias));

            return result != null;
        }

        /// <summary>
        /// Name of the configuation file
        /// </summary>
        private string ConfigFileName => IOHelper.MapPath("~/config/dashboard.config");

        /// <summary>
        /// The section alias to configure
        /// </summary>
        public string SectionAlias { get; set; }

        /// <summary>
        /// Area where this dashboard needs to be configured for
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Caption of the tab.
        /// </summary>
        public string TabCaption { get; set; }

        /// <summary>
        /// The control/ HTML view to display
        /// </summary>
        public string TabControl { get; set; }
    }
}