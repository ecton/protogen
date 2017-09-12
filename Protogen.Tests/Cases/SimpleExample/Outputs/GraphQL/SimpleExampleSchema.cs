using System;
using GraphQL;
using GraphQL.Types;

namespace SimpleExample.GraphQL
{
    public class SimpleExampleSchema : Schema
    {
        public SimpleExampleSchema(Func<Type, GraphType> resolveType) : base(resolveType)
        {
            Query = (SimpleExampleQuery)resolveType(typeof(SimpleExampleQuery));
        }
    }
}
