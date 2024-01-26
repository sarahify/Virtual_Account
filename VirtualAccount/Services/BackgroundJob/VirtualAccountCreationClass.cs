using Quartz;
using VirtualAccount.Services.Interface;

namespace VirtualAccount.Services.BackgroundJob
{
    public class VirtualAccountCreationClass : IJob
    {
        private readonly ILogger<VirtualAccountCreationClass> _logger;
        private readonly IVirtualAccountService _virtualAccountService;


        public VirtualAccountCreationClass(IVirtualAccountService virtualAccountService, ILogger<VirtualAccountCreationClass> logger)
        {
            _virtualAccountService = virtualAccountService;
            _logger = logger;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            var existingUsers = await _virtualAccountService.FetchRecords();
            if (existingUsers == null)
            {
                _logger.LogError("---No records returned-----");
            }
            _logger.LogInformation("---All records returned-----");


            int processedCount = 0;

            foreach (var user in existingUsers)
            {
                _logger.LogInformation("Creating account for customer with clientId of: " + user.client_id);
                var createVirtualAccount = await _virtualAccountService.CreatingVirtualAccountProcess(user.client_id);

                if (createVirtualAccount.Status == 200)
                {
                    _logger.LogInformation($"Client ID: {user.client_id}, FullName: {user.fullname}, FirstName: {user.firstname}, LastName: {user.lastname}, EmailAddress: {user.email_address}, Phone: {user.mobile_no}, MiddleName: {user.middlename}");
                    processedCount++;

                    // Stop processing after 50 records
                    if (processedCount >= 50)
                    {
                        break;
                    }
                }
                else
                {
                    _logger.LogError("User not processed successfully");
                }

            }
        }
    }
}
