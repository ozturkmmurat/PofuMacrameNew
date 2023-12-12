using Core.Entities.Concrete;
using Core.Utilities.Result.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        IDataResult<List<OperationClaim>> GetAllOperationClaim();
    }
}
