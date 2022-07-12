namespace PaymentGateway.Domain.Common
{
    public static class DateTimeProvider
    {
        private static Func<DateTime> _dateTimeNowFunc = () => DateTime.UtcNow;
        public static DateTime Now => _dateTimeNowFunc();

        public static void Set(Func<DateTime> dateTimeNowFunc)
        {
            _dateTimeNowFunc = dateTimeNowFunc;
        }
    }
}
