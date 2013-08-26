using System;
using System.Linq.Expressions;

namespace DDD.Sample.Foundation
{
    public abstract class Specification<TEntity> 
    {
        protected Specification()
        {

        }
	    protected Specification(Expression<Func<TEntity, bool>> satisfiedBy) 
        {
		    SatisfiedBy = satisfiedBy;
	    }
	
	    public Expression<Func<TEntity, bool>> SatisfiedBy { get; protected set; }
    }
}
