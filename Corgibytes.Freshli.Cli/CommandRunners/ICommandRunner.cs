﻿using System.CommandLine;
using System.CommandLine.Invocation;
using Corgibytes.Freshli.Cli.CommandOptions;

namespace Corgibytes.Freshli.Cli.CommandRunners;

public interface ICommandRunner<TCommand, TCommandOptions> where TCommand : Command where TCommandOptions : CommandOptions.CommandOptions
{
    public int Run(TCommandOptions options, InvocationContext context);
}
