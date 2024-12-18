namespace HomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ЭТО ЗАДАНИЕ A
            int[] m = { };
            int[] m1 = new int[0];
            int[] m2 = { };
            int[] m3 = new int[0];

            // ЭТО ЗАДАНИЕ №1
            int[] fibonacci = new int[8];
            fibonacci[1] = 1;
            for (int i = 2; i < 8; i++)
            {
                fibonacci[i] = fibonacci[i - 1] + fibonacci[i - 2];
            }

            // ЭТО ЗАДАНИЕ №2
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            // ЭТО ЗАДАНИЕ №3
            int[,] powers = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    powers[i, j] = (int)Math.Pow((j + 2), (i + 1));
                }
            }

            // ЭТО ЗАДАНИЕ №4
            double[][] jaggedArray = new double[3][];
            jaggedArray[0] = new double[] { 1, 2, 3, 4, 5 };
            jaggedArray[1] = new double[] { Math.E, Math.PI };
            jaggedArray[2] = new double[] {
                Math.Log10(1),
                Math.Log10(10),
                Math.Log10(100),
                Math.Log10(1000)
            };

            // ЭТО ЗАДАНИЕ №5
            int[] array = { 1, 2, 3, 4, 5 };
            int[] array2 = { 7, 8, 9, 10, 11, 12, 13 };
            Array.Copy(array, 0, array2, 0, 3);

            // ЭТО ЗАДАНИЕ №6
            int[] resizedArray = { 1, 2, 3, 4, 5 };
            Array.Resize(ref resizedArray, resizedArray.Length * 2);
        }
    }
}
