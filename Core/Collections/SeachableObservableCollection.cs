using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Labs.WPF.Core.Collections
{
    public class SeachableObservableCollection<T> : ICollection<T>, INotifyCollectionChanged
    {
        public SeachableObservableCollection()
        {
            this._filteredItems = new List<T>();
            this._internalItems = new List<T>();
        }

        private Func<T, bool> _predicate;
        private List<T> _filteredItems;
        private List<T> _internalItems;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count => this._filteredItems.Count;

        public bool IsReadOnly => false;

        public void FilterItems(Func<T, bool> predicate)
        {
            this._predicate = predicate;
            //this._filteredItems.Clear();
            //this._filteredItems.AddRange(this._internalItems.Where(predicate));
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void ClearFilter()
        {
            this._predicate = null;
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(T item)
        {
            this._internalItems.Add(item);
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void AddRange(IEnumerable<T> items)
        {
            this._internalItems.AddRange(items);
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void Clear()
        {
            this._internalItems.Clear();
            this._filteredItems.Clear();
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return this._internalItems.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this._predicate == null)
                return this._internalItems.GetEnumerator();
            else
                return this._internalItems.Where(this._predicate).GetEnumerator();
        }

        public bool Remove(T item)
        {
            var result = this._internalItems.Remove(item);
            var position = this._internalItems.IndexOf(item);

            if (position > -1)
            {
                if (this.CollectionChanged != null)
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, position));
            }
            else
            {
                if (this.CollectionChanged != null)
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (this._predicate == null)
                return this._internalItems.GetEnumerator();
            else
                return this._internalItems.Where(this._predicate).GetEnumerator();
        }
    }
}
