namespace Test.Object.Scene
{
    public interface Iterable
    {
        /// <param name="index">The index of the iterator.</param>
        /// <returns>The iterator corresponding to the index.</returns>
        Iterable GetNextIterator(int index);

        /// <summary>
        /// List of iterators.
        /// </summary>
        public Iterable[] Iterators { get; }
    }
}