using System;
using GraphQL;
using GraphQL.Types;

namespace SimpleExample.Models
{
    public abstract class SimpleExampleQueryBase : ObjectGraphType
    {
        public SimpleExampleQueryBase()
        {
            Field<Todo>(
                "allTodos",
                resolve: ResolveAllTodos,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument<int>
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument<int>
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
