(OutSystems.Model.IESpace eSpace) => {
    
    /*** creating IServerAction 'AvailabilityStatus_Classify' ***/
    var availabilityStatus_Classify = eSpace.CreateServerAction("AvailabilityStatus_Classify");
    availabilityStatus_Classify.CacheInMinutes = null; // default value
    availabilityStatus_Classify.Description = "Classifies one lot type\'s reading. Unknown when the reading is inconsistent (no lots, negative, or more available than total); Full at 0; Low below Site.LowAvailabilityMaxPct; High above Site.HighAvailabilityMinPct; otherwise Moderate.";
    availabilityStatus_Classify.Folder = null; // default value
    availabilityStatus_Classify.Function = true;
    availabilityStatus_Classify.Icon = null; // default value
    availabilityStatus_Classify.Public = false; // default value
    availabilityStatus_Classify.Shareable = false; // default value
    
    var availableLots = availabilityStatus_Classify.CreateInputParameter("AvailableLots");
    availableLots.DataType = eSpace.IntegerType;
    availableLots.Description = null; // default value
    availableLots.IsMandatory = true; // default value
    
    var totalLots = availabilityStatus_Classify.CreateInputParameter("TotalLots");
    totalLots.DataType = eSpace.IntegerType;
    totalLots.Description = null; // default value
    totalLots.IsMandatory = true; // default value
    
    var startNode = availabilityStatus_Classify.CreateNode<OutSystems.Model.Logic.Nodes.IStartNode>();
    startNode.HorizontalPosition = 3200;
    startNode.IsDisabled = false; // default value
    startNode.VerticalPosition = 800;
    
    var assignNode = availabilityStatus_Classify.CreateNode<OutSystems.Model.Logic.Nodes.IAssignNode>().ConnectedBelow(startNode, 1371);
    assignNode.IsDisabled = false; // default value
    assignNode.Label = null; // default value
    
    var assignment = assignNode.CreateAssignment("StatusId", "If(TotalLots <= 0 or AvailableLots < 0 or AvailableLots > TotalLots, Entities.AvailabilityStatus.Unknown, If(AvailableLots = 0, Entities.AvailabilityStatus.Full, If(AvailableLots * 100.0 / TotalLots < Site.LowAvailabilityMaxPct, Entities.AvailabilityStatus.Low, If(AvailableLots * 100.0 / TotalLots > Site.HighAvailabilityMinPct, Entities.AvailabilityStatus.High, Entities.AvailabilityStatus.Moderate))))");
    
    var endNode = availabilityStatus_Classify.CreateNode<OutSystems.Model.Logic.Nodes.IEndNode>().ConnectedBelow(assignNode, 1371);
    endNode.IsDisabled = false; // default value
    
    var statusId = availabilityStatus_Classify.CreateOutputParameter("StatusId");
    var availabilityStatus = eSpace.Entities.OfType<OutSystems.Model.Data.IStaticEntity>().Named("AvailabilityStatus");
    statusId.DataType = availabilityStatus.IdentifierType;
    statusId.SetDefaultValue(null); // default value
    statusId.Description = null; // default value
}
