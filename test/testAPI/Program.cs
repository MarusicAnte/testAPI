using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Dodajte podršku za JWT Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put your Bearer token here. Example: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});


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
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<RoleLogic>();
builder.Services.AddScoped<SubjectLogic>();
builder.Services.AddScoped<NotificationLogic>();
builder.Services.AddScoped<GradeLogic>();
builder.Services.AddScoped<ExamLogic>();
builder.Services.AddScoped<ExamRegistrationLogic>();
builder.Services.AddScoped<ActivityTypeLogic>();
builder.Services.AddScoped<SubjectActivityLogic>();
builder.Services.AddScoped<ScheduleLogic>();
builder.Services.AddScoped<StudentAttendanceLogic>();
builder.Services.AddScoped<UserHelper>();


// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPermission", policy => policy.RequireRole(RolesConstant.Administrator));
    options.AddPolicy("StudentPermission", policy => policy.RequireRole(RolesConstant.Student));
    options.AddPolicy("ProfessorPermission", policy => policy.RequireRole(RolesConstant.Profesor));
    options.AddPolicy("ProfessorPermission", policy => policy.RequireRole(RolesConstant.Profesor));
    options.AddPolicy("AsistantPermission", policy => policy.RequireRole(RolesConstant.Asistent));
    options.AddPolicy("Admin/Professor/Asistant", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.IsInRole(RolesConstant.Administrator)||
            context.User.IsInRole(RolesConstant.Profesor) ||
            context.User.IsInRole(RolesConstant.Asistent)
        );
    });
    options.AddPolicy("AnyUserRole", policy =>
    {
        policy.RequireAssertion(context =>
            context.User.IsInRole(RolesConstant.Administrator) ||
            context.User.IsInRole(RolesConstant.Profesor) ||
            context.User.IsInRole(RolesConstant.Student) ||
            context.User.IsInRole(RolesConstant.Asistent)
        );
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
