namespace Bench
{
    public class BenchmarkInfo
    {
        public string Group;

        public string Name;

        public int TestCount;

        public override string ToString()
        {
            return $"{Group}: {Name}({TestCount})";
        }
    }
}
