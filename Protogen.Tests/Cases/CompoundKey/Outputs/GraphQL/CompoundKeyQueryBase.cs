using System;
using GraphQL;
using GraphQL.Types;

namespace CompoundKey.GraphQL
{
    public abstract class CompoundKeyQueryBase : ObjectGraphType
    {
        public CompoundKeyQueryBase()
        {
            Field<Product>(
                "allProducts",
                resolve: ResolveAllProducts,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument<int>
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument<int>
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
            Field<Store>(
                "allStores",
                resolve: ResolveAllStores,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument<int>
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument<int>
                    {
                        Name = "limit",
                        DefaultValue = 50,
                        Description = @"The number of rows to return"
                    }
                )
            );
            Field<StoreProduct>(
                "allStoreProducts",
                resolve: ResolveAllStoreProducts,
                description: @"",
                arguments: new QueryArguments(
                    new QueryArgument<int>
                    {
                        Name = "offset",
                        DefaultValue = 0,
                        Description = @"The offset to start the query at"
                    },
                    new QueryArgument<int>
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
