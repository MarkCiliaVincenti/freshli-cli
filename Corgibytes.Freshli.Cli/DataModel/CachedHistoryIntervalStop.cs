using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Corgibytes.Freshli.Cli.DataModel;

[Index(nameof(Id), IsUnique = true)]
public class CachedHistoryIntervalStop
{
    public CachedHistoryIntervalStop(string gitCommitId, DateTimeOffset gitCommitDate)
    {
        GitCommitId = gitCommitId;
        GitCommitDate = gitCommitDate;
    }

    [Required] public int Id { get; set; }

    [Required] public DateTimeOffset GitCommitDate { get; set; }

    [Required] public string GitCommitId { get; set; }

    [Required] public CachedAnalysis CachedAnalysis { get; set; }
}
