using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) 
            : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SubjectUserJoin> SubjectsUsers { get; set; }
        public DbSet<DepartmentUserJoin> DepartmentsUsers { get; set; }
        public DbSet<DepartmentSubjectJoin> DepartmentsSubjects { get; set; }
        public DbSet<SubjectNotificationJoin> SubjectsNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // SubjectsUsers
            modelBuilder.Entity<SubjectUserJoin>()
                .HasKey(su => new { su.SubjectId, su.UserId });

            modelBuilder.Entity<SubjectUserJoin>()
                .HasOne(su => su.User)
                .WithMany(u => u.SubjectsUsers)
                .HasForeignKey(su => su.UserId);

            modelBuilder.Entity<SubjectUserJoin>()
                .HasOne(su => su.Subject)
                .WithMany(s => s.SubjectsUsers)
                .HasForeignKey(su => su.SubjectId);


            // DepartmentUser Joins
            modelBuilder.Entity<DepartmentUserJoin>()
                .HasKey(du => new { du.DepartmentId, du.UserId });

            modelBuilder.Entity<DepartmentUserJoin>()
                .HasOne(du => du.User)
                .WithMany(u => u.DepartmentsUsers)
                .HasForeignKey(du => du.UserId);

            modelBuilder.Entity<DepartmentUserJoin>()
                .HasOne(du => du.Department)
                .WithMany(d => d.DepartmentsUsers)
                .HasForeignKey(du => du.DepartmentId);


            // DepartmentsSubjects Joins
            modelBuilder.Entity<DepartmentSubjectJoin>()
                .HasKey(ds => new { ds.DepartmentId, ds.SubjectId });

            modelBuilder.Entity<DepartmentSubjectJoin>()
                .HasOne(ds => ds.Subject)
                .WithMany(s => s.DepartmentsSubjects)
                .HasForeignKey(ds => ds.SubjectId);

            modelBuilder.Entity<DepartmentSubjectJoin>()
                .HasOne(ds => ds.Department)
                .WithMany(d => d.DepartmentsSubjects)
                .HasForeignKey(ds => ds.DepartmentId);


            // User and Notification relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.SenderId)
                .OnDelete(DeleteBehavior.Cascade);


            // Subject and Notification relationships
            modelBuilder.Entity<SubjectNotificationJoin>()
                .HasKey(sn => new { sn.SubjectId, sn.NotificationId });

            modelBuilder.Entity<SubjectNotificationJoin>()
                .HasOne(sn => sn.Subject)
                .WithMany(n => n.SubjectsNotifications)
                .HasForeignKey(sn => sn.SubjectId);

            modelBuilder.Entity<SubjectNotificationJoin>()
                .HasOne(sn => sn.Notification)
                .WithMany(n => n.SubjectsNotifications)
                .HasForeignKey(sn => sn.NotificationId);
        }

    }
}
