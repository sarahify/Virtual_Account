using Dapper;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using Serilog;
using System.ComponentModel.DataAnnotations;
using VirtualAccount.Context;
using VirtualAccount.Contract;
using VirtualAccount.Dto;
using VirtualAccount.Entity;
using VirtualAccount.Services.Interface;

namespace VirtualAccount.Services.Implementation
{

    public class VirtualAccountService : IVirtualAccountService
    {
        private readonly DapperContext _context;
        private readonly IVirtualAccountRepo _virtualAccountRepo;
        private readonly IConfiguration _config;
        private readonly ILogger<VirtualAccountService> _logger;
        private readonly PayStackCreateCustomerRequest _request;

        public VirtualAccountService(DapperContext context, IVirtualAccountRepo virtualAccountRepo, IConfiguration config, ILogger<VirtualAccountService> logger, PayStackCreateCustomerRequest request)
        {
            _config = config;
            _context = context;
            _virtualAccountRepo = virtualAccountRepo;
            _logger = logger;
            _request = request;
        }

        public  async Task<ResponseDto> CreatingVirtualAccountProcess(int client_id)
        {
            var result = new ResponseDto();
            try
            {
                _logger.LogInformation("$ Fetch information from the table view");
                var existingUser = await _virtualAccountRepo.GetVirtualAccount(client_id);
                if (existingUser == null)
                {
                    _logger.LogError("$ClientId does not exist");
                    result.Status = 400;
                    result.ErrorMessage = "Existing user not found";
                    result.DisplayMessage = "Error";
                    return result;
                }
                _logger.LogInformation("$ClientId exist in the table");
                var newCustomer = new RequestCustomerData()
                {
                    email = existingUser.email_address,
                    phone = existingUser.mobile_no,
                    firstName = existingUser.firstname,
                    lastName = existingUser.lastname,
                };
                var createCustomer = await _request.PayStackCreateNewCustomer(newCustomer);
                if (createCustomer == null)
                {
                    _logger.LogError("$Customer does not exist");
                    result.Status = 400;
                    result.ErrorMessage = "Customer not found in the database";
                    result.DisplayMessage = "Error";
                    return result;
                }
                var createPerson = new VirtualAcct()
                {
                    email_address = createCustomer.Data.email,
                    mobile_no = createCustomer.Data.phone,
                    fullname = createCustomer.Data.first_name + " " + createCustomer.Data.last_name,
                    client_id = client_id,
                };

                var storeVirtualAccountToDb = _virtualAccountRepo.CreateVirtualAccount(createPerson);
                if (storeVirtualAccountToDb == null)
                {
                    _logger.LogError("$was not successfully saved to  the database");
                    result.Status = 400;
                    result.ErrorMessage = "Customer not saved in the database";
                    result.DisplayMessage = "Error";
                    return result;
                }
                _logger.LogInformation("successfully saved to the database");
                var existingCustomer = new RequestExistingCustomerData()
                {
                    customer = createCustomer.Data.id,
                    preferred_bank = "test-bank"
                };
                var createExistingCustomer = await _request.PayStackCreateExistingCustomerDedicatedVirtualAccount(existingCustomer);
                if (createExistingCustomer == null)
                {
                    _logger.LogError("$Existing Customer does not exist");
                    result.Status = 400;
                    result.ErrorMessage = "Existing Customer not found in the database";
                    result.DisplayMessage = "Error";
                    return result;
                }
                _logger.LogInformation("Existing customer created");
                var assignVirtualAccount = new RequestAssignDVA()
                {
                    email = createCustomer.Data.email,
                    first_name = createCustomer.Data.first_name,
                    last_name = createCustomer.Data.last_name,
                    phone = createCustomer.Data.phone,
                    preferred_bank = "test-bank",
                    country = "NG"

                };
                var createAssignVirtualAccount = await _request.PayStackAssignDedicatedVirtualAccount(assignVirtualAccount);
                if (createAssignVirtualAccount == null)
                {
                    _logger.LogError("$Virtual Account was not Assigned");
                    result.Status = 400;
                    result.ErrorMessage = "Virtual Account was not assigned to the customer";
                    result.DisplayMessage = "Error";
                    return result;
                }
                _logger.LogInformation("$Assigned Dedicated Virtual Account was made successfully");
                result.Status = 200;
                result.DisplayMessage = "Successfully created Dedicated Virtual Account";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                result.Status = 500;
                result.ErrorMessage = ex.Message;
                result.DisplayMessage = "Error";
                return result;
            }
            

        }


        public async Task<IEnumerable<VirtualAcct>> FetchRecords()
        {
            string query = "SELECT client_id,fullname, firstname, lastname, email_address, mobile_no, middlename FROM fineract_default.m_new_virtual_account_view";

            using var semaphore = new SemaphoreSlim(50);
            using var connection = _context.CreateConnection();

            await semaphore.WaitAsync();
            try
            {
                var records = await connection.QueryAsync<VirtualAcct>(query);
                return records.ToList();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
       
        
    
}
