﻿using Application.Dtos.Default;
using Application.Dtos.Game;
using Application.Dtos.Round;
using Application.Dtos.User.Create;
using Application.Dtos.User.Login;
using Application.Dtos.User.Password;
using Application.Dtos.User.Put;
using Domain.Entitites;

namespace Application.Services.Identity
{
    public interface IIdentityService
    {
        Task<BaseResponse<LoginUserResponse>> LoginAsync(LoginUserRequest loginData);
        Task<DefaultResponse> AddStudentUser(CreateStudentUserRequest userData);
        Task<DefaultResponse> AddProfessorUser(CreateProfessorUserRequest userData);
        Task<DefaultResponse> DeleteUser(LoginUserRequest email);
        Task<DefaultResponse> PutUser(PutUserRequest userData);
        Task<string> GetUserNameAsync(string id);
        Task<DefaultResponse> ValidateUsernameAsync(string email);
        Task<DefaultResponse> ValidateEmailAsync(string email);
        Task<DefaultResponse> ChangePasswordAsync(ChangePasswordRequest changePasswordData);
        Task<List<ApplicationUser>> GetStudents(IEnumerable<string> ids);
        Task<BaseResponse<List<StudentDto>>> GetStudents();
        Task<BaseResponse<UserBudgetResponse>> AnswerRound(AnswerRoundDto dto);
        Task<string> GetProfessorId();
        Task<decimal> GetBudgetAsync();
    }
}
