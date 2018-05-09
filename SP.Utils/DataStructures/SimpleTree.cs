using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.DataStructures
{
    /// <summary>
    /// Implementation based on: http://stackoverflow.com/a/942088/1498401
    /// </summary>
    public class SimpleTree<V> : HashSet<SimpleTree<V>>, IEquatable<SimpleTree<V>>
    {
        public V Key { get; set; }
        public V Value { get; set; }
        public string Description { get; set; }
        public SimpleTree<V> Parent { get; set; }
        public IEnumerable<Property> CustomProperties { get; set; }

        public SimpleTree() { }

        public SimpleTree(V key, V value, string desc = null, IEnumerable<Property> cproperties = null)
        {
            this.Key = key;
            this.Value = value;
            this.Description = desc;
            this.CustomProperties = cproperties;
        }

        public new void Add(SimpleTree<V> item)
        {
            base.Add(item);
            item.Parent = this;
        }

        /// <summary>
        /// Implementation based on: http://programmers.stackexchange.com/a/226162/200473
        /// </summary>
        public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Queue<T>();
            stack.Enqueue(item);
            while (stack.Any())
            {
                var next = stack.Dequeue();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Enqueue(child);
            }
        }

        public bool Equals(SimpleTree<V> other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return new { Count, Value }.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SimpleTree<V>)
                return this.Equals((SimpleTree<V>)obj);
            return false;
        }
    }

    public class Property
    {
        public string Name;
        public string Value;

        public Property(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}

