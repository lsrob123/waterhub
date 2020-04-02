namespace Blog.Web.Abstractions
{
    public interface ISiteInfoService
    {
        string GetFullPageTitleForLayout(string pageTitle);
    }
}