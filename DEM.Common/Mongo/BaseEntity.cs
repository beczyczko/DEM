using System;
using DEM.Common.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DEM.Common.Mongo
{
    public abstract class BaseEntity : IIdentifiable
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; protected set; }
    }
}