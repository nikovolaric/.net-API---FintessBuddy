using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Mvc;

public interface IExerciseService
{
    public Task<ServiceResult> AddExerciseService(AddExerciseRequest req);
    public Task<ServiceResult> DeleteExerciseService(long id);
    public Task<ServiceResult<Exercise>> UpdateExerciseService(long id, UpdateExerciseRequest req);
}
