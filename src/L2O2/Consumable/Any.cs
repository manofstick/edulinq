﻿using L2O2.Core;
using System;
using System.Collections.Generic;

namespace L2O2
{
    public static partial class Consumable
    {
        class AnyImpl<T> : Consumer<T, bool>
        {
            private Func<T, bool> selector;

            public AnyImpl(Func<T, bool> selector)
                : base(false)
            {
                this.selector = selector;
            }

            public override bool ProcessNext(T input, ref bool result)
            {
                if (selector(input))
                {
                    result = true;
                    StopFurtherProcessing();
                }
                return true; /*ignored*/
            }
        }

        internal static bool Any<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return Utils.Consume(source, new AnyImpl<TSource>(predicate));
        }

        internal static bool Any<TSource>(
            this IEnumerable<TSource> source)
        {
            return Any(source, _ => true);
        }
    }
}
