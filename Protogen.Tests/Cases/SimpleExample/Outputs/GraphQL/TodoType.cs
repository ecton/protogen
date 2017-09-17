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
            Field("id", x => x.Id, nullable: false).Description(@"");
            Field("completed_at", x => x.CompletedAt, nullable: true).Description(@"");
            Field("parent_id", x => x.ParentId, nullable: true).Description(@"");
            Field("priority", x => x.Priority, nullable: false).Description(@"");
            Field("task", x => x.Task, nullable: false).Description(@"");
        }
    }
}
