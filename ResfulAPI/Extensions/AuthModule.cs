using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyProject.Helper.ModelHelps;
using System.Net;
using System.Text;

namespace NekBigCore.Helpers.Module
{
    public static class AuthModule
    {

        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            JwtSettings jwtSettings)
        {
            services
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "True");
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        //private static IPermissionService GetService(Type type)
        //{
        //    throw new NotImplementedException();
        //}

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseSession();

            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }

    }
}
