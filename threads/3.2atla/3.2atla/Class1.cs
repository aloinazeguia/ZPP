//Bertsio ASINKRONOA AWAIT-ekin (Prozesuaren zai geratzen da besteak blokeatu gabe)

static async Task Main(string[] args)

{

    Coffee cup = PourCoffee();

    Console.WriteLine("coffee is ready");

    var eggsTask = FryEggsAsync(2);

    var baconTask = FryBaconAsync(3);

    var toastTask = MakeToastWithButterAndJamAsync(2);

    var eggs = await eggsTask;

    Console.WriteLine("eggs are ready");

    var bacon = await baconTask;

    Console.WriteLine("bacon is ready");

    var toast = await toastTask;

    Console.WriteLine("toast is ready");

    Juice oj = PourOJ();

    Console.WriteLine("oj is ready");

    Console.WriteLine("Breakfast is ready!");

    async Task<Toast> MakeToastWithButterAndJamAsync(int number)

    {

        var toast = await ToastBreadAsync(number);

        ApplyButter(toast);

        ApplyJam(toast);

        return toast;

    }

}

await Task.WhenAll(eggTask, baconTask, toastTask);

Console.WriteLine("eggs are ready");

Console.WriteLine("bacon is ready");

Console.WriteLine("toast is ready");

Console.WriteLine("Breakfast is ready!");

