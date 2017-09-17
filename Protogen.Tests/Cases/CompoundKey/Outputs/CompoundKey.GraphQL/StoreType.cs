using System;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class StoreType : ObjectGraphType<Store>
    {
        public StoreType()
        {
            Field(
                typeof(long).GetGraphTypeFromType(false),
                "id",
                @"",
                resolve: ctx => ctx.Source.Id
            );
            Field(
                typeof(string).GetGraphTypeFromType(false),
                "name",
                @"",
                resolve: ctx => ctx.Source.Name
            );
        }
    }
}
