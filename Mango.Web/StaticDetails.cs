using System;
namespace Mango.Web
{
    public static class StaticDetails
    {
        public static string ProductAPIBase { get; set; }
    }
    public enum ApiType
    {
        POST,
        PUT,
        DELETE,
        GET
    }
}

