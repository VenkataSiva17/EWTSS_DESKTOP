using System;
using System.Collections.Generic;
using System.Linq;
using EWTSS_DESKTOP.Core.Interfaces;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class UserManagementService
    {
        private readonly IUserManagementRepository _userRepository;
        private readonly AppDbContext _context;

        public UserManagementService(IUserManagementRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public bool CreateUser(string firstName, string lastName, string userName, string password, string roleName)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("Required fields are missing.");
            }

            var existingUser = _userRepository.GetByUserName(userName);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                throw new Exception("Selected role not found.");
            }

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Password = password,
                RoleId = role.Id,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                IsActive = true
            };

            return _userRepository.CreateUser(user);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public bool UpdateUser(int id, string firstName, string lastName, string userName, string roleName)
        {
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(roleName))
            {
                throw new Exception("Required fields are missing.");
            }

            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var duplicateUser = _context.Users
                                        .FirstOrDefault(u => u.UserName == userName && u.Id != id && u.IsActive);
            if (duplicateUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                throw new Exception("Selected role not found.");
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserName = userName;
            user.RoleId = role.Id;
            user.UpdatedOn = DateTime.Now;

            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return _userRepository.DeleteUser(user);
        }

        public bool ResetPassword(int id, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new Exception("Password cannot be empty.");
            }

            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Password = newPassword;
            user.UpdatedOn = DateTime.Now;

            return _userRepository.UpdateUser(user);
        }

        public List<Feature> GetAllFeatures()
        {
            return _context.Features
                           .OrderBy(f => f.Id)
                           .ToList();
        }

        public List<int> GetUserPermissionFeatureIds(int userId)
        {
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.Id == userId && u.IsActive);

            if (user == null)
                throw new Exception("User not found.");

            return _context.RolePermissions
                           .Where(rp => rp.RoleId == user.RoleId)
                           .Select(rp => rp.FeatureId)
                           .ToList();
        }

        public bool SaveUserPermissions(int userId, List<int> featureIds)
        {
            var user = _context.Users
                               .Include(u => u.Role)
                               .FirstOrDefault(u => u.Id == userId && u.IsActive);

            if (user == null)
                throw new Exception("User not found.");

            var roleId = user.RoleId;

            var oldPermissions = _context.RolePermissions
                                         .Where(rp => rp.RoleId == roleId)
                                         .ToList();

            _context.RolePermissions.RemoveRange(oldPermissions);

            foreach (var featureId in featureIds.Distinct())
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    FeatureId = featureId
                });
            }

            return _context.SaveChanges() > 0;
        }
    }
}