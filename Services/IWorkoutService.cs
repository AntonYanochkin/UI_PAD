using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Models;
namespace UI.Services
{


    public interface IWorkoutService
    {
        Task<WorkoutProgram> GetWorkoutProgramByIdAsync(int id);
        Task<List<WorkoutProgram>> GetWorkoutProgramsAsync();
        Task<WorkoutProgram> CreateWorkoutProgramAsync(WorkoutProgram workoutProgram);
    }
}
