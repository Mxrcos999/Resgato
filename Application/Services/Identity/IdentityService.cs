﻿using App.Application.Configuration;
using Application.Dtos.Default;
using Application.Dtos.User.Create;
using Application.Dtos.User.Login;
using Application.Dtos.User.Password;
using Application.Dtos.User.Put;
using Domain.Entitites;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationContext _context;

        private readonly SignInManager<ApplicationUser> _singInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly JwtOptions _jwtOptions;

        private readonly string _userId;

        public IdentityService(ApplicationContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _singInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<DefaultResponse> PutUser(PutUserRequest userData)
        {
            var user = await _userManager.FindByIdAsync(_userId);

            var response = new DefaultResponse();

            if (user != null)
            {
                user.Name = userData.Name == null ? user.Name : userData.Name;
                user.Email = userData.Email == null ? user.Email : userData.Email;
                user.UserName = userData.Username == null ? user.UserName : userData.Username;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    response.AddErrors(result.Errors.ToList().ConvertAll<string>(item => item.Description));
            
                return response;
            }

            else
            {
                response.AddError("Faça login novamente e tente mais tarde.");

                return response;
            }
        }

        public async Task<DefaultResponse> AddUser(CreateUserRequest userData)
        {
            var user = new ApplicationUser()
            {
                UserName = userData.Email,
                Email = userData.Email,
                CreateUserDate = DateTime.UtcNow,
                EmailConfirmed = false,
                Name = userData.Name,
                StudentCode = userData.StudentCode
            };

            var createdUser = await _userManager.CreateAsync(user, userData.Password);
      
            var defaultResponse = new DefaultResponse();

            if (!createdUser.Succeeded)
            {
                foreach (var error in createdUser.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordRequiresNonAlphanumeric":
                            defaultResponse.Errors.Add("A senha precisa conter pelo menos um caracter especial - ex( * | ! ).");
                            break;

                        case "PasswordRequiresDigit":
                            defaultResponse.Errors.Add("A senha precisa conter pelo menos um número (0 - 9).");
                            break;

                        case "PasswordRequiresUpper":
                            defaultResponse.Errors.Add("A senha precisa conter pelo menos um caracter em maiúsculo.");
                            break;

                        case "DuplicateUserName":
                            defaultResponse.Errors.Add("O email informado já foi cadastrado!");
                            break;

                        default:
                            defaultResponse.Errors.Add("Erro ao criar usuário.");
                            break;
                    }

                }
            }

            return defaultResponse;
        }

        public async Task<DefaultResponse> DeleteUser(LoginUserRequest loginData)
        {
            var user = await GetUserByEmailOrUsername(loginData.Email);

            var login = await _singInManager.PasswordSignInAsync(user, loginData.Password, false, false);

            var response = new DefaultResponse();

            if (login.Succeeded && user.Id == _userId)
            {
                _userManager.DeleteAsync(user);

                return response;
            }

            else
            {
                response.AddError("Senha ou Usuario incorretos.");

                return response;
            }
        }

        public async Task<DefaultResponse> ValidateEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var response = new DefaultResponse();

            if (response.Success)
            {
                return response;
            }

            else
            {
                response.AddError("E-mail ja utilizado.");

                return response;
            }
        }

        public async Task<BaseResponse<LoginUserResponse>> LoginAsync(LoginUserRequest loginData)
        {
            var user = await GetUserByEmailOrUsername(loginData.Email);

            var signInResult = await _singInManager.PasswordSignInAsync(user, loginData.Password, false, false);

            var response = new BaseResponse<LoginUserResponse>();
   
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    response.Errors.Add("Esta conta está bloqueada.");
                }
                else if (signInResult.IsNotAllowed)
                {
                    response.Errors.Add("Esta conta não tem permissão para entrar.");
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    response.Errors.Add("Confirme seu email.");
                }
                else
                {
                    response.Errors.Add("Nome de usuário ou senha estão incorretos.");
                }

                return response;
            }

            response.Data = new()
            {
                Email = user.Email,
                ExpectedExpirationTokenDateTime = DateTime.UtcNow,
                Name = user.Name,
                Username = user.UserName,
                ExpirationTokenTime = _jwtOptions.AccessTokenExpiration,
                Token = await CreateToken(user)

            };

            return response;
        }

        public async Task<IList<Claim>> GetClaimsAndRoles(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));

            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            };

            return claims;
        }

        public async Task<string> CreateToken(ApplicationUser user)
        {
            var claims = await GetClaimsAndRoles(user);

            var expiresDate = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresDate,
                notBefore: DateTime.Now,
                signingCredentials: _jwtOptions.SigningCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public async Task<ApplicationUser> GetUserByEmailOrUsername(string accessKey)
        {
            var user = IsValidEmail(accessKey) ?
                await _userManager.FindByEmailAsync(accessKey) :
                await _userManager.FindByNameAsync(accessKey);

            return user;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }

            catch (RegexMatchTimeoutException e)
            {
                return false;
            }

            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }

            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public async Task<DefaultResponse> ValidateUsernameAsync(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            var response = new DefaultResponse();

            if (!response.Success)
            {
                response.AddError("Nome de usuário já utilizado.");

                return response;
            }

            return response;
        }

        public async Task<DefaultResponse> ChangePasswordAsync(ChangePasswordRequest changePasswordData)
        {
            var user = await _userManager.FindByIdAsync(_userId);

            var changedPassword = await _userManager.ChangePasswordAsync(user, changePasswordData.Passowrd, changePasswordData.NewPassword);

            var response = new DefaultResponse();

            if (response.Success)
            {
                return response;
            }

            else
            {
                response.AddErrors(changedPassword.Errors.ToList().ConvertAll<string>(item => item.Description));
            }

            throw new NotImplementedException();
        }
    }
}