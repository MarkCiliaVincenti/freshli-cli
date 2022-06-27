using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.DependencyManagers;
using Corgibytes.Freshli.Cli.Functionality;

namespace Corgibytes.Freshli.Cli.Services;

public class CalculateLibYearFromCycloneDxFile : ICalculateLibYearFromFile
{
    private readonly ReadCycloneDxFile _readFile;
    private readonly IDependencyManagerRepository _repository;

    public CalculateLibYearFromCycloneDxFile(
        ReadCycloneDxFile readFileService,
        IDependencyManagerRepository repository
    )
    {
        _readFile = readFileService;
        _repository = repository;
    }

    public IList<PackageLibYear> AsList(string filePath, int precision = 2)
    {
        var packageUrls = _readFile.AsPackageURLs(filePath);
        var libYearList = new List<PackageLibYear>();

        foreach (var currentlyInstalled in packageUrls)
        {
            var latestVersion =
                _repository.GetLatestVersion(currentlyInstalled);
            var releaseDatePackageCurrentlyInstalled =
                _repository.GetReleaseDate(currentlyInstalled);
            var releaseDatePackageLatestAvailable =
                _repository.GetReleaseDate(latestVersion);

            libYearList.Add(new(
                releaseDatePackageCurrentlyInstalled,
                currentlyInstalled,
                releaseDatePackageLatestAvailable,
                latestVersion,
                LibYear.GivenReleaseDates(releaseDatePackageCurrentlyInstalled, releaseDatePackageLatestAvailable).AsDecimalNumber(precision)
            ));
        }

        return libYearList;
    }

    public double TotalAsDecimalNumber(string filePath, int precision = 2)
    {
        var packageUrls = _readFile.AsPackageURLs(filePath);
        var libYear = 0.0;

        foreach (var currentlyInstalled in packageUrls)
        {
            var latestVersion =
                _repository.GetLatestVersion(currentlyInstalled);
            var releaseDatePackageCurrentlyInstalled =
                _repository.GetReleaseDate(currentlyInstalled);
            var releaseDatePackageLatestAvailable =
                _repository.GetReleaseDate(latestVersion);

            libYear += LibYear.GivenReleaseDates(releaseDatePackageCurrentlyInstalled, releaseDatePackageLatestAvailable).AsDecimalNumber(precision);
        }

        return Math.Round(libYear, precision);
    }
}

