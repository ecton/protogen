using System;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class StoreType : ObjectGraphType<Store>
    {
        public StoreType()
        {
            Id(x => x.Id);
            Field(
                typeof(string).GetGraphTypeFromType(false),
                "name",
                @"",
                resolve: ctx => ctx.Source.Name
            );
        }
    }
}
