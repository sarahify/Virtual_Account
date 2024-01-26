using VirtualAccount.Entity;

namespace VirtualAccount.Contract
{
    public interface IVirtualAccountRepo
    {
        Task<VirtualAcct?> GetVirtualAccount(int client_id);
        Task<VirtualAcct> CreateVirtualAccount(VirtualAcct virtualAccount);
    }
}
        
      
    
