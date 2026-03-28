using JobService.Data;
using JobService.Models;
using JobService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobService.Services;

public class JobServiceImpl : IJobService
{
    private readonly AppDbContext _context;

    public JobServiceImpl(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Job>> GetAllJobs()
    {
        return await _context.Jobs.ToListAsync();
    }

    public async Task<Job?> GetJobById(int id)
    {
        return await _context.Jobs.FindAsync(id);
    }

    public async Task<Job> CreateJob(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<bool> UpdateJob(int id, Job updatedJob)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;

        job.Title = updatedJob.Title;
        job.Company = updatedJob.Company;
        job.Location = updatedJob.Location;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteJob(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return false;

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return true;
    }
}