using Orchard;

namespace Moov2.Orchard.Analytics.Core.User
{
    public interface IUserProvider : IDependency
    {
        string GetUserIdentifier();
    }
}
