// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;

namespace RigoFunc.DAL.Internal {
    /// <summary>
    /// Enum ChangeKind
    /// </summary>
    internal enum ChangeKind {
        /// <summary>
        /// Indicating the change because of a new entity added to db context.
        /// </summary>
        Added,
        /// <summary>
        /// Indicating the change because of any entity had been modified.
        /// </summary>
        Modified,
        /// <summary>
        /// Indicating the change because of any entity had been deleted.
        /// </summary>
        Deleted,
    }

    /// <summary>
    /// Represents the entity change history.
    /// </summary>
    internal class ChangeHistory {
        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the source id.
        /// </summary>
        /// <value>The source id.</value>
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the json before changing.
        /// </summary>
        /// <value>The json before changing.</value>
        public string BeforeJson { get; set; }

        /// <summary>
        /// Gets or sets the json after changed.
        /// </summary>
        /// <value>The json after changed.</value>
        public string AfterJson { get; set; }

        /// <summary>
        /// Gets or sets the change kind.
        /// </summary>
        /// <value>The change kind.</value>
        public ChangeKind Kind { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
