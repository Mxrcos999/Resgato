﻿using App.Application.Configuration;
using Application.Dtos.Default;
using Application.Dtos.Game;
using Application.Dtos.Prevention;
using Application.Dtos.Round;
using Application.Dtos.User.Create;
using Application.Dtos.User.Login;
using Application.Dtos.User.Password;
using Application.Dtos.User.Put;
using Domain.Entitites;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Repositories.BaseRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationContext _context;

        private readonly SignInManager<ApplicationUser> _singInManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISessionService _session;

        private readonly JwtOptions _jwtOptions;

        private readonly ISettingRep settingRep;

        private readonly string _userId;

        private readonly IBaseRepository<Professor> professorRep;
        private readonly IBaseRepository<Answers> AnswarRep;
        private readonly IBaseRepository<Settings> Settingrepository;
        private readonly IGameRep gameRep;
        private readonly IAnswersService answerSvc;

        private readonly IBaseRepository<PreventionAction> preventionRep;
        public IdentityService(ApplicationContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions, ISessionService session, IBaseRepository<Professor> professorRep, ISettingRep settingRep, IBaseRepository<PreventionAction> preventionRep, IBaseRepository<Answers> answarRep, IAnswersService answerSvc, IGameRep gameRep, IBaseRepository<Settings> settingrepository)
        {
            _context = context;
            _singInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _userId = _context._contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _session = session;
            this.professorRep = professorRep;
            this.settingRep = settingRep;
            this.preventionRep = preventionRep;
            AnswarRep = answarRep;
            this.answerSvc = answerSvc;
            this.gameRep = gameRep;
            Settingrepository = settingrepository;
        }

        public async Task<BaseResponse<List<StudentDto>>> GetStudents()
        {
            var data = _userManager.Users.ToList().Where(x => x.Type == "Student");

            var response = new BaseResponse<List<StudentDto>>();

            response.Data = (from student in data
                             select new StudentDto()
                             {
                                 Email = student.Email,
                                 StudentCode = student.StudentCode,
                                 Name = student.Name,
                                 Id = student.Id
                             }).ToList();

            return response;
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

        public async Task<DefaultResponse> AddStudentUser(CreateStudentUserRequest userData)
        {
            var user = new ApplicationUser()
            {
                UserName = userData.Email,
                Email = userData.Email,
                Type = "Student",
                CreateUserDate = DateTime.UtcNow,
                EmailConfirmed = false,
                Name = userData.Name,
                StudentCode = userData.StudentCode,
                Budget = 10000m,

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

        public async Task<DefaultResponse> AddProfessorUser(CreateProfessorUserRequest userData)
        {
            var professor = new Professor();

            var user = new ApplicationUser()
            {
                UserName = userData.Email,
                Type = "Professor",
                Email = userData.Email,
                CreateUserDate = DateTime.UtcNow,
                EmailConfirmed = false,
                Name = userData.Name,
                Professor = professor,
                Budget = 10000m
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

        public async Task<BaseResponse<UserBudgetResponse>> AnswerRound(AnswerRoundDto dto)
        {
            var user = await _userManager.FindByIdAsync(_userId);

            var preventions = await preventionRep.GetAllAsync();

            decimal maleCastrationsValue = (preventions.Where
                (x => x.Action == "Castração" && x.Gender == "Macho")
                .FirstOrDefault()).Value * dto.QtdMaleCastrate;

            decimal femaleCastrationsValue = (preventions.Where
                (x => x.Action == "Castração" && x.Gender == "Femea")
                .FirstOrDefault()).Value * dto.QtdFemaleCastrate;

            decimal maleShelterValue = (preventions.Where
                (x => x.Action == "Abrigo" && x.Gender == "Macho")
                .FirstOrDefault()).Value * dto.QtdMaleShelter;

            decimal femaleShelterValue = (preventions.Where
                (x => x.Action == "Abrigo" && x.Gender == "Femea")
                .FirstOrDefault()).Value * dto.QtdFamaleShelter;

            var total = maleCastrationsValue + femaleCastrationsValue + maleShelterValue + femaleShelterValue;

            var settingList = (await settingRep.GetAllAsync(dto.GameId, _userId)).FirstOrDefault();

            var totalPopulation = settingList.SettingCat.Sum(x => x.CatsQuantity);

            var maleSetting = settingList.SettingCat.FirstOrDefault(x => x.Gender == "Macho");
            if (maleSetting != null)
            {
                maleSetting.CatsQuantity -= (dto.QtdMaleCastrate);
                maleSetting.CatsQuantity -= (dto.QtdMaleShelter);
            }

            var femaleSetting = settingList.SettingCat.FirstOrDefault(x => x.Gender == "Femea");

            if (femaleSetting != null)
            {
                femaleSetting.CatsQuantity -= (dto.QtdFemaleCastrate);
                femaleSetting.CatsQuantity -= (dto.QtdFamaleShelter);
            }
            var gamee = (await gameRep.GetAllGame()).Where(x => x.Id == dto.GameId);
            var round = gamee.FirstOrDefault().Rounds.Where(x => x.Active == true).FirstOrDefault();
            gamee.FirstOrDefault().Settings.BudgetGame -= total;
            var resultRound = new ResultRound()
            {
                TotalPopulation = totalPopulation,
                TotalPopulationCastrated = dto.QtdMaleCastrate + dto.QtdFemaleCastrate,
                TotalPopulationFemaleCastrated = dto.QtdFemaleCastrate,
                TotalPopulationMaleCastrated = dto.QtdMaleCastrate,
            };

            var result = await answerSvc.CreateAnswer(dto, round.CurrentRound, resultRound, round.Deadline);

            await Settingrepository.UpdateAsync(gamee.FirstOrDefault().Settings) ;

            var response = new BaseResponse<UserBudgetResponse>();
            response.Data = new UserBudgetResponse() 
            { 
                Budget = user.Budget,
                DateCastrate = dto.DateCastration.ToString("dd/MM/yyyy"),
                TotalPopulation = totalPopulation,
                TotalPopulationCastrate = resultRound.TotalPopulationCastrated,
            };

            return response;
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
        public async Task<string> GetProfessorId()
        {
            var email = await _userManager.FindByIdAsync(_userId);

            return email.Email;
        }
        public async Task<List<ApplicationUser>> GetStudents(IEnumerable<string> ids)
        {
            var students = new List<ApplicationUser>();
            foreach (var id in ids)
            {
                students.Add(await _userManager.FindByIdAsync(id));
            }

            return students;
        }
        public async Task<decimal> GetBudgetAsync()
        {
            var user = await _userManager.FindByIdAsync(_userId);

            return user.Budget;
        }     
        public async Task<string> GetUserNameAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user.Name;
        }
    }
}

public class UserBudgetResponse
{
    public decimal Budget { get; set; }
    public int TotalPopulation { get; set; }
    public int TotalPopulationCastrate { get; set; }
    public string DateCastrate { get; set; }
    public int NextRound { get; set; }

}