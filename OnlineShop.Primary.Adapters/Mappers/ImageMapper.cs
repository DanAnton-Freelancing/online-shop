using System.Collections.Generic;
using System.Linq;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Mappers;

public static class ImageMapper
{
    public static List<primaryPorts.Image> MapToPrimary(this List<secondaryPorts.Image> images)
        => images.Select(l => l.MapToPrimary())
            .ToList();

    public static primaryPorts.Image MapToPrimary(this secondaryPorts.Image image)
        => new()
        {
            Id = image.Id,
        };

}