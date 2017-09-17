using System;
using GraphQL;
using GraphQL.Types;
using SimpleExample.Models;

namespace SimpleExample.GraphQL
{
    public class SimpleExampleSchema : Schema
    {
        public class Context
        {
            public SimpleExampleDbContext Database { get; set; }
        }
        public SimpleExampleSchema(Func<Type, GraphType> resolveType) : base(resolveType)
        {
            Query = (SimpleExampleQuery)resolveType(typeof(SimpleExampleQuery));
        }
    }
}
