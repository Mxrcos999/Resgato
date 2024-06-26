﻿using Application.Dtos.Round;
using Newtonsoft.Json;

namespace Application.Dtos.Game;

public class GetGameDto
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public int ProfessorId { get; set; }
    public int TotalStudent { get; set; }
    public IEnumerable<StudentDto> Students { get; set; }
    public IEnumerable<GetRoundDto> Round { get; set; }
}
public class GameDto
{
    public string GameName { get; set; }
    public IEnumerable<string> StudentsId { get; set; }
    public IEnumerable<RoundDto> Round { get; set; }
}

public class GameInformation
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public string Name { get; set; }
    public bool GameConcluded { get; set; }
    public bool AnsweredRound { get; set; }
    public int CurrentRound { get; set; }
    public int RoundActive { get; set; }
    public int TotalCatsMale { get; set; }
    public int TotalCatsMaleCastrated { get; set; }
    public int TotalCatsFemale { get; set; }
    public int TotalCatsFemaleCastrated { get; set; }
    public decimal BudgetUser { get; set; }
    public int TotalStudent { get; set; }
}

public class StudentDto
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string StudentCode { get; set; }
    public string Email { get; set; }
}
public class PassRound
{
    public int Id { get; set; }
}

public class Players
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public int TotalPopulation { get; set; }
}
