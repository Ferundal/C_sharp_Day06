using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using d_06.Model;
using Microsoft.Extensions.Configuration;

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var conf = configurationBuilder.Build();
if (!int.TryParse(conf["GoodServiceTime"], out var goodServiceTime)
    || !int.TryParse(conf["CustomerChangeTime"], out var customerChangeTime))
{
    Console.WriteLine("Wrong \"appsettings.json\" file parameters");
    return;
}

var cultureInfo = new CultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
Thread.CurrentThread.CurrentCulture = cultureInfo;

var store = new Store(50, 4, goodServiceTime, customerChangeTime);
var customersAmount = 10;
var customers = new Customer [customersAmount];
int index = 0;
for (; index < customersAmount; ++index)
{
    customers[index] = new Customer($"Customer {index + 1}", index + 1);
    customers[index].ShoppingList(7);
}

customers.AsParallel().ForAll(customer =>
{
    if (store.IsOpen())
        store.AddToQueue(customer, CustomerExtensions.HasLessGoods);
});
Console.WriteLine(store);
store.OpenRegisters();
TimeSpan timeSpan = new TimeSpan(hours: 0, minutes: 0, seconds: 7);
while (store.IsOpen())
{
    Thread.Sleep(timeSpan);
    var customer = new Customer($"Late Customer {index + 1}", index + 1);
    customer.ShoppingList(7);
    ++index;
    store.AddToQueue(customer, CustomerExtensions.HasLessGoods);
}
store.ProceedAllCustomers();
Console.WriteLine(store.Results());