// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using Microsoft.EntityFrameworkCore;

using RigoFunc.DAL.Internal;

namespace RigoFunc.DAL.Extensions {
    /// <summary>
    /// Provides some extension methods for <see cref="ModelBuilder"/>
    /// </summary>
    public static class ModelBuilderExtensions {
        /// <summary>
        /// Enables the automatic recording change history.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>ModelBuilder.</returns>
        public static ModelBuilder EnableAutoHistory(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<ChangeHistory>()
                .Property(c => c.SourceId)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<ChangeHistory>()
                .Property(c => c.TypeName)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<ChangeHistory>()
                .Property(c => c.BeforeJson)
                .HasMaxLength(5000);

            modelBuilder.Entity<ChangeHistory>()
                .Property(c => c.AfterJson)
                .IsRequired()
                .HasMaxLength(5000);

            return modelBuilder;
        }
    }
}
