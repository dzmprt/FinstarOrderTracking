using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FOT.DatabaseInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                migrationBuilder.AlterDatabase()
                    .Annotation("Npgsql:PostgresExtension:citext", ",,")
                    .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");
            }

            migrationBuilder.CreateTable(
                name: "DomainEventOutboxNotifications",
                columns: table => new
                {
                    DomainEventOutboxId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AggregateType = table.Column<string>(type: "citext", maxLength: 1000, nullable: false),
                    AggregateId = table.Column<string>(type: "citext", maxLength: 1000, nullable: false),
                    EventCode = table.Column<string>(type: "citext", maxLength: 1000, nullable: false),
                    Payload = table.Column<string>(type: "citext", maxLength: 10000, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEventOutboxNotifications", x => x.DomainEventOutboxId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderNumber = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "citext", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderNumber);
                });
            
            var random = new Random();
            var products = new[]
            {
                "Wireless mouse", "Mechanical keyboard", "USB-C hub", "Gaming monitor",
                "Bluetooth speaker", "External SSD", "Webcam", "Noise-cancelling headphones",
                "Smartwatch", "Laptop stand", "Ergonomic chair", "Graphic tablet",
                "Portable charger", "LED desk lamp", "Microphone", "VR headset",
                "Stylus pen", "HDMI cable", "Wi-Fi router", "Laptop backpack"
            };

            var values = new object[100, 5];

            for (int i = 0; i < 100; i++)
            {
                var guid = Guid.NewGuid();

                var year = random.Next(2023, 2026);
                var month = random.Next(1, 13);
                var day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
                var hour = random.Next(0, 24);
                var minute = random.Next(0, 60);
                var second = random.Next(0, 60);

                var dateTime = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero).UtcDateTime;
                var description = products[random.Next(products.Length)];
                var status = i % 4 + 1; 

                values[i, 0] = guid;
                values[i, 1] = dateTime;
                values[i, 2] = null;
                values[i, 3] = description;
                values[i, 4] = status;
            }

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderNumber", "CreatedAt", "UpdatedAt", "Description", "Status" },
                values: values
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEventOutboxNotifications");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
