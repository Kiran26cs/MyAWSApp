using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using MyCompanyApp.api.model;
using Newtonsoft.Json;

namespace profile.app.api.Controllers
{
    [ApiController]
    [Route("userapi")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB dbClient;
        public UserController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB dbClient, ILogger<UserController> logger)
        {
            _logger = logger;
            this.dynamoDBContext = dynamoDBContext;
            this.dbClient = dbClient;
        }
        [HttpGet(Name = "GetMe")]
        public async Task<List<User>> Index()
        {
            var UserDetail = await dynamoDBContext.ScanAsync<User>(null).GetRemainingAsync();
            return UserDetail;
        }
        [HttpPost(Name = "RegisterMe")]
        public async Task AddUser(User UserDetail)
        {
            await dynamoDBContext.SaveAsync<User>(UserDetail);
        }
        [HttpPut(Name = "UpdateMe")]
        public async Task UpdateUser(User UserDetail)
        {
            var item = Document.FromJson(JsonConvert.SerializeObject(UserDetail)); 
            var request = new PutItemRequest()
            {
                TableName=nameof(User),
                Item = item.ToAttributeMap()
            };
            await dbClient.PutItemAsync(request);
        }
    }
}
