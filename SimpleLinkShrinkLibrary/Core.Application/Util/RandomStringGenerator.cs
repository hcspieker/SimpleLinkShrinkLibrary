namespace SimpleLinkShrinkLibrary.Core.Application.Util
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        private readonly Random _random;
        private readonly char[] _characterList;
        private readonly int _characterListLength;

        public RandomStringGenerator()
        {
            _random = new Random();
            var similarLookingCharacters = new List<char> { 'l', 'I', 'O', '0' };

            _characterList = [.. Enumerable.Range('a', 26)
                .Concat(Enumerable.Range('A', 26))
                .Concat(Enumerable.Range('0', 10))
                .Select(x => (char)x)
                .Where(x => !similarLookingCharacters.Any(y => y == x))];

            _characterListLength = _characterList.Length;
        }

        public string GenerateRandomString(int length)
        {
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = _characterList[_random.Next(_characterListLength)];
            }

            return new string(result);
        }
    }
}
