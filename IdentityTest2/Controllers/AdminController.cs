﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace IdentityTest2.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext context;


        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        public async System.Threading.Tasks.Task<ActionResult> Index(string sortOrder, int? page)
        {

            ViewBag.DateSortParm = sortOrder == "ID" ? "Time" : "ID";
            var User_model = from u in context.Users select u;
            var all_users = User_model.ToList();
            //var myRoleStore = new CustomRoleStore(context);
            //var roleManager = new ApplicationRoleManager(myRoleStore);

            List<UsersAndRoles> users_roles = new List<UsersAndRoles>();
            //foreach (var user in User_model.ToList())
            //{
            //    List<String> rolelist = new List<String>();

            //    if (user.Roles.Count == 0)
            //    {
            //        rolelist.Add("member");
            //    }
            //    else
            //    {
            //        foreach (var role in user.Roles)
            //        {
            //            CustomRole _role = await roleManager.FindByIdAsync(role.RoleId);
            //            rolelist.Add(_role.Name);
            //        }
            //    }

            //    users_roles.Add(new UsersAndRoles(rolelist,user.UserPhoto,user.UserName,user.Email));

            //}

            users_roles = await getUsersWithRoles(User_model.ToList());
            ViewBag.message = "All Users and their Roles";

            return View(users_roles.ToList().ToPagedList(page ?? 1, 10));
        }
        public async System.Threading.Tasks.Task<List<UsersAndRoles>> getUsersWithRoles(List<ApplicationUser> userList)
        {

            var myRoleStore = new CustomRoleStore(context);
            var roleManager = new ApplicationRoleManager(myRoleStore);

            List<UsersAndRoles> users_roles = new List<UsersAndRoles>();
            foreach (var user in userList)
            {
                List<String> rolelist = new List<String>();

                if (user.Roles.Count == 0)
                {
                    rolelist.Add("member");
                }
                else
                {
                    foreach (var role in user.Roles)
                    {
                        CustomRole _role = await roleManager.FindByIdAsync(role.RoleId);
                        rolelist.Add(_role.Name);
                    }
                }

                users_roles.Add(new UsersAndRoles(rolelist, user.UserPhoto, user.UserName, user.Email));
            }

            return users_roles;

        }
        [Authorize(Roles = "admin")]
        public ActionResult CreateRole(String n) {

            ViewBag.roleId = new SelectList(context.Roles, "roleId", "roleName");
            
            return View();
        }

        [AllowAnonymous]
        public async System.Threading.Tasks.Task<ActionResult> SearchForUsers(string searchString, int? page)
        {

            var searchedUsers = from s in context.Users
                                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchedUsers = searchedUsers.Where(n => n.UserName.Contains(searchString));
            }

            List<UsersAndRoles> searchUsers_n_Roles = await getUsersWithRoles(searchedUsers.ToList());
            //return View(searchedNews);
            return PartialView(searchUsers_n_Roles.ToPagedList(page ?? 1, 10));
        }
    }
}