using Ecommerce.Dtos.Cart;
using Ecommerce.Dtos.Products;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Models.Cart;
using Ecommerce.Models.Order;
using Ecommerce.Models.Product;
using Ecommerce.Models.User;
using Ecommerce.Models.User.AboutAndContact;
using Ecommerce.Models.User.FavAndBooking;
using Ecommerce.Models.User.ServicesAndTutorials;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SpecialOffersModel> SpecialOffers { get; set; }
        public DbSet<FaveoriteModel> Faveorites { get; set; }
        public DbSet<FaveoriteItemModel> FaveoriteItems { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<ReviewsModel> Reviews { get; set; }
        public DbSet<AboutModel> Abouts { get; set; }
        public DbSet<GetinTouchModel> ContactUs { get; set; }
        public DbSet<TutorialsModel> Tutorials { get; set; }
        public DbSet<MaintenanceServicesModel> MaintenanceServices { get; set; }
        public DbSet<AvaliableDatesModel> AvaliableDates { get; set; }
        public DbSet<CheckOutModel> CheckOuts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "Security");
            builder.Entity<IdentityRole>().ToTable("Roles", "Security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");

            builder.Entity<CategoryModel>().HasIndex(m => m.Name).IsUnique();
            builder.Entity<ProductModel>().HasIndex(m => m.Name).IsUnique();

            builder.Entity<CartItemModel>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItemModel>()
                .HasOne(ci => ci.OrderItem)
                .WithOne(oi => oi.CartItem)
                .HasForeignKey<OrderItemModel>(oi => oi.CartItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartModel>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<CartModel>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Faveorites)
                .WithOne(f => f.User)
                .HasForeignKey<FaveoriteModel>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.CheckOuts)
                .WithOne(ch => ch.User)
                .HasForeignKey(ch => ch.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductModel>()
                .HasMany(p => p.cartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductModel>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductModel>()
                .HasOne(p => p.SpecialOffer)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SpecialOfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MaintenanceServicesModel>()
                .HasOne(m => m.Tutorial)
                .WithOne(t => t.RelatedService)
                .HasForeignKey<TutorialsModel>(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductModel>(e =>
            {
                e.Property(p => p.Price).HasPrecision(18, 2);
            });

            builder.Entity<SpecialOffersModel>(e =>
            {
                e.Property(o => o.DiscountPercentage).HasPrecision(5, 2);
            });

            builder.Entity<MaintenanceServicesModel>(e =>
            {
                e.Property(m => m.Price).HasPrecision(18, 2);
            });
        }
    }
}
