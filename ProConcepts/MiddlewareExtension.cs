using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProConcepts
{
    public static class MiddlewareExtension
    {
        public static void MyExtensionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware>();
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"), handleBranchApp =>
            {
                handleBranchApp.Run(async context =>
                {
                    await context.Response.WriteAsync("Branch Map When \n");
                });
            });
            //Handle Multiple Segements
            app.Map("/Map1/Seg1", HandleMultiSegApp =>
            {
                HandleMultiSegApp.Run(async context =>
                {
                    await context.Response.WriteAsync("Map multi seg \n");
                });
            });

            app.Map("/MainMapPath", MainApp =>
            {
                MainApp.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Main \n");
                    await next.Invoke();
                });
                MainApp.Map("/NestedMapPath", NestedApp =>
                {
                    NestedApp.Use(async (context, next) =>
                    {
                        await context.Response.WriteAsync("Nested \n");
                        await next.Invoke();
                    });
                });
            });

            app.Map("/map1", mapappbuilder => {
                mapappbuilder.Run(async context =>
                {
                    await context.Response.WriteAsync("Map1 \n");
                });
            });
            app.UseWhen(context => context.Request.Query.ContainsKey("branch"), myconappbuilder =>
            {
                myconappbuilder.Use(async (context, next) => {
                    await context.Response.WriteAsync("11 \n");
                    await next.Invoke();
                    await context.Response.WriteAsync("55 \n");
                });
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("1 \n");
                await next.Invoke();
                await context.Response.WriteAsync("5 \n");
            });
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("2 \n");
                await next.Invoke();
                await context.Response.WriteAsync("4 \n");
            });
            app.Run(async context =>
            {
                await context.Response.WriteAsync("3 \n");
            });
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Called from Run Middleware2 \n");
            });
        }
    }
}
