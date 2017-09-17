using System;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class StoreProductType : ObjectGraphType<StoreProduct>
    {
        public StoreProductType()
        {
            Id(x => $"{x.ProductId}::{x.StoreId}");
            Field<ProductType>("product", @"", resolve: ctx => 
            {
                var schemaContext = (CompoundKeySchema.Context)ctx;
                return schemaContext.Database.Products.Where(x => x.Id == ctx.Source.ProductId).FirstOrDefault();
            });
            Field<StoreType>("store", @"", resolve: ctx => 
            {
                var schemaContext = (CompoundKeySchema.Context)ctx;
                return schemaContext.Database.Stores.Where(x => x.Id == ctx.Source.StoreId).FirstOrDefault();
            });
            Field(
                typeof(double).GetGraphTypeFromType(false),
                "price",
                @"",
                resolve: ctx => ctx.Source.Price
            );
        }
    }
}
