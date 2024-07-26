using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<SubjectActivity> SubjectActivities { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<ExamRegistration> ExamRegistrations { get; set; }
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


            // Grades
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Grade>()
                .HasIndex(g => new { g.StudentId, g.SubjectId })
                .IsUnique();

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.SubjectId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(u => u.StudentGrades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Professor)
                .WithMany(p => p.ProfessorGrades)
                .HasForeignKey(g => g.ProfessorId)
                .OnDelete(DeleteBehavior.NoAction);


            // Exams
            modelBuilder.Entity<Exam>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Classroom)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Professor)
                .WithMany(p => p.Exams)
                .HasForeignKey(e => e.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);


            // ExamRegistrations
            modelBuilder.Entity<ExamRegistration>()
                .HasKey(er => er.Id);

            modelBuilder.Entity<ExamRegistration>()
                .HasOne(er => er.Student)
                .WithMany(s => s.ExamRegistrations)
                .HasForeignKey(er => er.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamRegistration>()
                .HasOne(er => er.Exam)
                .WithMany(e => e.ExamRegistrations)
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamRegistration>()
                .HasIndex(er => er.StudentId);

            modelBuilder.Entity<ExamRegistration>()
                .HasIndex(er => er.ExamId);


            // SubjectActivities
            modelBuilder.Entity<SubjectActivity>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<SubjectActivity>()
                .HasOne(sa => sa.Subject)
                .WithMany(s => s.SubjectActivities)
                .HasForeignKey(sa => sa.SubjectId);

            modelBuilder.Entity<SubjectActivity>()
                .HasOne(sa => sa.ActivityType)
                .WithMany()
                .HasForeignKey(sa => sa.ActivityTypeId);

            modelBuilder.Entity<SubjectActivity>()
                .HasOne(sa => sa.Classroom)
                .WithMany(c => c.SubjectActivities)
                .HasForeignKey(sa => sa.ClassroomId);

            modelBuilder.Entity<SubjectActivity>()
                .HasOne(sa => sa.Instructor)
                .WithMany(i => i.SubjectActivities)
                .HasForeignKey(sa => sa.InstructorId);


            // StudentAttendances
            modelBuilder.Entity<StudentAttendance>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<StudentAttendance>()
                .HasOne(st => st.Student)
                .WithMany(s => s.StudentAttendances)
                .HasForeignKey(st => st.StudentId);

            modelBuilder.Entity<StudentAttendance>()
                .HasOne(st => st.SubjectActivity)
                .WithMany(sa => sa.StudentAttendances)
                .HasForeignKey(st => st.SubjectActivityId);
        }

    }
}
