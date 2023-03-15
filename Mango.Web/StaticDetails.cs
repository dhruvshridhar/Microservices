using System;
namespace Mango.Web
{
    public static class StaticDetails
    {
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }
    }
    public enum ApiType
    {
        POST,
        PUT,
        DELETE,
        GET
    }
}

