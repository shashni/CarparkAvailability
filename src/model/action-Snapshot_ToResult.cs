(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IServerAction 'Snapshot_ToResult' ***/
    var snapshot_ToResult = eSpace.CreateServerAction("Snapshot_ToResult");
    snapshot_ToResult.CacheInMinutes = null; // default value
    snapshot_ToResult.Description = "Turns a stored AvailabilitySnapshot into dashboard rows + metadata, labelling it with the given freshness (Live when served from the short cache, Cached when the latest retrieval failed). Data age keeps growing with the time since retrieval.";
    snapshot_ToResult.Folder = null; // default value
    snapshot_ToResult.Function = false; // default value
    snapshot_ToResult.Icon = null; // default value
    snapshot_ToResult.Public = false; // default value
    snapshot_ToResult.Shareable = false; // default value
    
    var snapshot = snapshot_ToResult.CreateInputParameter("Snapshot");
    var availabilitySnapshot = eSpace.Entities.OfType<OutSystems.Model.Data.IServerEntity>().Named("AvailabilitySnapshot");
    snapshot.DataType = availabilitySnapshot;
    snapshot.Description = null; // default value
    snapshot.IsMandatory = true; // default value
    
    var freshness = snapshot_ToResult.CreateInputParameter("Freshness");
    freshness.DataType = eSpace.TextType;
    freshness.Description = null; // default value
    freshness.IsMandatory = true; // default value
    
    var message = snapshot_ToResult.CreateInputParameter("Message");
    message.DataType = eSpace.TextType;
    message.SetDefaultValue(null); // default value
    message.Description = null; // default value
    message.IsMandatory = false;
    
    var startNode = snapshot_ToResult.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var deserializeRows = snapshot_ToResult.CreateNode<OutSystems.Model.Logic.Nodes.IJSONDeserializeNode>("DeserializeRows").ConnectedBelow(startNode, 1371);
    var carparkLotRow = eSpace.Structures.Named("CarparkLotRow");
    deserializeRows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    deserializeRows.DateFormat = OutSystems.Model.Enumerations.JSONDateFormat.ISO; // default value
    deserializeRows.Description = null; // default value
    deserializeRows.IsDisabled = false; // default value
    deserializeRows.SetJSONString("Snapshot.RowsJson");
    
    var assignNode = snapshot_ToResult.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(deserializeRows, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = null; // default value
    
    var assignment = assignNode.CreateAssignment("Rows", "DeserializeRows.Data");
    
    var assignment2 = assignNode.CreateAssignment("Meta.Freshness", "Freshness");
    
    var assignment3 = assignNode.CreateAssignment("Meta.HasData", "True");
    
    var assignment4 = assignNode.CreateAssignment("Meta.SourceTimestamp", "Snapshot.SourceTimestamp");
    
    var assignment5 = assignNode.CreateAssignment("Meta.DataAgeSeconds", "If(Snapshot.DataAgeSecondsAtRetrieval < 0, -1, Snapshot.DataAgeSecondsAtRetrieval + DiffSeconds(Snapshot.RetrievedOn, CurrDateTime()))");
    
    var assignment6 = assignNode.CreateAssignment("Meta.StaleAfterMinutes", "Site.StaleAfterMinutes");
    
    var assignment7 = assignNode.CreateAssignment("Meta.CarparkCount", "Snapshot.CarparkCount");
    
    var assignment8 = assignNode.CreateAssignment("Meta.RejectedCount", "Snapshot.RejectedCount");
    
    var assignment9 = assignNode.CreateAssignment("Meta.DuplicateCount", "Snapshot.DuplicateCount");
    
    var assignment10 = assignNode.CreateAssignment("Meta.AnomalyCount", "Snapshot.AnomalyCount");
    
    var assignment11 = assignNode.CreateAssignment("Meta.IsPartial", "Snapshot.RejectedCount > 0");
    
    var assignment12 = assignNode.CreateAssignment("Meta.HasLocationData", "Snapshot.HasLocationData");
    
    var assignment13 = assignNode.CreateAssignment("Meta.Message", "Message");
    
    var assignment14 = assignNode.CreateAssignment("Meta.SourceTimestampText", "FormatDateTime(Snapshot.SourceTimestamp, \"d MMM yyyy, h:mm tt\")");
    
    var assignment15 = assignNode.CreateAssignment("Meta.ThresholdSummary", "\"High: more than \" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% of lots available · Moderate: \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"–\" + FormatDecimal(Site.HighAvailabilityMinPct, 0, \".\", \"\") + \"% · Low: under \" + FormatDecimal(Site.LowAvailabilityMaxPct, 0, \".\", \"\") + \"% · Full: no lots available\"");
    
    var endNode = snapshot_ToResult.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode, 1371);
    endNode.IsDisabled = false; // default value
    
    var rows = snapshot_ToResult.CreateOutputParameter("Rows");
    rows.DataType = eSpace.GetOrCreateListType(carparkLotRow);
    rows.Description = null; // default value
    
    var meta = snapshot_ToResult.CreateOutputParameter("Meta");
    var availabilityMeta = eSpace.Structures.Named("AvailabilityMeta");
    meta.DataType = availabilityMeta;
    meta.Description = null; // default value
}
