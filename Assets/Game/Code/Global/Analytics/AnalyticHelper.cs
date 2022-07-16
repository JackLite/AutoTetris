using System.Collections.Generic;
using Utilities;

namespace Global.Analytics
{
    public static class AnalyticHelper
    {
        public static AnalyticEvent CreateEvent(string id)
        {
            return new AnalyticEvent { eventId = id };
        }

        public static AnalyticEvent CreateEvent(string id, string data)
        {
            return new AnalyticEvent { eventId = id, data = new Variant(data) };
        }

        public static AnalyticEvent CreateEvent(string id, float data)
        {
            return new AnalyticEvent { eventId = id, data = new Variant(data) };
        }

        public static AnalyticEvent CreateEvent(string id, Dictionary<string, string> data)
        {
            return new AnalyticEvent { eventId = id, data = new Variant(data) };
        }
    }
}