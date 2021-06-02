using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aq.ExpressionJsonSerializer.PreProcessor
{
    public class ProcessingVisitor: ExpressionVisitor
    {
        private readonly Assembly[] contractAssemblies;

        public ProcessingVisitor(Assembly[] contractAssemblies)
        {
            this.contractAssemblies = contractAssemblies;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression.Type.Assembly.FullName.StartsWith("System") || contractAssemblies.Contains(node.Expression.Type.Assembly))
            {
                // Safe to pass - return the default
                return base.VisitMember(node);
            }
            else
            {
                // Get the runtime value
                var objectMember = Expression.Convert(node, typeof(object));

                var getterLambda = Expression.Lambda<Func<object>>(objectMember);

                var getter = getterLambda.Compile();

                object result = getter();

                // Convert to a constant
                return Expression.Constant(result, node.Type);
            }
        }
    }
}
