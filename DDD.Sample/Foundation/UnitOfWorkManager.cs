using System.Security;
using System.Threading;

namespace DDD.Sample.Foundation
{
    public static class UnitOfWorkManager
    {

        public static IUnitOfWork GetInstance()
        {
            var userPrincipal = Thread.CurrentPrincipal as UserPrincipal;
            if (userPrincipal == null)
                throw new SecurityException();
            userPrincipal.VoteInUse++;
            if (userPrincipal.UnitOfWork != null)
            {
                return userPrincipal.UnitOfWork;
            }
            var unitOfWork = IoC.Resolve<IUnitOfWork>();
            return userPrincipal.UnitOfWork = unitOfWork;
        }

        public static void Unregister()
        {
            var userPrincipal = Thread.CurrentPrincipal as UserPrincipal;
            if (userPrincipal == null)
                return;
            userPrincipal.VoteInUse--;
            if (userPrincipal.VoteInUse == 0)
            {
                userPrincipal.UnitOfWork = null;
            }
        }
    }
}
