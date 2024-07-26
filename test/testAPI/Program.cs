using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using testAPI.Constants;
using testAPI.Data;
using testAPI.Helpers;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register the DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Register IUserService and its implementation UserService
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IExamRegistrationService, ExamRegistrationService>();
builder.Services.AddScoped<IActivityTypeService, ActivityTypeService>();
builder.Services.AddScoped<ISubjectActivityService, SubjectActivityService>();
builder.Services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
builder.Services.AddScoped<RoleLogic>();
builder.Services.AddScoped<SubjectLogic>();
builder.Services.AddScoped<NotificationLogic>();
builder.Services.AddScoped<GradeLogic>();
builder.Services.AddScoped<ExamLogic>();
builder.Services.AddScoped<ExamRegistrationLogic>();
builder.Services.AddScoped<ActivityTypeLogic>();
builder.Services.AddScoped<SubjectActivityLogic>();
builder.Services.AddScoped<StudentAttendanceLogic>();
builder.Services.AddScoped<UserHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
