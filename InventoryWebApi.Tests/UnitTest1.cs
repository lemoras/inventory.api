using System;
using Xunit;
using InventoryWebApi.DataAccess;
using InventoryWebApi.DTO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using FluentAssertions;

namespace InventoryWebApi.Tests
{
    public class UnitTest1: IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public UnitTest1()
        {
            SetUpClient();
        }

        public void Dispose()
        {

        }

        public async Task SeedData()
        {

            // Create entry with barcode 1 
            var createForm0 = GenerateCreateForm("Shirt", "Clothes", 500, 150, 10);
            var response0 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            // Create entry with barcode 2 
            var createForm1 = GenerateCreateForm("Clock", "Decor", 1500, 100, 6);
            var response1 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            // Create entry with barcode 3 
            var createForm2 = GenerateCreateForm("Bedsheet", "Decor", 1000, 700, 60);
            var response2 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            // Create entry with barcode 4 
            var createForm3 = GenerateCreateForm("Cover", "Decor", 800, 200, 7);
            var response3 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));

            // Create entry with barcode 5
            var createForm4 = GenerateCreateForm("Skirt", "Clothes", 2000, 1000, 5);
            var response4 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm4), Encoding.UTF8, "application/json"));
        }

        // TEST NAME - CreateInventoryItem
        // TEST DESCRIPTION - A new item should be created
        [Fact]
        public async Task TestCase0()
        {
            await SeedData();

            // Create entry with barcode 6
            var createForm0 = GenerateCreateForm("Bag", "Utility", 300, 50, 2);
            var response0 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().BeEquivalentTo(201);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("{\"barcode\":6,\"name\":\"Bag\",\"category\":\"Utility\",\"price\":300,\"discount\":50,\"quantity\":2}");
            realData0.Should().BeEquivalentTo(expectedData0);

            // Create entry with barcode 7
            var createForm1 = GenerateCreateForm("Table", "Utility", 447, 107, 3);
            var response1 = await Client.PostAsync("/inventory/item", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));
            response1.StatusCode.Should().BeEquivalentTo(201);

            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            var expectedData1 = JsonConvert.DeserializeObject("{\"barcode\":7,\"name\":\"Table\",\"category\":\"Utility\",\"price\":447,\"discount\":107,\"quantity\":3}");
            realData1.Should().BeEquivalentTo(expectedData1);
        }


        // TEST NAME - GetInventoryItems
        // TEST DESCRIPTION - It finds all the items in the inventory
        [Fact]
        public async Task TestCase1()
        {
            await SeedData();

            // Get All items
            var response0 = await Client.GetAsync("/inventory/item");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":1,\"name\":\"Shirt\",\"category\":\"Clothes\",\"price\":500,\"discount\":150,\"quantity\":10},{\"barcode\":2,\"name\":\"Clock\",\"category\":\"Decor\",\"price\":1500,\"discount\":100,\"quantity\":6},{\"barcode\":3,\"name\":\"Bedsheet\",\"category\":\"Decor\",\"price\":1000,\"discount\":700,\"quantity\":60},{\"barcode\":4,\"name\":\"Cover\",\"category\":\"Decor\",\"price\":800,\"discount\":200,\"quantity\":7},{\"barcode\":5,\"name\":\"Skirt\",\"category\":\"Clothes\",\"price\":2000,\"discount\":1000,\"quantity\":5}]");
            realData0.Should().BeEquivalentTo(expectedData0);
        }

        // TEST NAME - getSingleItemByBarcode
        // TEST DESCRIPTION - It finds single item by barcode
        [Fact]
        public async Task TestCase2()
        {
            await SeedData();

            // Get Single item By Barcode 
            var response0 = await Client.GetAsync("inventory/item/query?barcode=5");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":5,\"name\":\"Skirt\",\"category\":\"Clothes\",\"price\":2000,\"discount\":1000,\"quantity\":5}]");
            realData0.Should().Equals(expectedData0);

            // Get Single item By barcode - which does not exist should return empty array
            var response1 = await Client.GetAsync("inventory/item/query?barcode=9");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        // TEST NAME - getitemsByCategory
        // TEST DESCRIPTION - It finds items by category
        [Fact]
        public async Task TestCase3()
        {
            await SeedData();

            // Get items by category
            var response0 = await Client.GetAsync("inventory/item/query?category=decor");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":3,\"name\":\"Bedsheet\",\"category\":\"Decor\",\"price\":1000,\"discount\":700,\"quantity\":60},{\"barcode\":4,\"name\":\"Cover\",\"category\":\"Decor\",\"price\":800,\"discount\":200,\"quantity\":7},{\"barcode\":5,\"name\":\"Skirt\",\"category\":\"Clothes\",\"price\":2000,\"discount\":1000,\"quantity\":5}]");
            realData0.Should().Equals(expectedData0);

            // If no such item exists, return empty array
            var response1 = await Client.GetAsync("inventory/item/query?category=abc");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        // TEST NAME - getitemsByName
        // TEST DESCRIPTION - It finds items by Name
        [Fact]
        public async Task TestCase4()
        {
            await SeedData();

            // Get item by name
            var response0 = await Client.GetAsync("inventory/item/query?name=Bedsheet");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":3,\"name\":\"Bedsheet\",\"category\":\"Decor\",\"price\":1000,\"discount\":700,\"quantity\":60}]");
            realData0.Should().Equals(expectedData0);

            // If no such item exists, return empty array
            var response1 = await Client.GetAsync("inventory/item/query?name=abc");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        //// TEST NAME - getitemsByDiscount
        //// TEST DESCRIPTION - It finds items greater than a particular discount value
        [Fact]
        public async Task TestCase5()
        {
            await SeedData();

            // Get items greater than a particular discount value 
            var response0 = await Client.GetAsync("inventory/item/query?discount=200");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":3,\"name\":\"Bedsheet\",\"category\":\"Decor\",\"price\":1000,\"discount\":700,\"quantity\":60},{\"barcode\":4,\"name\":\"Cover\",\"category\":\"Decor\",\"price\":800,\"discount\":200,\"quantity\":7},{\"barcode\":5,\"name\":\"Skirt\",\"category\":\"Clothes\",\"price\":2000,\"discount\":1000,\"quantity\":5}]");
            realData0.Should().Equals(expectedData0);

            // If no such item exists, return empty array
            var response1 = await Client.GetAsync("inventory/item/query?discount=3000");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }

        // TEST NAME - checkNonExistentApi
        // TEST DESCRIPTION - It should check if an API exists
        [Fact]
        public async Task TestCase6()
        {
            await SeedData();

            // Return with 404 if no API path exists 
            var response0 = await Client.GetAsync("/inventory/item/id/123");
            response0.StatusCode.Should().BeEquivalentTo(404);

            // Return with 405 if API path exists but called with different method
            var response1 = await Client.GetAsync("/inventory/item/123");
            response1.StatusCode.Should().BeEquivalentTo(405);
        }

        // TEST NAME - getSortedItems
        // TEST DESCRIPTION - It finds the items sorted by price
        [Fact]
        public async Task TestCase7()
        {
            await SeedData();

            // Get items sorted by price
            var response0 = await Client.GetAsync("inventory/item/sort");
            response0.StatusCode.Should().BeEquivalentTo(200);
            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"barcode\":5,\"name\":\"Skirt\",\"category\":\"Clothes\",\"price\":2000,\"discount\":1000,\"quantity\":5},{\"barcode\":2,\"name\":\"Clock\",\"category\":\"Decor\",\"price\":1500,\"discount\":100,\"quantity\":6},{\"barcode\":3,\"name\":\"Bedsheet\",\"category\":\"Decor\",\"price\":1000,\"discount\":700,\"quantity\":60},{\"barcode\":4,\"name\":\"Cover\",\"category\":\"Decor\",\"price\":800,\"discount\":200,\"quantity\":7},{\"barcode\":1,\"name\":\"Shirt\",\"category\":\"Clothes\",\"price\":500,\"discount\":150,\"quantity\":10}]");
            realData0.Should().BeEquivalentTo(expectedData0);
        }

        // TEST NAME - updateItems
        // TEST DESCRIPTION - Update item details
        [Fact]
        public async Task TestCase8()
        {
            await SeedData();
            // Return with 204 if item is updated 
            var body0 = JsonConvert.DeserializeObject("{\"name\":\"Bedsheet123\",\"category\":\"Decor123\",\"price\":1000,\"discount\":700,\"quantity\":60}");
            var response0 = await Client.PutAsync("/inventory/item/3", new StringContent(JsonConvert.SerializeObject(body0), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().Be(204);

            //Check if the item is updated
            var response1 = await Client.GetAsync("inventory/item/query?barcode=3");
            response1.StatusCode.Should().BeEquivalentTo(200);

            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            var expectedData1 = JsonConvert.DeserializeObject("[{\"barcode\":3,\"name\":\"Bedsheet123\",\"category\":\"Decor123\",\"price\":1000,\"discount\":700,\"quantity\":60}]");
            realData1.Should().Equals(expectedData1);
        }

        // TEST NAME - deleteItem
        // TEST DESCRIPTION - Delete an item by barcode
        [Fact]
        public async Task TestCase9()
        {
            await SeedData();

            // Return with 204 if item is deleted
            var response0 = await Client.DeleteAsync("inventory/item/2");
            response0.StatusCode.Should().Be(204);

            // Check if the item does not exist
            var response1 = await Client.GetAsync("inventory/item/query?barcode=2");
            response1.StatusCode.Should().BeEquivalentTo(200);
            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            realData1.Should().Equals("[]");
        }


        private CreateForm GenerateCreateForm(string name, string category, int price, int discount, int quantity)
        {
            return new CreateForm()
            {
                Category = category,
                Name = name,
                Price = price,
                Discount = discount,
                Quantity = quantity
            };
        }

        private void SetUpClient()
        {

            var builder = new WebHostBuilder()
                .UseStartup<InventoryWebApi.Startup>()
                .ConfigureServices(services =>
                {
                    var context = new InventoryContext(new DbContextOptionsBuilder<InventoryContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(InventoryContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
