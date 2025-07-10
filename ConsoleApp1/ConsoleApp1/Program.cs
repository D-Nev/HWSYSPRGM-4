namespace ConsoleApp1
{
    internal class Program
    {
    static void Main()
    {
        int[] array = { 5, 3, 7, 3, 9, 1, 5, 6, 2, 9 };
        int searchValue = 7;

        Console.WriteLine("Исходный массив: " + string.Join(", ", array));

        Task<int[]> removeDuplicatesTask = Task.Run(() =>
        {    
            return array.Distinct().ToArray();
        });

        Task<int[]> sortTask = removeDuplicatesTask.ContinueWith(prevTask =>
        {
            Console.WriteLine("Сортировка массива ");
            int[] uniqueArray = prevTask.Result;
            Array.Sort(uniqueArray);
            return uniqueArray;
        });

        Task<int> searchTask = sortTask.ContinueWith(prevTask =>
        {
            Console.WriteLine("Значение: " + searchValue);
            int index = Array.BinarySearch(prevTask.Result, searchValue);
            return index;
        });

            searchTask.ContinueWith(prevTask =>
            {
                int[] sortedArray = sortTask.Result;
                Console.WriteLine("Отсортированный массив без дубликатов: " + string.Join(", ", sortedArray));
                int index = prevTask.Result;
                if (index >= 0)
                    Console.WriteLine($"Значение {searchValue} найдено по индексу {index}.");
                else
                    Console.WriteLine("Error");
            }).Wait();
        }

    }
}
