using System;

namespace Store.Web.Contractors
{
    public interface IwebContractorService
    {
        string UniqueCode { get; }
        string GetUri { get; }
    }

}
