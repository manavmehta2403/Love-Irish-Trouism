using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ItineraryFolderinfo
{
    public long Folderid { get; set; }

    public int? Hierarchylevel { get; set; }

    public string? Parentfolder { get; set; }

    public string? FolderFullpath { get; set; }
}
