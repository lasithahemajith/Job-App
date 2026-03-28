using Microsoft.AspNetCore.Mvc;
using JobService.Models;
using JobService.Services.Interfaces;

namespace JobService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _jobService.GetAllJobs();
        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJob(int id)
    {
        var job = await _jobService.GetJobById(id);
        if (job == null) return NotFound();
        return Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(Job job)
    {
        var createdJob = await _jobService.CreateJob(job);
        return CreatedAtAction(nameof(GetJob), new { id = createdJob.Id }, createdJob);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(int id, Job job)
    {
        var updated = await _jobService.UpdateJob(id, job);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var deleted = await _jobService.DeleteJob(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}