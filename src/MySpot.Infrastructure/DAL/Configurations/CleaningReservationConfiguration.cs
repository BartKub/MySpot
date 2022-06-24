using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

internal sealed class CleaningReservationConfiguration : IEntityTypeConfiguration<CleaningReservation>
{
    public void Configure(EntityTypeBuilder<CleaningReservation> builder)
    {
    }
}