using System;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class StoreProductType : ObjectGraphType<StoreProduct>
    {
        public StoreProductType()
        {
            Field<StringGraphType>(
                "id",
                @"",
                resolve: ctx => $"{ctx.Source.ProductId}::{ctx.Source.StoreId}");
                Field<ProductType>("product", @"", resolve: ctx => 
                {
                    var schemaContext = (CompoundKeySchema.Context)ctx.UserContext;
                    return schemaContext.Database.Products.Where(x => x.Id == ctx.Source.ProductId).FirstOrDefault();
                });
                Field<StoreType>("store", @"", resolve: ctx => 
                {
                    var schemaContext = (CompoundKeySchema.Context)ctx.UserContext;
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
