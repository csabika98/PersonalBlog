
namespace PersonalBlogCsabaSallai.Services
{
    public class ViewLocatorService
    {
        private readonly IWebHostEnvironment _environment;

        public ViewLocatorService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IEnumerable<string> GetViewNames(string controllerName)
        {
            var viewsPath = Path.Combine(_environment.ContentRootPath, "Views", controllerName);
            if (!Directory.Exists(viewsPath))
            {
                yield break;
            }

            foreach (var file in Directory.GetFiles(viewsPath, "*.cshtml"))
            {
                yield return Path.GetFileNameWithoutExtension(file);
            }
        }
    }
}