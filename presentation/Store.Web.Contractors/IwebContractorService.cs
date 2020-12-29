using System;
using System.Collections.Generic;

namespace Store.Web.Contractors
{
    public interface IwebContractorService
    {
        string Name { get; }

        Uri StartSession(IReadOnlyDictionary<string, string> parametrs, Uri request);
    }

}
