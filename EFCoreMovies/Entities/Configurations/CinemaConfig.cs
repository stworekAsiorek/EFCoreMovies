using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EFCoreMovies.Entities.Configurations
{
    public class CinemaConfig : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.Property(p => p.Name).IsRequired();

            builder.HasOne(c => c.CinemaOffer).WithOne().HasForeignKey<CinemaOffer>(co => co.CinemaId);

            builder.HasMany(c => c.CinemaHalls).WithOne(ch => ch.Cinema)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(ch => ch.TheCinemaId);

            builder.HasOne(c => c.CinemaDetail).WithOne(ch => ch.Cinema).HasForeignKey<CinemaDetail>(cd => cd.Id);

            builder.OwnsOne(c => c.Address, add =>
            {
                add.Property(p => p.Street).HasColumnName("Street");
                add.Property(p => p.Province).HasColumnName("Province");
                add.Property(p => p.County).HasColumnName("Country");
            });
        }
    }
}