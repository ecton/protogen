using System;
using GraphQL;
using GraphQL.Types;

namespace CompoundKey.GraphQL
{
    public abstract class CompoundKeyQueryBase : ObjectGraphType
    {
        public CompoundKeyQueryBase()
        {
            Field<ProductType>(
                "allProducts",
                resolve: ResolveAllProducts,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
            Field<StoreType>(
                "allStores",
                resolve: ResolveAllStores,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
            Field<StoreProductType>(
                "allStoreProducts",
                resolve: ResolveAllStoreProducts,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument(typeof(int).GetGraphTypeFromType(true))
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
        }
        public abstract object ResolveAllProducts(ResolveFieldContext<object> context);
        public abstract object ResolveAllStores(ResolveFieldContext<object> context);
        public abstract object ResolveAllStoreProducts(ResolveFieldContext<object> context);
    }
}
