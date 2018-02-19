namespace Labs.WPF.Core.Notifiers
{
    public interface INotify<T>
    {
        void Notify(T obj);
    }
}
