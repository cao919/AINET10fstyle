using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Zilor.AICopilot.EntityFrameworkCore.Repository;
using Zilor.AICopilot.Services.Common.Contracts;
using Zilor.AICopilot.SharedKernel.Repository;

namespace Zilor.AICopilot.EntityFrameworkCore;

public static class DependencyInjection
{
    public static void AddEfCore(this IHostApplicationBuilder builder)
    {
      builder.AddNpgsqlDbContext<AiCopilotDbContext>("ai-copilot");
        // 替换 Aspire 的 AddNpgsqlDbContext 为原生 AddDbContext
        //builder.Services.AddDbContext<AiCopilotDbContext>((sp, options) =>
        //{
        //    // 读取连接字符串（和 Aspire 用同一个配置键）
        //    var connectionString = builder.Configuration.GetConnectionString("ai-copilot");
        //    // 注册 PostgreSQL 驱动，无自动建库逻辑
        //    options.UseNpgsql(connectionString);

        //    // 可选：添加 EF Core 额外配置
        //    options.EnableSensitiveDataLogging();
        //});
         



        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped<IDataQueryService, DataQueryService>();

        builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AiCopilotDbContext>();
    }
}