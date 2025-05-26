using AutoMapper;
using MiniProject.Models;

namespace MiniProject.Services;

public class MappingProfiles : Profile
{

    public MappingProfiles()
    {
        CreateMap<Workout, Workout>();
        CreateMap<Exercise, Exercise>();
        CreateMap<WorkoutExercise, WorkoutExercise>();

    }

}