namespace Marketly.Core.Common
{
    public static class DataValidationConstants
    {
        public static class Ad
        {
            public const int TitleMinLength = 10;
            public const int TitleMaxLength = 100;
            public const int DescriptionMinLength = 20;
            public const int DescriptionMaxLength = 2000;
            public const string PriceMin = "0.00";
            public const string PriceMax = "1000000.00";
        }

        public static class Category
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Message
        {
            public const int ContentMinLength = 2;
            public const int ContentMaxLength = 1000;
        }

        public static class User
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 50;
            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 50;
        }
    }
}