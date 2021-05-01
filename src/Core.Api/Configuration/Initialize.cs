using Core.Api.Data;
using Core.Business.Models;
using Core.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Api.Configuration
{
    public static class Initialize
    {
        public static async Task SeedUserAdmin(IList<string> roles, IConfiguration configuration, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            var list = roles;
            var logger = loggerFactory.CreateLogger("UserAdminDatabase");

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!dbContext.Users.Any(x => x.UserName.Equals(configuration["AppUserAdmin:UserName"])))
                {
                    logger.LogDebug("-----------------------------------Begin Add User Admin automatic-----------------------------------");

                    try
                    {
                        RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                        UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                        var user = new IdentityUser { UserName = configuration["AppUserAdmin:UserName"], Email = configuration["AppUserAdmin:Email"], EmailConfirmed = Convert.ToBoolean(configuration["AppUserAdmin:EmailConfirmed"]) };
                        var result = await userManager.CreateAsync(user, configuration["AppUserAdmin:Password"]);
                        if (result.Succeeded)
                        {
                            if (!roleManager.RoleExistsAsync(configuration["AppUserAdmin:Role"]).Result)
                            {
                                IdentityRole role = new IdentityRole();
                                role.Name = configuration["AppUserAdmin:Role"];
                                roleManager.CreateAsync(role).Wait();
                            }
                            //add Role
                            userManager.AddToRoleAsync(user, configuration["AppUserAdmin:Role"]).Wait();

                            //add Claim
                            var claims = configuration.GetSection("AppSettings:ClaimsListAdmin").Get<Dictionary<string, string[]>>();
                            foreach (var claim in claims)
                            {
                                foreach (var item in claim.Value)
                                {
                                    await userManager.AddClaimAsync(user, new Claim(claim.Key, item));
                                }
                            }

                            logger.LogDebug("User admin created a new account with password and add to a role " + configuration["AppUserAdmin:Role"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error: " + ex.Message);
                    }

                    logger.LogDebug("------------------------------------End Add User Admin automatic------------------------------------");
                }
            }
        }

        public static async Task SeedControllersActions(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var logger = loggerFactory.CreateLogger("Claims");

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetService<MyDbContext>())
                {
                    var claims = configuration.GetSection("ClaimsList").Get<Dictionary<string, string[]>>();
                    UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();

                    logger.LogInformation("-----------------------------------Begin Add Controller action automatic-----------------------------------");

                    try
                    {
                        var user = userManager.Users.FirstOrDefault(u => u.UserName.Equals(configuration["AppUserAdmin:UserName"]));
                        var ctrl = (AppController)null;

                        int nControllerDb = dbContext.Controllers.Count();
                        int nActionDb = dbContext.Actions.Count();


                        foreach (var controller in claims)
                        {
                            var existsController = dbContext.Controllers.Where(c => c.ControllerName.Equals(controller.Key)).FirstOrDefault();

                            if (existsController == null)
                            {
                                ctrl = new AppController { ControllerName = controller.Key, CreatedDate = DateTime.Now, CreatedBy = user.Id };
                                dbContext.Add(ctrl);
                                await dbContext.SaveChangesAsync();
                                existsController = ctrl;
                            }

                            foreach (var action in controller.Value)
                            {
                                bool existsAction = dbContext.Actions.Where(c => c.ActionName.Equals(action) && c.ControllerId == existsController.Id).Any();

                                if (existsAction == false && existsController.Id != null)
                                {
                                    var a = new AppAction { ActionName = action, ControllerId = existsController.Id, CreatedDate = DateTime.Now, CreatedBy = user.Id };
                                    dbContext.Add(a);
                                    await dbContext.SaveChangesAsync();
                                }

                                if (existsAction == false && existsController.Id == null)
                                {
                                    var a = new AppAction { ActionName = action, ControllerId = ctrl.Id, CreatedDate = DateTime.Now, CreatedBy = user.Id };
                                    dbContext.Add(a);
                                    await dbContext.SaveChangesAsync();
                                }

                                if (existsAction == true && existsController.Id == null)
                                {
                                    var a = new AppAction { ActionName = action, ControllerId = existsController.Id, CreatedDate = DateTime.Now, CreatedBy = user.Id };
                                    dbContext.Remove(a);
                                    await dbContext.SaveChangesAsync();
                                }

                            }

                        }

                        logger.LogInformation("controller action adicionados com sucesso.");
                    }

                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error: " + ex.Message);
                    }

                    logger.LogInformation("------------------------------------End Add Controller action automatic------------------------------------");
                }
            }
        }
    }
}
