namespace Blog.Web.Config
{
    public static class PageDefinitions
    {
        public static class Home
        {
            public const string PageName = "Index", PageTitle = nameof(Home);
        }

        public static class Edit
        {
            public const string PageName = nameof(Edit), PageTitle = nameof(Edit);
        }

        public static class Posts
        {
            public const string PageName = nameof(Posts), PageTitle = nameof(Posts), Context = nameof(Posts);
        }
    }
}
