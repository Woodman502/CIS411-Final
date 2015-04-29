using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroceryList.Startup))]
namespace GroceryList
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
