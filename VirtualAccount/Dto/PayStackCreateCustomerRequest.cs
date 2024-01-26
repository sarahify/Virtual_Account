using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Serilog;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using VirtualAccount.Context;
using VirtualAccount.Entity;
using VirtualAccount.Services.Implementation;

namespace VirtualAccount.Dto
{
    public class PayStackCreateCustomerRequest
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VirtualAccountService> _logger;
        private readonly DapperContext _context;

        public PayStackCreateCustomerRequest(IConfiguration configuration, ILogger<VirtualAccountService> logger, DapperContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        public async Task<PayStackVirtualAccount> PayStackCreateNewCustomer(RequestCustomerData requestCustomerData)
        {
            try
            {
                string apiUrl = _configuration["PayStack:CreateNewCustomer"];
                string authorizationToken = "Bearer " + _configuration["PayStack:AuthorizationToken"];

                using (HttpClient client = new HttpClient())
                {
                    // Set the base address of the API
                    client.BaseAddress = new Uri(apiUrl);

                    // Set the 'Authorization' header
                    client.DefaultRequestHeaders.Add("Authorization", authorizationToken);

                    // Serialize the request data and create StringContent to include in the POST request
                    var jsonContent = JsonConvert.SerializeObject(requestCustomerData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Make the POST request with content in the request body
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                    LogDto logdto = new LogDto
                    {
                        request = response.ToString(),
                        response = response.ToString(),
                        statusCode = (int)response.StatusCode,
                        ApiUrl = apiUrl,
                        logDate = DateTime.Now,
                    };

                    await LogRequest(logdto);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and handle the response
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //serilog
                        _logger.LogInformation(responseContent);
                        var result = JsonConvert.DeserializeObject<PayStackVirtualAccount>(responseContent);
                        return result;
                    }
                    else
                    {
                        _logger.LogError(response.ReasonPhrase, response.StatusCode);
                    }



                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
            return null;
        }


        public async Task<PayStackCreateExistingCustomerDVA> PayStackCreateExistingCustomerDedicatedVirtualAccount(RequestExistingCustomerData requestExistingCustomerData)
        {
            try
            {
                string apiUrl = _configuration["PayStack:CreateExistingCustomerDedicatedVirtualAccount"];
                string authorizationToken = "Bearer " + _configuration["PayStack:AuthorizationToken"];

                using (HttpClient client = new HttpClient())
                {

                    // Set the base address of the API
                    client.BaseAddress = new Uri(apiUrl);

                    // Set the 'Authorization' header
                    client.DefaultRequestHeaders.Add("Authorization", authorizationToken);

                    // Serialize the request data and create StringContent to include in the POST request
                    var jsonContent = JsonConvert.SerializeObject(requestExistingCustomerData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Make the POST request with content in the request body
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);


                    LogDto logdto = new LogDto
                    {
                        request = response.ToString(),
                        response = response.ToString(),
                        statusCode = (int)response.StatusCode,
                        ApiUrl = apiUrl,
                        logDate = DateTime.Now,
                    };

                    await LogRequest(logdto);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and handle the response
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //serilog
                        _logger.LogInformation(responseContent);
                        var result = JsonConvert.DeserializeObject<PayStackCreateExistingCustomerDVA>(responseContent);
                        return result;
                    }
                    else
                    {
                        _logger.LogError(response.ReasonPhrase, response.StatusCode);
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<PayStackAssignDVA> PayStackAssignDedicatedVirtualAccount(RequestAssignDVA assignVirtualAccount)
        {
            try
            {
                string apiUrl = _configuration["PayStack:AssignDedicatedVirtualAccount"];
                string authorizationToken = "Bearer " + _configuration["PayStack:AuthorizationToken"];

                using (HttpClient client = new HttpClient())
                {
                    // Set the base address of the API
                    client.BaseAddress = new Uri(apiUrl);

                    // Set the 'Authorization' header
                    client.DefaultRequestHeaders.Add("Authorization", authorizationToken);

                    // Serialize the request data and create StringContent to include in the POST request
                    var jsonContent = JsonConvert.SerializeObject(assignVirtualAccount);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Make the POST request with content in the request body
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);



                    LogDto logdto = new LogDto
                    {

                        request = response.ToString(),
                        response = response.ToString(),
                        statusCode = (int)response.StatusCode,
                        ApiUrl = apiUrl,
                        logDate = DateTime.Now,

                    };
                        
                   

                    await LogRequest(logdto);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and handle the response
                        string responseContent = await response.Content.ReadAsStringAsync();
                        //serilog
                        _logger.LogInformation(responseContent);
                        var result = JsonConvert.DeserializeObject<PayStackAssignDVA>(responseContent);
                        return result;
                    }
                    else
                    {
                        _logger.LogError(response.ReasonPhrase, response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> LogRequest(LogDto model)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var createException = new
                    {
                        LogId = model.LogId,
                        statusCode = model.statusCode,
                        request = model.request,
                        response = model.response,
                        ApiUrl = model.ApiUrl,
                        logDate = DateTime.Now,

                    };

                    string query = @"INSERT INTO virtual_acc_creation_logexdb_view (LogId, statusCode, request, response, ApiUrl, LogDate)
                        VALUES (@LogId, @statusCode, @request, @response, @ApiUrl, @LogDate);";

                    await connection.ExecuteAsync(query, createException);
                }
            }


            catch (Exception ex)
            {
                _logger.LogError("Exception not log to the database");
            }

            return true;

        }

    }

}
