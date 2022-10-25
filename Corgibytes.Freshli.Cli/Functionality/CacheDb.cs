using System;
using System.Collections.Generic;
using System.Linq;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Functionality.Git;
using Microsoft.Data.Sqlite;
using Polly;
using PackageUrl;

namespace Corgibytes.Freshli.Cli.Functionality;

public class CacheDb : ICacheDb, IDisposable
{
    private bool _disposed;

    public CacheDb(string cacheDir) => Db = new CacheContext(cacheDir);

    private CacheContext Db { get; }

    public Guid SaveAnalysis(CachedAnalysis analysis)
    {
        if (analysis.Id == Guid.Empty)
        {
            var savedEntity = Db.CachedAnalyses.Add(analysis);
            SaveChanges();
            return savedEntity.Entity.Id;
        }

        Db.CachedAnalyses.Update(analysis);
        SaveChanges();
        return analysis.Id;
    }

    public CachedAnalysis? RetrieveAnalysis(Guid id) => Db.CachedAnalyses.Find(id);
    public CachedGitSource? RetrieveCachedGitSource(CachedGitSourceId id) => Db.CachedGitSources.Find(id.Id);
    public void RemoveCachedGitSource(CachedGitSource cachedGitSource) => Db.CachedGitSources.Remove(cachedGitSource);

    public void AddCachedGitSource(CachedGitSource cachedGitSource)
    {
        Db.CachedGitSources.Add(cachedGitSource);
        SaveChanges();
    }

    public CachedHistoryStopPoint? RetrieveHistoryStopPoint(int historyStopPointId) =>
        Db.CachedHistoryStopPoints.Find(historyStopPointId);

    public int AddHistoryStopPoint(CachedHistoryStopPoint historyStopPoint)
    {
        var savedEntity = Db.CachedHistoryStopPoints.Add(historyStopPoint);
        SaveChanges();
        return savedEntity.Entity.Id;
    }

    public CachedPackageLibYear? RetrievePackageLibYear(int packageLibYearId) =>
        Db.CachedPackageLibYears.Find(packageLibYearId);

    public List<CachedPackage> RetrieveCachedReleaseHistory(PackageURL packageUrl) => Db.CachedPackages
        .Where(value => value.PackageUrlWithoutVersion == packageUrl.ToString()).ToList();

    public void StoreCachedReleaseHistory(List<CachedPackage> packages)
    {
        Db.CachedPackages.AddRange(packages);
        Db.SaveChanges();
    }

    public int AddPackageLibYear(CachedPackageLibYear packageLibYear)
    {
        var savedEntity = Db.CachedPackageLibYears.Add(packageLibYear);
        SaveChanges();
        return savedEntity.Entity.Id;
    }

    private void SaveChanges()
    {
        void Changes()
        {
            Db.SaveChanges();
        }

        Policy
            .Handle<SqliteException>()
            .WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            })
            .Execute(Changes);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Db.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
