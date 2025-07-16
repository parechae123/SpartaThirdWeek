namespace RtanRPG.Object.Scene
{
    public interface Iterable
    {
        /// <param name="index"></param>
        /// <returns></returns>
        Iterable GetNextIterator(int index);

        /// <summary>
        /// A list of iterators stored as one-to-one correspondences to keys.
        /// </summary>
        Iterable[] Iterators { get; }
    }
}