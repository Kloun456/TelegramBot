using CoffeBot.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeBot
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        { 
           //string? connection = Configuration.GetConnectionString("DefaultConnection");
           //services.AddDbContext<ApplicationDbContext>(options => 
           //     options.UseNpgsql(connection));
            services.AddTransient<IBotService, BotService>();
            
        }
    }
}
