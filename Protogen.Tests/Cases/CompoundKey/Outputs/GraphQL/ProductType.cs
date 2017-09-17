using System;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class ProductType : ObjectGraphType<Product>
    {
        public ProductType()
        {
            Id(x => x.Id);
            Field(
                typeof(double).GetGraphTypeFromType(false),
                "msrp",
                @"",
                resolve: ctx => ctx.Source.Msrp
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
