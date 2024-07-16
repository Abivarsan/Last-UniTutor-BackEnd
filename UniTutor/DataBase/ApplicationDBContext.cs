
using Microsoft.EntityFrameworkCore;
using UniTutor.Model;

namespace UniTutor.DataBase
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and properties for each entity

            // Example configuration for Student entity
            modelBuilder.Entity<Student>()
                .HasKey(s => s._id); // Assuming Id is the primary key

            // Example configuration for Tutor entity
            modelBuilder.Entity<Tutor>()
                .HasKey(t => t._id); // Assuming Id is the primary key

            // Example configuration for Comment entity
            modelBuilder.Entity<Comment>()
                .HasKey(c => c._id); // Assuming Id is the primary key

            // Example configuration for Transaction entity
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id); // Assuming Id is the primary key
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Description)
                .IsRequired(); // Example of required property

            // Configure relationships between entities
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Comments)
                .WithOne(c => c.Student)
                .HasForeignKey(c => c.stuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tutor>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Tutor)
                .HasForeignKey(c => c.tutId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Request entity relationship
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Tutor)
                .WithMany(t => t.Requests)
                .HasForeignKey(r => r.tutorId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction as needed

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Tutor)
                .WithMany(tu => tu.Transactions)
                .HasForeignKey(t => t.tutorId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the behavior here
                                                    // Configure relationships and other configurations
           


            // Additional configurations for other relationships...

            base.OnModelCreating(modelBuilder);
        }
    }
}







