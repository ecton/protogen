using System;
using GraphQL;
using GraphQL.Types;
using SimpleExample.Models;

namespace SimpleExample.GraphQL
{
    public class TodoType : ObjectGraphType<Todo>
    {
        public TodoType()
        {
            Id(x => x.Id);
            Field<DateTimeOffset?>("completedAt", @"", resolve: ctx => ctx.Source.CompletedAt);
            Field<Todo>("parent", @"", resolve: ctx => 
            {
                var schemaContext = (SimpleExampleSchema.Context)ctx;
                return schemaContext.Database.Todos.Where(x => x.Id == ctx.Source.ParentId).FirstOrDefault();
            });
            Field<bool>("priority", @"", resolve: ctx => ctx.Source.Priority);
            Field<string>("task", @"", resolve: ctx => ctx.Source.Task);
        }
    }
}
