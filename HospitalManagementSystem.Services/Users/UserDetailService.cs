using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.Models.Categories;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace HospitalManagementSystem.Services.Users
{
    public class UserDetailService : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public UserDetailService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Response CreateAsync(CreateUserDetailsDto createUserDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(createUserDetailsDto);

            string password = Encrypt(userDetails.Password);
            userDetails.Id = Guid.NewGuid();
            userDetails.CreatedDate = DateTime.Now;
            userDetails.Password = password;
            _dbContext.UserMaster.Add(userDetails);
            _dbContext.SaveChanges();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetUsersDetailsDto> Get(Guid id)
        {
            var data = await _dbContext.UserMaster.FindAsync(id);
            var userDetail = _mapper.Map<GetUsersDetailsDto>(data);
            return userDetail;
        }

        public async Task<GetUsersDetailsDto> GetByEmail(string emailId)
        {
            var data = await _dbContext.UserMaster.Where(x => x.Email == emailId).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetUsersDetailsDto>(data);
            return userDetail;
        }

        public async Task<GetUsersDetailsDto> GetByMobile(string mobile)
        {
            var data = await _dbContext.UserMaster.Where(x=>x.MobileNo==mobile).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetUsersDetailsDto>(data);
            return userDetail;
        }

        public async Task<GetUsersDetailsDto> GetByName(string name)
        {
            var data = await _dbContext.UserMaster.Where(x => x.FirstName == name).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetUsersDetailsDto>(data);
            return userDetail;
        }

        public async Task<List<GetUsersDetailsDto>> GetList()
        {
            var data = await _dbContext.UserMaster.ToListAsync();
            var userDetail = _mapper.Map<List<GetUsersDetailsDto>>(data);
            return userDetail;
        }

        public async Task<List<GetUsersDetailsDto>> GetActiveUserList()
        {
            var data = await _dbContext.UserMaster.Where(x=>x.IsActive==true).ToListAsync();
            var userDetail = _mapper.Map<List<GetUsersDetailsDto>>(data);
            return userDetail;
        }

        public Response UpdateUser(Guid id, UpdateUserDetailsDto updateUserDetailsDto)
        {
            var userDetails = _mapper.Map<UserDetails>(updateUserDetailsDto);
            var data = _dbContext.UserMaster.Find(id);
           
            data.FirstName=updateUserDetailsDto.FirstName;
            data.LastName=updateUserDetailsDto.LastName;
            data.MiddleName=updateUserDetailsDto.MiddleName;
            data.MobileNo=updateUserDetailsDto.MobileNo;
            data.Email=updateUserDetailsDto.Email;
            data.ProfileImage=updateUserDetailsDto.ProfileImage;
            data.Age=updateUserDetailsDto.Age;
            data.Gender = updateUserDetailsDto.Gender;
            data.UpdateDate = userDetails.UpdateDate;
            data.UpdateBy=userDetails.UpdateBy;
            data.HospitalId = userDetails.HospitalId;
            data.BranchId = userDetails.BranchId;
            
            _dbContext.UserMaster.Update(data);
            _dbContext.SaveChanges();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
        }

        public async Task<bool> UpdateStatus(Guid id, bool verified, Guid updateBy)
        {

            var data = _dbContext.UserMaster.Find(id);

            data.IsActive = verified;
            data.UpdateBy = updateBy;
            data.UpdateDate = DateTime.Now;
            _dbContext.UserMaster.Update(data);
             await _dbContext.SaveChangesAsync();

            return true;
        }

        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        /// <summary>
        /// Get user by ID (returns UserDetails entity directly)
        /// Used by RoleAssignmentService and other services
        /// </summary>
        public async Task<UserDetails?> GetByIdAsync(Guid userId)
        {
            var user = await _dbContext.UserMaster.FindAsync(userId);
            return user;
        }

        /// <summary>
        /// Update user with UserDetails entity
        /// Used by RoleAssignmentService and other services
        /// </summary>
        public async Task<ApiResponse<UserDetails>> UpdateAsync(UserDetails user)
        {
            try
            {
                if (user == null)
                {
                    return ApiResponse<UserDetails>.Failure("Validation Error", "User cannot be null");
                }

                var existingUser = await _dbContext.UserMaster.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return ApiResponse<UserDetails>.Failure("Not Found", $"User with ID {user.Id} not found");
                }

                // Update user properties
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.Email = user.Email;
                existingUser.MobileNo = user.MobileNo;
                existingUser.Role = user.Role;
                existingUser.UpdateBy = user.UpdateBy;
                existingUser.UpdateDate = user.UpdateDate;
                existingUser.IsActive = user.IsActive;
                existingUser.ProfileImage = user.ProfileImage;
                existingUser.Age = user.Age;
                existingUser.Gender = user.Gender;

                _dbContext.UserMaster.Update(existingUser);
                await _dbContext.SaveChangesAsync();

                return ApiResponse<UserDetails>.Success(existingUser, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDetails>.Failure("Error", $"Error updating user: {ex.Message}");
            }
        }
    }
}
