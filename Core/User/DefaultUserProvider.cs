using Orchard;

namespace Moov2.Orchard.Analytics.Core.User
{
    public class DefaultUserProvider : IUserProvider
    {
        #region Dependencies
        private readonly IWorkContextAccessor _workContextAccessor;
        #endregion

        #region Constructor
        public DefaultUserProvider(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }
        #endregion

        #region IUserProvider
        public string GetUserIdentifier()
        {
            var user = _workContextAccessor.GetContext().CurrentUser;
            return user != null ? user.UserName : string.Empty;
        }
        #endregion
    }
}