﻿using System;
using SiliconStudio.Presentation.Transactions;

namespace SiliconStudio.Presentation.Tests.Transactions.Helpers
{
    internal class SimpleOperation : Operation
    {
        public Guid Guid { get; } = Guid.NewGuid();

        public bool IsDone { get; private set; } = true;

        public int RollbackCount { get; private set; }

        public int RollforwardCount { get; private set; }

        protected override void Rollback()
        {
            IsDone = false;
            ++RollbackCount;
        }

        protected override void Rollforward()
        {
            IsDone = true;
            ++RollforwardCount;
        }
    }
}
