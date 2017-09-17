using System;
using GraphQL;
using GraphQL.Types;
using CompoundKey.Models;

namespace CompoundKey.GraphQL
{
    public class CompoundKeySchema : Schema
    {
        public class Context
        {
            public CompoundKeyDbContext Database { get; set; }
        }
        public CompoundKeySchema(Func<Type, GraphType> resolveType) : base(resolveType)
        {
            Query = (CompoundKeyQuery)resolveType(typeof(CompoundKeyQuery));
        }
    }
}
