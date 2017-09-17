using System;
using GraphQL;
using GraphQL.Types;

namespace SimpleExample.GraphQL
{
    public abstract class SimpleExampleQueryBase : ObjectGraphType
    {
        public SimpleExampleQueryBase()
        {
            Field<TodoType>(
                "allTodos",
                resolve: ResolveAllTodos,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
        }
        public abstract object ResolveAllTodos(ResolveFieldContext<object> context);
    }
}
