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
            Field(
                typeof(DateTimeOffset).GetGraphTypeFromType(true),
                "completedAt",
                @"",
                resolve: ctx => ctx.Source.CompletedAt?.UtcDateTime
            );
            Field<TodoType>("parent", @"", resolve: ctx => 
            {
                var schemaContext = (SimpleExampleSchema.Context)ctx;
                return schemaContext.Database.Todos.Where(x => x.Id == ctx.Source.ParentId).FirstOrDefault();
            });
            Field(
                typeof(bool).GetGraphTypeFromType(false),
                "priority",
                @"",
                resolve: ctx => ctx.Source.Priority
            );
            Field(
                typeof(string).GetGraphTypeFromType(false),
                "task",
                @"",
                resolve: ctx => ctx.Source.Task
            );
            Connection<ListGraphType<TodoType>>()
                .Name("children")
                .Resolve(ctx => 
                {
                    var schemaContext = (SimpleExampleSchema.Context)ctx;
                    return schemaContext.Database.Todos.Where(x => x.ParentId == ctx.Source.Id);
                });
        }
    }
}
