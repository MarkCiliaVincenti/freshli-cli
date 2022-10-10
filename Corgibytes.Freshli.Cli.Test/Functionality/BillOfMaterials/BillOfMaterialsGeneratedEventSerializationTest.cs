using System;
using Corgibytes.Freshli.Cli.Functionality.BillOfMaterials;
using Xunit;

namespace Corgibytes.Freshli.Cli.Test.Functionality.BillOfMaterials;

[IntegrationTest]
public class BillOfMaterialsGeneratedEventSerializationTest : SerializationTest<BillOfMaterialsGeneratedEvent>
{
    protected override BillOfMaterialsGeneratedEvent BuildIncoming() =>
        new(Guid.NewGuid(), this.BuildHistoryStopData(), "/path/to/bom", "/path/to/agent");

    protected override void AssertEqual(BillOfMaterialsGeneratedEvent incoming, BillOfMaterialsGeneratedEvent outgoing)
    {
        Assert.Equal(incoming.AnalysisId, outgoing.AnalysisId);
        this.AssertHistoryStopDataEqual(incoming.HistoryStopData, outgoing.HistoryStopData);
        Assert.Equal(incoming.PathToBillOfMaterials, outgoing.PathToBillOfMaterials);
        Assert.Equal(incoming.AgentExecutablePath, outgoing.AgentExecutablePath);
    }
}
