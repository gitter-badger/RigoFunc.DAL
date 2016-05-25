// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

using RigoFunc.DAL.Internal;

namespace RigoFunc.DAL.Extensions {
    /// <summary>
    /// Provides some extension methods for <see cref="DbContext"/>.
    /// </summary>
    internal static class DbContextExtensions {
        public async static Task<int> SaveChangesWithHistoryAsync(this DbContext context) {
            var jsonSetting = new JsonSerializerSettings {
                ContractResolver = new EntityContractResolver(context),
            };

            var entries = context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged);
            foreach (var entry in entries) {
                var history = new ChangeHistory {
                    SourceId = entry.PrimaryKey(),
                    TypeName = entry.Entity.GetType().Name,
                };

                switch (entry.State) {
                    case EntityState.Added:
                        history.Kind = ChangeKind.Added;
                        history.AfterJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    case EntityState.Deleted:
                        history.Kind = ChangeKind.Deleted;
                        history.BeforeJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    case EntityState.Modified:
                        history.Kind = ChangeKind.Modified;
                        history.BeforeJson = JsonConvert.SerializeObject(entry.Original(), Formatting.Indented, jsonSetting);
                        history.AfterJson = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented, jsonSetting);
                        break;
                    default:
                        break;
                }

                context.Set<ChangeHistory>().Add(history);
            }

            return await context.SaveChangesAsync();
        }

        private static object Original(this EntityEntry entry) {
            var type = entry.Entity.GetType();
            var entity = Activator.CreateInstance(type, true);

            var properties = type.GetTypeInfo().GetProperties();
            foreach (var property in properties) {
                var original = entry.Property(property.Name).OriginalValue;
                property.SetValue(entity, original);
            }

            return entity;
        }

        private static string PrimaryKey(this EntityEntry entry) {
            var key = entry.Metadata.FindPrimaryKey();

            var values = new List<object>();
            foreach (var property in key.Properties) {
                var value = entry.Property(property.Name).CurrentValue;
                if(value != null) {
                    values.Add(value);
                }
            }

            return string.Join(",", values);
        }
    }
}
