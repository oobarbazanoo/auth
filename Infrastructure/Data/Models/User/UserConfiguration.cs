using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Models.User
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Login);

            builder.Property(u => u.Login)
                .IsRequired();

            builder.Property(u => u.Salt)
                .IsRequired();

            builder.Property(u => u.Hash)
                .IsRequired();
        }
    }
}