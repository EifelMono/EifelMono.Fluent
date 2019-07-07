namespace EifelMono.Fluent.Classes
{
    public class ValueBox
    {
        public static ValueBox<V1> Create<V1>(V1 item1)
            => new ValueBox<V1>()
            .In(item1);
        public static ValueBox<V1, V2> Create<V1, V2>
            (V1 item1, V2 item2)
            => new ValueBox<V1, V2>()
            .In(item1, item2);
        public static ValueBox<V1, V2, V3> Create<V1, V2, V3>
            (V1 item1, V2 item2, V3 item3)
            => new ValueBox<V1, V2, V3>()
            .In(item1, item2, item3);
        public static ValueBox<V1, V2, V3, V4> Create<V1, V2, V3, V4>
            (V1 item1, V2 item2, V3 item3, V4 item4)
            => new ValueBox<V1, V2, V3, V4>()
            .In(item1, item2, item3, item4);
        public static ValueBox<V1, V2, V3, V4, V5> Create<V1, V2, V3, V4, V5>
            (V1 item1, V2 item2, V3 item3, V4 item4, V5 item5)
            => new ValueBox<V1, V2, V3, V4, V5>()
            .In(item1, item2, item3, item4, item5);
        public static ValueBox<V1, V2, V3, V4, V5, V6> Create<V1, V2, V3, V4, V5, V6>
            (V1 item1, V2 item2, V3 item3, V4 item4, V5 item5, V6 item6)
            => new ValueBox<V1, V2, V3, V4, V5, V6>()
            .In(item1, item2, item3, item4, item5, item6);
        public static ValueBox<V1, V2, V3, V4, V5, V6, V7> Create<V1, V2, V3, V4, V5, V6, V7>
            (V1 item1, V2 item2, V3 item3, V4 item4, V5 item5, V6 item6, V7 item7)
            => new ValueBox<V1, V2, V3, V4, V5, V6, V7>()
            .In(item1, item2, item3, item4, item5, item6, item7);
        public static ValueBox<V1, V2, V3, V4, V5, V6, V7, V8> Create<V1, V2, V3, V4, V5, V6, V7, V8>
            (V1 item1, V2 item2, V3 item3, V4 item4, V5 item5, V6 item6, V7 item7, V8 item8)
            => new ValueBox<V1, V2, V3, V4, V5, V6, V7, V8>()
            .In(item1, item2, item3, item4, item5, item6, item7, item8);
    }

    public class ValueBox<V1> : ValueBox
    {
        public V1 Item1 { get; set; }

        public ValueBox<V1> In(in V1 item1)
        {
            Item1 = item1;
            return this;
        }

        public ValueBox<V1> Out(out V1 item1)
        {
            item1 = Item1;
            return this;
        }

        public void Deconstruct(out V1 item1)
        {
            item1 = Item1;
        }
    }

    public class ValueBox<V1, V2> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }

        public ValueBox<V1, V2> In(in V1 item1, in V2 item2)
        {
            Item1 = item1;
            Item2 = item2;
            return this;
        }
        public ValueBox<V1, V2> Out(out V1 item1, out V2 item2)
        {
            item1 = Item1;
            item2 = Item2;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }

    public class ValueBox<V1, V2, V3> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }

        public ValueBox<V1, V2, V3> In(in V1 item1, in V2 item2, in V3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            return this;
        }

        public ValueBox<V1, V2, V3> Out(out V1 item1, out V2 item2, out V3 item3)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
        }
    }

    public class ValueBox<V1, V2, V3, V4> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }
        protected V4 Item4 { get; set; }


        public ValueBox<V1, V2, V3, V4> In(in V1 item1, in V2 item2, in V3 item3, in V4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            return this;
        }

        public ValueBox<V1, V2, V3, V4> Out(out V1 item1, out V2 item2, out V3 item3, out V4 item4)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3, out V4 item4)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
        }
    }

    public class ValueBox<V1, V2, V3, V4, V5> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }
        protected V4 Item4 { get; set; }
        protected V5 Item5 { get; set; }

        public ValueBox<V1, V2, V3, V4, V5> In(in V1 item1, in V2 item2, in V3 item3, in V4 item4,
            in V5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            return this;
        }

        public ValueBox<V1, V2, V3, V4, V5> Out(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
        }
    }

    public class ValueBox<V1, V2, V3, V4, V5, V6> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }
        protected V4 Item4 { get; set; }
        protected V5 Item5 { get; set; }
        protected V6 Item6 { get; set; }

        public ValueBox<V1, V2, V3, V4, V5, V6> In(in V1 item1, in V2 item2, in V3 item3, in V4 item4,
            in V5 item5, in V6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            return this;
        }

        public ValueBox<V1, V2, V3, V4, V5, V6> Out(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
        }
    }

    public class ValueBox<V1, V2, V3, V4, V5, V6, V7> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }
        protected V4 Item4 { get; set; }
        protected V5 Item5 { get; set; }
        protected V6 Item6 { get; set; }
        protected V7 Item7 { get; set; }

        public ValueBox<V1, V2, V3, V4, V5, V6, V7> In(in V1 item1, in V2 item2, in V3 item3, in V4 item4,
            in V5 item5, in V6 item6, in V7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            return this;
        }

        public ValueBox<V1, V2, V3, V4, V5, V6, V7> Out(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6, out V7 item7)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            item7 = Item7;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6, out V7 item7)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            item7 = Item7;
        }
    }

    public class ValueBox<V1, V2, V3, V4, V5, V6, V7, V8> : ValueBox
    {
        protected V1 Item1 { get; set; }
        protected V2 Item2 { get; set; }
        protected V3 Item3 { get; set; }
        protected V4 Item4 { get; set; }
        protected V5 Item5 { get; set; }
        protected V6 Item6 { get; set; }
        protected V7 Item7 { get; set; }
        protected V8 Item8 { get; set; }

        public ValueBox<V1, V2, V3, V4, V5, V6, V7, V8> In(in V1 item1, in V2 item2, in V3 item3, in V4 item4,
            in V5 item5, in V6 item6, in V7 item7, in V8 item8)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Item8 = item8;
            return this;
        }

        public ValueBox<V1, V2, V3, V4, V5, V6, V7, V8> Out(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6, out V7 item7, out V8 item8)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            item7 = Item7;
            item8 = Item8;
            return this;
        }

        public void Deconstruct(out V1 item1, out V2 item2, out V3 item3, out V4 item4,
            out V5 item5, out V6 item6, out V7 item7, out V8 item8)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            item7 = Item7;
            item8 = Item8;
        }
    }
}
