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
            Field("id", x => x.Id, nullable: False).Description(@"");
            Field("complete", x => x.Complete, nullable: False).Description(@"");
            Field("parent_id", x => x.ParentId, nullable: True).Description(@"");
            Field("task", x => x.Task, nullable: False).Description(@"");
        }
    }
}
