using EPIC.EntitiesBase.Dto;
using MongoDB.Bson;
using System.Text.Json;

namespace EPIC.EntitiesBase.MongoDb
{
    public static class PagingRequestMongoExtension
    {
        /// <summary>
        /// Chuyển object sang filter của mongo Db
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BsonDocument ParseFilter(this PagingRequestMongoBaseDto input)
        {
            return BsonDocument.Parse(JsonSerializer.Serialize(input.Filter));
        }
    }
}
