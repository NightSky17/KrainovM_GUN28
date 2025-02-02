namespace Final_Task.Dices
{
    public class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException(int number, int min, int max)
            : base($"Invalid dice number {number}. Allowed range: {min} - {max}.")
        {
        }
    }
}
