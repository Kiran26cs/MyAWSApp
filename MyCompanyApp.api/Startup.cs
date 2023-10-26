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
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}