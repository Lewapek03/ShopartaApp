using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoparta.Models;

namespace Shoparta.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
            public DbSet<Category> Categories {  get; set; }
            public DbSet<ShoppingCart> ShoppingCarts {  get; set; }
            public DbSet<Item> Items {  get; set; }
            public DbSet<Order> Orders {  get; set; }
            public DbSet<OrderDetail> OrderDetails {  get; set; }
            public DbSet<OrderStatus> OrderStatuses {  get; set; }
            public DbSet<CardDetail> CardDetail {  get; set; }
            
        
    }
}