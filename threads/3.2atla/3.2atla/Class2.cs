//Bertsio ASINKRONOA Task.WhenAny-ekin (aurreko kasuan bezela prozesuak amaitu arte itxaroten degu, baina kasu honetan bukatu bezain laister abisatuz)

static async Task Main(string[] args)

{

    Coffee cup = PourCoffee();

    Console.WriteLine("coffee is ready");

    var eggsTask = FryEggsAsync(2);

    var baconTask = FryBaconAsync(3);

    var toastTask = MakeToastWithButterAndJamAsync(2);

    var allTasks = new List<Task> { eggsTask, baconTask, toastTask };

    while (allTasks.Any())

    {

        Task finished = await Task.WhenAny(allTasks);

        if (finished == eggsTask)

        {

            Console.WriteLine("eggs are ready");

        }

        else if (finished == baconTask)

        {

            Console.WriteLine("bacon is ready");

        }

        else if (finished == toastTask)

        {

            Console.WriteLine("toast is ready");

        }

        allTasks.Remove(finished);

    }

    Console.WriteLine("Breakfast is ready!");

    async Task<Toast> MakeToastWithButterAndJamAsync(int number)

    {

        var toast = await ToastBreadAsync(number);

        ApplyButter(toast);

        ApplyJam(toast);

        return toast;

    }

}