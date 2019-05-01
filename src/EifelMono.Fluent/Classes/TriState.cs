namespace EifelMono.Fluent.Classes
{
    public enum TriState
    {
        Unkown,
        Yes,
        No
    }

    public static class TriStateExtensions
    {
        public static bool IsUnkown(this TriState thisValue)
            => thisValue == TriState.Unkown;
        public static bool IsYes(this TriState thisValue)
            => thisValue == TriState.Yes;
        public static bool IsNo(this TriState thisValue)
            => thisValue == TriState.No;
    }
}
