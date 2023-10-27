using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace MyCompanyApp.api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        var credentials = new BasicAWSCredentials("AKIAU55C56JPY5MNEC6Z", "N8+NpjZJZHj0mzarBSxixMJyVB/QeoQSBBvY6Tsy");
        var confg = new AmazonDynamoDBConfig()
        {
            RegionEndpoint = Amazon.RegionEndpoint.APSoutheast2
        };
        var client = new AmazonDynamoDBClient(credentials, confg);
        services.AddSingleton<IAmazonDynamoDB>(client);
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                var html = @"<h1>Welcome to running ASP.NET Core on AWS Lambda</h1>
                            <div>Following are the active endpoints with the webapi</div>
                            <div>#/userapi <strong>(HttpGet from dynamoDB)</strong></div>
                            <div>#/userapi <strong>(HttpPost to dynamoDB)</strong></div>
                            <div>#/userapi <strong>(HttpPut to dynamoDB)</strong></div>";
                await context.Response.WriteAsync(html);
            });
        });
    }
}