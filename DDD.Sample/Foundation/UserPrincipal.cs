using System.Security.Principal;

namespace DDD.Sample.Foundation
{
    public class UserPrincipal : IPrincipal
    {
        public UserPrincipal(UserIdentity userIndentity)
        {
            Identity = userIndentity;
        }

        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }

        public IUnitOfWork UnitOfWork { get; internal set; }

        public int VoteInUse { get; set; }
    }
}
