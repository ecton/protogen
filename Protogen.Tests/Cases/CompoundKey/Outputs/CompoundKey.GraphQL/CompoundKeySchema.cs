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
        public CompoundKeySchema()
        {
            Query = new CompoundKeyQuery();
        }
    }
}
