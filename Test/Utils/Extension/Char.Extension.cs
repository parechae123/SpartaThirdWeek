namespace Test.Utils.Extension
{
    public static class CharExtension
    {
        /// <param name="ch">Character to display.</param>
        /// <returns>Length of character graphically displayed on the console.</returns>
        public static int GetGraphicLength(this char ch)
        {
            return (0xFF00 & ch) == 0 ? 1 : 2;
        }

        /// <param name="ch">Character to display.</param>
        /// <returns>Logical length of Unicode character.</returns>
        public static int GetUnicodeLength(this char ch)
        {
            return (0xFF00 & ch) == 0 ? 0 : 1;
        }
    }
}