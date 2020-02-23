namespace Blog.Web.Abstractions
{
    public interface IPostInfo
    {
        string Abstract { get; set; }
        bool IsPublished { get; set; }
        bool IsSticky { get; set; }
        string Title { get; set; }
        string UrlFriendlyTitle { get; set; }
    }
}