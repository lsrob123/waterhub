namespace Blog.Web.Config
{
    public static class PageDefinitions
    {
        public static class Home
        {
            public const string PageName = "Index", PageTitle = nameof(Home);
        }

        public static class SiteMap
        {
            public const string PageName = nameof(SiteMap), PageTitle = nameof(SiteMap);
        }

        public static class Admin
        {
            public const string PageName = nameof(Admin), PageTitle = nameof(Admin);
        }

        public static class Posts
        {
            public const string PageName = nameof(Posts), PageTitle = nameof(Posts), Context = nameof(Posts);
        }

        public static class Businesses
        {
            public const string PageName = nameof(Businesses), PageTitle = nameof(Businesses), Context = nameof(Businesses);
        }

        public static class Products
        {
            public const string PageName = nameof(Products), PageTitle = nameof(Products), Context = nameof(Products);
        }

        public static class Contact
        {
            public const string PageName = nameof(Contact), PageTitle = nameof(Contact), Context = nameof(Contact);
        }
    }
}
