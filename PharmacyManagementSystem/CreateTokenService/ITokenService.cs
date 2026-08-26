using Core.Entities;

namespace PharmacyManagementSystem.CreateTokenService
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(User user);
    }
}
