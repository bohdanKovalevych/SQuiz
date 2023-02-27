namespace SQuiz.Shared.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string validationMessage) : base(validationMessage)
        {
        }

        public BadRequestException()
        {
        }
    }
}
