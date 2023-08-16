using Dapper;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities
{
    public class EntityTypeMapper
    {
        public static void InitEntityMapper()
        {
            var entityTypeInfos = Assembly
            .GetEntryAssembly()
            .GetReferencedAssemblies()
            .Select(Assembly.Load)
            .SelectMany(x => x.DefinedTypes)
            .Where(type => typeof(IFullAudited)
            .IsAssignableFrom(type));

            foreach (var ti in entityTypeInfos)
            {
                var type = ti.AsType();
                if (!type.Equals(typeof(IFullAudited)))
                {
                    SqlMapper.SetTypeMap(
                    type,
                    new ColumnAttributeTypeMapper(type));
                }
            }
        }
    }
}
