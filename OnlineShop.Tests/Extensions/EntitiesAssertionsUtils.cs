using System.Collections.Generic;
using System.Linq;
using System.Net;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Tests.Extensions;

public static class EntitiesAssertionsUtils<T>
    where T : Editable
{

public static bool AreListsEqual(List<T> list1,
        List<T> list2)
        => !list1.Select(l1 => l1.Id).Except(list2.Select(l2 => l2.Id)).Any();

    public static bool AreEntriesEqual(T entity1,
        T entity2)
        => entity1.Id.Equals(entity2.Id);

    public static bool IsCorrectError(HttpStatusCode currentStatusCode,
        string currentErrorMessage,
        HttpStatusCode expectedStatusCode,
        string expectedErrorMessage)
        => currentStatusCode.Equals(expectedStatusCode) && currentErrorMessage.Equals(expectedErrorMessage);
}