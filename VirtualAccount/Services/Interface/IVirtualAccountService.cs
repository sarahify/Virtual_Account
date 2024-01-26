using Dapper;
using System.Data;
using VirtualAccount.Dto;
using VirtualAccount.Entity;

namespace VirtualAccount.Services.Interface
{
    public interface IVirtualAccountService
    {
        Task<ResponseDto> CreatingVirtualAccountProcess(int client_id);
        Task<IEnumerable<VirtualAcct>> FetchRecords();




    }
}
