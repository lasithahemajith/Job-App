using JobService.Models;

namespace JobService.Services.Interfaces;

public interface IJobService
{
    Task<IEnumerable<Job>> GetAllJobs();
    Task<Job?> GetJobById(int id);
    Task<Job> CreateJob(Job job);
    Task<bool> UpdateJob(int id, Job job);
    Task<bool> DeleteJob(int id);
}