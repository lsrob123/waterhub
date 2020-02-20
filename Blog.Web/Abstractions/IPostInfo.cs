namespace Blog.Web.Abstractions
{
    public interface IPostInfo
    {
        bool IsPublished { get; set; }
        bool IsSticky { get; set; }
        string Title { get; set; }
        string UrlFriendlyTitle { get; set; }
    }
}