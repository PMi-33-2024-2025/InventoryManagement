namespace InventoryManagement.BLL.Helpers
{
    public static class Validator
    {
        public static bool IsStringValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static bool IsDecimalValid(decimal input)
        {
            return input > 0;
        }

        public static bool IsIntValid(int input)
        {
            return input >= 0;
        }
    }
}
