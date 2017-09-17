using System;
using GraphQL;
using GraphQL.Types;
using SimpleExample.GraphQL;

namespace SimpleExample.GraphQL
{
    public class TodoType : ObjectGraphType<Todo>
    {
        public SimpleExampleType()
        {
            Id(x => x.Id)
            Field("completedAt", x => x.CompletedAt, nullable: true).Description(@"");
            Field("parent", x => x.Parent, nullable: true).Description(@"");
            Field("priority", x => x.Priority, nullable: false).Description(@"");
            Field("task", x => x.Task, nullable: false).Description(@"");
        }
    }
}
