using System;

namespace Corgibytes.Freshli.Cli.Exceptions;

public class ReleaseDateNotFoundException : ApplicationException
{
    private ReleaseDateNotFoundException(string message) : base(message)
    {
    }

    public static ReleaseDateNotFoundException BecauseNoAgentReturnedAnyResults()
    {
        return new("None of the agents returned results for this package url");
    }

    public static ReleaseDateNotFoundException BecauseReturnedListDidNotContainReleaseDate()
    {
        return new("The returned list did not contain a release date for this package url");
    }
}

