using System;
using GraphQL;
using GraphQL.Types;

namespace SimpleExample.Models
{
    public class SimpleExampleSchema : Schema
    {
        public SimpleExampleSchema(Func<Type, GraphType> resolveType) : base(resolveType)
        {
            Query = (SimpleExampleQuery)resolveType(typeof(SimpleExampleQuery));
        }
    }
}
