using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Base
{
    public class BaseScenario
    {
        public async Task<IHost> GetHost()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(wb =>
                {
                    wb
                        .UseTestServer()
                        .UseEnvironment("test")
                        .UseContentRoot(Path.GetDirectoryName(Assembly.GetAssembly(typeof(BaseScenario)).Location))
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton(new InMemorySetup());
                        })
                        .UseStartup<TestStartup>();
                });
            var host = await hostBuilder.StartAsync();

            return host;
        }

        protected StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj ?? string.Empty), Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializeResponseContent<T>(HttpResponseMessage responseMessage)
        {
            var responseString = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}