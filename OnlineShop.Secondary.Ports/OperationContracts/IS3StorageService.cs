using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IS3StorageService
{
    Task<IList<string>> Add(string prefix, string bucketName, string folderName, IEnumerable<IFormFile> files, CancellationToken cancellationToken);


}