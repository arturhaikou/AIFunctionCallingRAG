using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AIFunctionCallingRAG.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Color", "Description", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { new Guid("02c53ded-3671-43d1-1a7c-2081ef3b1c62"), "silver", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Tasty Plastic Mouse", 966.52m, "L" },
                    { new Guid("02d08080-35b2-75ea-9bae-7d10444e6cc2"), "cyan", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Small Concrete Salad", 998.15m, "L" },
                    { new Guid("04fc3275-56d6-02f0-924c-ca1e2755c893"), "magenta", "The Football Is Good For Training And Recreational Purposes", "Incredible Frozen Bacon", 625.64m, "XXL" },
                    { new Guid("0f2491d7-e351-dde8-f8e6-38fa8176a30e"), "orange", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Unbranded Wooden Fish", 83.75m, "XXL" },
                    { new Guid("114d4fba-1a31-12c6-331d-4576c5889869"), "lime", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Ergonomic Cotton Hat", 814.03m, "M" },
                    { new Guid("124c980f-36ca-7e9d-f1e1-7514a9e7263a"), "indigo", "The Football Is Good For Training And Recreational Purposes", "Practical Fresh Keyboard", 991.83m, "M" },
                    { new Guid("14027204-169d-54b0-cac7-fb08e77345ea"), "gold", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Small Wooden Computer", 989.69m, "M" },
                    { new Guid("18e919b3-66b7-4ca2-7341-61485e9514bf"), "mint green", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Tasty Soft Car", 619.17m, "M" },
                    { new Guid("1ac101b3-c4fd-6d8c-b316-c09039bd4406"), "silver", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Gorgeous Frozen Chips", 295.84m, "XL" },
                    { new Guid("1c18e188-9c48-4120-4775-ab87dee6a760"), "fuchsia", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Rustic Frozen Fish", 739.36m, "L" },
                    { new Guid("22dca47c-9b11-34a1-1a8a-754c58e0af0f"), "turquoise", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Handcrafted Rubber Pizza", 864.36m, "L" },
                    { new Guid("27b962d9-2231-6c84-41cf-986a73db82ae"), "red", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Handmade Soft Pants", 644.28m, "XL" },
                    { new Guid("2b5323a0-d0a2-5da6-c57b-92b6341e6feb"), "blue", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Handmade Granite Salad", 488.33m, "L" },
                    { new Guid("2ce404b1-d4bb-f5f9-a5ae-1b4613ae51e0"), "cyan", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Generic Metal Chicken", 210.10m, "XL" },
                    { new Guid("2fe7b459-fa53-5ada-75cb-f79cbf531b24"), "plum", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Intelligent Soft Chips", 288.67m, "M" },
                    { new Guid("3047a899-ec8f-c960-eea8-ffd79a3ed33d"), "teal", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Gorgeous Plastic Towels", 532.96m, "M" },
                    { new Guid("330b8d9b-7202-c6fa-e305-7c70ab739796"), "orchid", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Rustic Wooden Sausages", 444.84m, "L" },
                    { new Guid("34bce8ad-daf5-e9af-7921-24aa38548141"), "salmon", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Small Concrete Table", 245.50m, "XXL" },
                    { new Guid("392810f7-5ea4-56f8-ca54-89c0a86d350e"), "green", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Incredible Wooden Hat", 729.47m, "XXL" },
                    { new Guid("3b2f61f7-2959-df8d-94d4-1826ed03070f"), "azure", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Sleek Fresh Pizza", 122.27m, "S" },
                    { new Guid("3f26ebf4-e673-2a43-d1bf-ea653ed0886c"), "white", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Refined Granite Bacon", 176.60m, "XL" },
                    { new Guid("3f91270a-1d43-0fb8-3d84-97ce6aa34229"), "maroon", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Small Wooden Fish", 165.34m, "XXL" },
                    { new Guid("415d3526-e6fc-133b-5aeb-7f1267d5bccf"), "violet", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Gorgeous Concrete Soap", 958.03m, "S" },
                    { new Guid("419af2a6-e0ae-6769-39ce-dcc4acb52e0c"), "lime", "The Football Is Good For Training And Recreational Purposes", "Rustic Soft Shoes", 936.27m, "XXL" },
                    { new Guid("45e351c7-0d9a-ae41-729a-bd2b23ae80bc"), "orange", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Handcrafted Rubber Towels", 437.55m, "XL" },
                    { new Guid("46e47f64-f185-36d6-c3fd-ddfd29f2b2b0"), "gold", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Sleek Plastic Shirt", 777.71m, "M" },
                    { new Guid("47942aeb-6aa3-8681-eccc-47f5e5b31ae5"), "magenta", "The Football Is Good For Training And Recreational Purposes", "Gorgeous Plastic Computer", 539.35m, "XL" },
                    { new Guid("47ad8c75-45c3-ce20-f2b9-d1ee5f55c8fa"), "mint green", "The Football Is Good For Training And Recreational Purposes", "Licensed Steel Bacon", 37.79m, "XL" },
                    { new Guid("4958705f-2c94-15d9-f022-36585cd42300"), "teal", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Generic Frozen Table", 646.32m, "XL" },
                    { new Guid("499ea524-d820-66f4-e04e-b2b2cd9de9cd"), "silver", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Small Fresh Gloves", 738.63m, "S" },
                    { new Guid("49e95373-7520-3520-afeb-820deb5cb19f"), "lime", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Awesome Wooden Chips", 641.94m, "S" },
                    { new Guid("49f644d9-2b3a-8742-7e85-2e8540765985"), "salmon", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Fantastic Cotton Soap", 473.80m, "M" },
                    { new Guid("4a179f74-49f8-adec-cd5b-31e905aa9518"), "olive", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Rustic Cotton Pants", 778.43m, "XXL" },
                    { new Guid("4a3886ae-e523-fb6f-dcb1-0233df78dd09"), "olive", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Fantastic Cotton Chips", 105.47m, "S" },
                    { new Guid("4ccc6d12-b275-3f07-7c5d-776969fd99ad"), "teal", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Intelligent Concrete Pants", 557.53m, "XL" },
                    { new Guid("4d4218f5-c43b-f852-768f-4dced4876c48"), "mint green", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Practical Wooden Computer", 644.85m, "L" },
                    { new Guid("50944446-5207-8451-59d2-4b7c7b5dd445"), "tan", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Gorgeous Plastic Salad", 953.35m, "S" },
                    { new Guid("50cde19b-587a-6ba4-9cf9-b85d237ca12b"), "indigo", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Unbranded Wooden Fish", 78.48m, "XL" },
                    { new Guid("50dc10be-10ab-5cc9-0c5f-47c0cc1da8fa"), "tan", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Refined Steel Soap", 986.28m, "M" },
                    { new Guid("5446b652-6992-e25a-1682-954532435b26"), "lavender", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Small Frozen Computer", 839.59m, "M" },
                    { new Guid("551ba158-594f-e170-49da-e3a98db39057"), "indigo", "The Football Is Good For Training And Recreational Purposes", "Incredible Frozen Shoes", 951.53m, "L" },
                    { new Guid("56a8bf1e-349d-9454-caa4-3a43221c5c18"), "orange", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Unbranded Frozen Pizza", 100.42m, "S" },
                    { new Guid("594a740d-f249-b874-639f-a92ad5b54b37"), "violet", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Fantastic Concrete Shoes", 352.79m, "S" },
                    { new Guid("60b9738b-edff-aaa8-4158-b8478c75572b"), "salmon", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Rustic Metal Mouse", 835.13m, "L" },
                    { new Guid("62e798e6-184f-1667-9ba9-abfbaed6a7cf"), "white", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Ergonomic Frozen Keyboard", 205.19m, "L" },
                    { new Guid("658be547-1924-1d37-f142-50e32394a0d8"), "cyan", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Handcrafted Soft Table", 137.71m, "XXL" },
                    { new Guid("6909d4f7-4177-4a30-0bbb-563d3921924a"), "mint green", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Gorgeous Frozen Chair", 685.69m, "M" },
                    { new Guid("6c8276fb-5c65-7e26-5f93-9de7fb1e6089"), "black", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Handmade Plastic Pizza", 375.54m, "S" },
                    { new Guid("6d4fd904-5dfc-5ba4-8bd3-b56e7cd5e57a"), "orange", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Awesome Soft Computer", 924.52m, "XXL" },
                    { new Guid("6f1a440a-e542-bdc5-0c6c-7523d3163946"), "lime", "The Football Is Good For Training And Recreational Purposes", "Incredible Concrete Gloves", 78.74m, "L" },
                    { new Guid("71837950-2a09-f55d-9139-2a2496d87aaa"), "magenta", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Refined Steel Fish", 47.02m, "XL" },
                    { new Guid("7289f4d6-0b0d-b3d5-52e3-cb22f2d148c8"), "orchid", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Gorgeous Cotton Chips", 243.18m, "L" },
                    { new Guid("7292093c-563a-6589-ba4b-9270f840a0d5"), "maroon", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Ergonomic Plastic Chicken", 202.17m, "XXL" },
                    { new Guid("729e296d-b2e1-cb0a-6de2-850e9b3c86f8"), "pink", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Gorgeous Soft Mouse", 65.52m, "S" },
                    { new Guid("72a23fec-4aa2-987b-b6c2-2d5ae6555ff6"), "green", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Unbranded Plastic Hat", 283.54m, "M" },
                    { new Guid("73202627-1778-9e07-4289-7d7d288f84a5"), "turquoise", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Practical Fresh Chair", 657.36m, "M" },
                    { new Guid("74cde527-6a5a-af11-9d02-f6e365d8a0ee"), "mint green", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Generic Concrete Mouse", 565.89m, "XXL" },
                    { new Guid("7520f0a4-2c84-6b75-56ff-e5842e4ffd21"), "purple", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Ergonomic Cotton Shirt", 417.97m, "XL" },
                    { new Guid("79049b16-04a9-79f4-6588-198593b6dcfb"), "salmon", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Handcrafted Concrete Bike", 657.96m, "XL" },
                    { new Guid("7c1d5736-3c54-61b7-b169-e580cfd295cd"), "ivory", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Licensed Metal Computer", 778.85m, "XL" },
                    { new Guid("7f141377-01f8-edba-1bd0-2b56211eb6c4"), "turquoise", "The Football Is Good For Training And Recreational Purposes", "Generic Cotton Shirt", 790.51m, "XXL" },
                    { new Guid("80075b79-0a18-670a-1d90-de81ad0829e1"), "turquoise", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Gorgeous Rubber Table", 430.10m, "S" },
                    { new Guid("81c4566d-a2bc-600b-98b7-a829eb75dc45"), "magenta", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Licensed Fresh Bike", 932.70m, "S" },
                    { new Guid("84fc28ca-def1-f364-d0a1-7903238ebc49"), "orange", "The Football Is Good For Training And Recreational Purposes", "Practical Fresh Hat", 489.77m, "M" },
                    { new Guid("873b7a02-c0e4-f636-b650-fdaae09ead92"), "plum", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Generic Cotton Pants", 62.53m, "XXL" },
                    { new Guid("88ea458b-b014-f7d5-6c47-a76d6a39ddf6"), "mint green", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Tasty Plastic Pizza", 377.77m, "XXL" },
                    { new Guid("8933be4f-eed6-b7ca-8284-dd8f3751563b"), "yellow", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Generic Concrete Cheese", 291.06m, "M" },
                    { new Guid("8bccbb8b-5899-d3d9-243d-5ef7dfab7985"), "maroon", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Handmade Cotton Chair", 84.39m, "M" },
                    { new Guid("8c60f07a-f6a4-63f3-886c-ae6eca7a398a"), "grey", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Handcrafted Rubber Keyboard", 352.37m, "XXL" },
                    { new Guid("8e25970a-8d79-76b7-e3cd-e98e7174d0b9"), "cyan", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Incredible Rubber Pants", 10.21m, "L" },
                    { new Guid("8ee433c4-363c-8848-5436-774a263de13e"), "lime", "The Football Is Good For Training And Recreational Purposes", "Small Granite Shirt", 14.68m, "M" },
                    { new Guid("90149ffa-4ff7-74b2-f457-cd38ad28f582"), "maroon", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Refined Cotton Pants", 556.46m, "L" },
                    { new Guid("92d01daf-6070-0425-6c3f-62c615b25ab0"), "orange", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Refined Cotton Car", 151.36m, "M" },
                    { new Guid("93696d6e-8753-ebf0-9874-d0422f70e2fd"), "violet", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Small Metal Table", 120.00m, "XL" },
                    { new Guid("953a5d4b-b1a5-ce81-03f5-bd211c09a254"), "magenta", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Handcrafted Steel Chair", 546.19m, "M" },
                    { new Guid("9a68cc26-7e0a-816c-345d-efb7f36b8549"), "gold", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Small Rubber Chips", 411.56m, "S" },
                    { new Guid("9b596a6c-3e5a-8ea8-3d1b-88dc1d05ad91"), "orange", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Intelligent Fresh Hat", 948.33m, "XL" },
                    { new Guid("9d9d2138-0561-2320-d462-6c0199d59c22"), "tan", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Handcrafted Concrete Chicken", 749.94m, "XL" },
                    { new Guid("9ecb8627-8487-d626-afae-2922357b5b8e"), "fuchsia", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Licensed Fresh Chair", 957.95m, "S" },
                    { new Guid("a13cf457-a160-e073-b276-ba94ab08c4d5"), "turquoise", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Awesome Concrete Cheese", 593.44m, "XXL" },
                    { new Guid("a1895a54-1251-acbf-9539-5c1b18de8255"), "orchid", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Generic Steel Cheese", 117.34m, "M" },
                    { new Guid("a92d3cfb-ed6c-01e8-c685-46e56c4a5a11"), "sky blue", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Refined Fresh Salad", 325.06m, "L" },
                    { new Guid("a982f2cd-118f-a8b8-7185-acc374cf413e"), "white", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Generic Metal Sausages", 574.78m, "XXL" },
                    { new Guid("a9d31f71-5851-59c1-e766-f0e1ad131dce"), "mint green", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Licensed Steel Soap", 629.57m, "M" },
                    { new Guid("a9dab435-cbe7-05e0-9b06-cc7b1aab1c82"), "tan", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Fantastic Concrete Salad", 223.85m, "XL" },
                    { new Guid("aa295521-f16d-0bf5-f085-6cd6fe9206a2"), "teal", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Handmade Metal Table", 735.98m, "XL" },
                    { new Guid("af8c55f5-b040-586b-7769-553df4f90545"), "lavender", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Handmade Frozen Computer", 44.66m, "L" },
                    { new Guid("b175b1cb-5a9a-8dc7-837e-c83828900a82"), "green", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Handcrafted Wooden Chicken", 606.15m, "L" },
                    { new Guid("b4da27d8-cbb3-cb9b-52ea-98af53a56db5"), "black", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Ergonomic Frozen Soap", 12.41m, "XL" },
                    { new Guid("b8338527-a12e-240b-1ccf-ace64471e3ae"), "cyan", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Licensed Rubber Pizza", 775.11m, "M" },
                    { new Guid("b8a62ba4-28c3-d340-2399-a8a36da576f2"), "mint green", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Awesome Metal Table", 315.41m, "M" },
                    { new Guid("b9f4a4d9-3cbc-77f8-cd9a-f2b7b476dffa"), "mint green", "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design", "Awesome Cotton Tuna", 893.38m, "XXL" },
                    { new Guid("bd50f84f-f81c-7989-28ac-04e1ce280824"), "lime", "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body and 7- Color RGB LED Back-lighting for smart functionality", "Small Concrete Salad", 490.44m, "XL" },
                    { new Guid("be5ac606-f38a-1209-e71c-3929741332c9"), "indigo", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Rustic Cotton Cheese", 525.87m, "XL" },
                    { new Guid("c3a99c26-8d32-b850-33c9-cd94466907d1"), "plum", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Handcrafted Concrete Computer", 354.69m, "L" },
                    { new Guid("c4e8fc00-447e-5168-a5c5-345bac2419ff"), "grey", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Sleek Steel Bacon", 265.67m, "XL" },
                    { new Guid("c512021b-0223-2f7a-8c09-3648bde6cf82"), "lavender", "Boston's most advanced compression wear technology increases muscle oxygenation, stabilizes active muscles", "Incredible Frozen Shoes", 906.28m, "XXL" },
                    { new Guid("c78cf441-d630-3e29-8018-aaf736d6b1b8"), "orchid", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Tasty Wooden Hat", 256.53m, "M" },
                    { new Guid("ca2b850f-bd2b-ad75-cf33-3c9444e006cb"), "red", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Intelligent Soft Chicken", 924.42m, "L" },
                    { new Guid("ce5411ec-5135-cbd1-d00f-0c78dae49be4"), "azure", "Carbonite web goalkeeper gloves are ergonomically designed to give easy fit", "Practical Steel Computer", 936.63m, "L" },
                    { new Guid("cea2586a-4f70-97db-8593-30330a575cf0"), "gold", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Gorgeous Soft Hat", 423.89m, "L" },
                    { new Guid("d09dc9b5-7e87-fab2-7716-17776b732578"), "fuchsia", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Handcrafted Frozen Salad", 204.61m, "S" },
                    { new Guid("d192a71a-124a-4550-328a-4cfa0e29c2dc"), "indigo", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Incredible Frozen Tuna", 220.71m, "XXL" },
                    { new Guid("d2309c24-39eb-56cd-6c30-f3d10a60920b"), "purple", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Practical Fresh Sausages", 714.58m, "S" },
                    { new Guid("d309634f-9134-8147-41d6-a21d076501b7"), "azure", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Unbranded Rubber Mouse", 609.59m, "L" },
                    { new Guid("d3dc8f5f-679a-4c37-1496-9c948acce285"), "cyan", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Fantastic Soft Towels", 478.32m, "XL" },
                    { new Guid("e1e8bd30-fb3a-7425-6a67-f130ff9dffcf"), "blue", "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes, that started with the 1984 ABC800J", "Unbranded Wooden Sausages", 484.21m, "M" },
                    { new Guid("e9335802-9f56-deee-2157-45171f60d7a2"), "turquoise", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Unbranded Plastic Chair", 311.78m, "XL" },
                    { new Guid("ebde81c4-42f7-07c2-3681-1c28092883ea"), "magenta", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Sleek Soft Table", 97.30m, "XL" },
                    { new Guid("eee3aedb-0a27-884a-e295-c083f650e222"), "grey", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Generic Fresh Gloves", 2.33m, "L" },
                    { new Guid("efa09f39-cbe3-3eb0-18a1-7b18e1e986ac"), "magenta", "Andy shoes are designed to keeping in mind durability as well as trends, the most stylish range of shoes & sandals", "Handcrafted Steel Sausages", 253.15m, "L" },
                    { new Guid("f1e747b3-5006-af26-d4d5-036b128c1aa0"), "turquoise", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Incredible Steel Towels", 12.90m, "S" },
                    { new Guid("f441d4f6-b123-972c-ad3e-c74a98fc5973"), "violet", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Licensed Frozen Chicken", 490.58m, "M" },
                    { new Guid("f4df3ad8-885d-4ad8-ec6c-5b1a3b1f32a6"), "black", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Handcrafted Rubber Tuna", 87.96m, "XXL" },
                    { new Guid("f7393609-515d-b63b-7d2c-93387d25bf82"), "white", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Sleek Steel Bacon", 6.73m, "S" },
                    { new Guid("f9740d3d-0f32-c582-4dc1-09c8c94b1563"), "indigo", "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients", "Generic Metal Bacon", 518.36m, "L" },
                    { new Guid("fba05aba-0950-7331-29ae-daa63c079a07"), "orange", "New ABC 13 9370, 13.3, 5th Gen CoreA5-8250U, 8GB RAM, 256GB SSD, power UHD Graphics, OS 10 Home, OS Office A & J 2016", "Gorgeous Frozen Towels", 905.11m, "XXL" },
                    { new Guid("fc936fc1-c88b-d7ad-384a-0d062ba6b4a1"), "olive", "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support", "Rustic Plastic Salad", 312.43m, "XXL" },
                    { new Guid("fd39cebb-52fb-bb34-9ba1-656528833b92"), "mint green", "The automobile layout consists of a front-engine design, with transaxle-type transmissions mounted at the rear of the engine and four wheel drive", "Awesome Metal Computer", 738.89m, "S" },
                    { new Guid("ff36d574-ac19-1dd0-50f1-1fc9f5911cab"), "teal", "New range of formal shirts are designed keeping you in mind. With fits and styling that will make you stand apart", "Licensed Metal Chips", 364.67m, "XL" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
