using EFDbFactory.Sql;
using Microsoft.EntityFrameworkCore;

namespace AspCoreApi
{
    public partial class BooksDbContext : EFDbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookRating> BookRating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(256);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.HasOne(d => d.Author)
                      .WithMany(p => p.Book)
                      .HasForeignKey(d => d.AuthorId)
                      .HasConstraintName("FK_Author_Book");
            });

            modelBuilder.Entity<BookRating>(entity => entity.HasOne(d => d.Book)
                                                            .WithMany(p => p.BookRating)
                                                            .HasForeignKey(d => d.BookId)
                                                            .HasConstraintName("FK_BookRating_Book"));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
