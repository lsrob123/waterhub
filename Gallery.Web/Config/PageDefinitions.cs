namespace Gallery.Web.Config
{
    public static class PageDefinitions
    {
        public static class Home
        {
            public const string PageName = "Index", PageTitle = nameof(Home);
        }

        public static class Contact
        {
            public const string PageName = nameof(Contact), PageTitle = nameof(Contact);
        }
    }
}
