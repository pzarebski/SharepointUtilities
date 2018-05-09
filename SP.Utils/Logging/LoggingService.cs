using Microsoft.SharePoint.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Logging
{
    public class LoggingService : SPDiagnosticsServiceBase
    {
        public enum EventCategory
        {
            MASTERPAGE,
            INTRANET_WEBPARTS,
            WEB_RECEIVER,
            NAVIGATION_PROVIDER,
            WEBSERVICE_PROJECTCREATION,
            TIMERJOB,
            AUDIENCE_HELPER,
            HTTP_MODUE_FILEACCESSTRACKING,
            KEYWORD_SEARCH_QUERYING
        }

        public static string DefaultDiagnosticAreaName = "SP.Utils";
        private static volatile LoggingService instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Multi-threaded singleton instance: http://msdn.microsoft.com/en-us/library/ff650316.aspx
        /// </summary>
        public static LoggingService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LoggingService();
                        }
                    }
                }

                return instance;
            }
        }


        private LoggingService() : base("", SPFarm.Local)
        {
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsCategory> categories = new List<SPDiagnosticsCategory>();
            foreach (string category in Enum.GetNames(typeof(EventCategory)))
            {
                categories.Add(new SPDiagnosticsCategory(category + "_ERROR", TraceSeverity.Unexpected, EventSeverity.Error));
                categories.Add(new SPDiagnosticsCategory(category + "_INFORMATION", TraceSeverity.Monitorable, EventSeverity.Information));
            }

            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
            {
                new SPDiagnosticsArea(DefaultDiagnosticAreaName, categories)
            };

            return areas;
        }

        public static void LogError(EventCategory enumCategory, string errorMessage)
        {
            SPDiagnosticsCategory category = LoggingService.Instance.Areas[DefaultDiagnosticAreaName].Categories[enumCategory.ToString() + "_ERROR"];
            LoggingService.Instance.WriteTrace(0, category, category.TraceSeverity, errorMessage);
        }

        public static void LogInformation(EventCategory enumCategory, string infoMessage)
        {
            SPDiagnosticsCategory category = LoggingService.Instance.Areas[DefaultDiagnosticAreaName].Categories[enumCategory.ToString() + "_INFORMATION"];
            LoggingService.Instance.WriteTrace(0, category, category.TraceSeverity, infoMessage);
        }
    }
}
