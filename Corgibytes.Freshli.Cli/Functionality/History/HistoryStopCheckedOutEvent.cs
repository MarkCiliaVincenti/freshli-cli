using System;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;

namespace Corgibytes.Freshli.Cli.Functionality.History;

public class HistoryStopCheckedOutEvent : IApplicationEvent
{
    public Guid AnalysisId { get; init; }
    public IHistoryStopData HistoryStopData { get; init; } = null!;

    public void Handle(IApplicationActivityEngine eventClient) =>
        eventClient.Dispatch(new DetectAgentsForDetectManifestsActivity(AnalysisId, HistoryStopData));
}
