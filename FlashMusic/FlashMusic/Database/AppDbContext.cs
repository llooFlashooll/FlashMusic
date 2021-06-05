using FlashMusic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlashMusic.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<History> History { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=119.3.254.34; port=3306; Database=flashmusic_db; user id=root; password=123456;");
            }
        }

        // 此处可以添加种子数据 或 指明model的主外键
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // var productJson = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
            //     @"/Database/ProductMockData.json");
            // IList<Product> product = JsonConvert.DeserializeObject<IList<Product>>(productJson);
            // modelBuilder.Entity<Product>().HasData(product);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("productid")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.CategoryId).HasColumnName("categoryid");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.PicUrl).HasColumnName("picurl");

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userid");

                entity.Property(e => e.UserName).HasColumnName("username");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Avatar).HasColumnName("avatar");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.HasKey(e => e.UserId);

                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userid");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("productid")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.State).HasColumnName("state");

            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("history");

                entity.HasKey(e => e.UserId);

                entity.HasKey(e => e.ProductId);

                entity.HasKey(e => e.PayTime);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userid");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasColumnName("productid")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.PayTime)
                    .IsRequired()
                    .HasColumnName("paytime");

                entity.Property(e => e.Num).HasColumnName("num");
            });
        }
    }
}
