using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using FoodOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "soup",
                Description = "Descr..",
            },
            new Category()
            {
                Id = 2,
                Name = "salad",
                Description = "Descr..",
            },
            new Category()
            {
                Id = 3,
                Name = "Main dishes",
                Description = "Descr..",
            },
            new Category()
            {
                Id = 4,
                Name = "Drinks",
                Description = "Descr..",
            }
            );

            modelBuilder.Entity<Item>().HasData(
            new Item()
            {
                Id = 1,
                Name = "Item1",
                Description = "Descr..",
                Price = 10,
                CategoryId = 1

            },
            new Item()
            {
                Id = 2,
                Name = "Item2",
                Description = "Descr..",
                Price = 20,
                CategoryId = 1

            },
            new Item()
            {
                Id = 3,
                Name = "Item3",
                Description = "Descr..",
                Price = 30,
                CategoryId = 2
            }
            );

            modelBuilder.Entity<User>().HasData(
            new User()
            {
                Id = 1,
                UserName = "admin",
                Name = "Admin",
                Email = "admin@email.com",
                Password = "password",
                Role = "admin"
            },
            new User()
            {
                Id = 2,
                UserName = "user1",
                Name = "customer 1",
                Email = "customer1@email.com",
                Password = "pw1",
                Role = "customer"
            },
            new User()
            {
                Id = 3,
                UserName = "user2",
                Name = "customer 2",
                Email = "customer2@email.com",
                Password = "pw2",
                Role = "customer"
            }
            );

            modelBuilder.Entity<Address>().HasData(
            new Address()
            {
                Id = 1,
                Details = "1250 Debar",
                Name = "Ev",
                UserId = 2
            },
            new Address()
            {
                Id = 2,
                Details = "1000, Skopje",
                Name = "Home",
                UserId = 3
            }
            );

            modelBuilder.Entity<OrderHeader>().HasData(
            new OrderHeader()
            {
                Id = 1,
                UserId = 2,
                AddressId = 1,
                Status = 1,
                Notes = "Cold coca-cola",
                DateTime = DateTime.Now,

            },
            new OrderHeader()
            {
                Id = 2,
                UserId = 3,
                AddressId = 2,
                Status = 1,
                Notes = ".",
                DateTime = DateTime.Now,

            }
            );

            modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem()
            {
                Id = 1,
                ItemId = 1,
                Quantity = 2,
                OrderHeaderId = 1
            },
            new OrderItem()
            {
                Id = 2,
                ItemId = 2,
                Quantity = 1,
                OrderHeaderId = 1
            },
            new OrderItem()
            {
                Id = 3,
                ItemId = 2,
                Quantity = 3,
                OrderHeaderId = 2
            },
            new OrderItem()
            {
                Id = 4,
                ItemId = 3,
                Quantity = 1,
                OrderHeaderId = 1
            },
            new OrderItem()
            {
                Id = 5,
                ItemId = 3,
                Quantity = 1,
                OrderHeaderId = 2
            }
            );



        }
    }
}
