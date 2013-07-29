using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Monobjc.Tools.Generator.Model
{
	public partial class BaseEntity
	{
		public static void Change<T> (T entity, IEnumerable<String> statements) where T : BaseEntity
		{
		}
		
		public static void Change<T> (T entity, String statement) where T : BaseEntity
		{
            ParameterExpression p = Expression.Parameter (typeof(T), "entity");
            LambdaExpression expr = System.Linq.Dynamic.DynamicExpression.ParseAction<T> (new []{ p }, "entity." + statement, null);

            try {
                //Console.WriteLine ("Patching '" + entity.Name + "' with '" + expr + "'");
                Action<T> action = (Action<T>)expr.Compile ();
                action (entity);
            } catch(Exception) {
                Console.WriteLine ("Error while patching '" + entity.Name + "' with '" + expr + "'");
            }
		}
	}
}
