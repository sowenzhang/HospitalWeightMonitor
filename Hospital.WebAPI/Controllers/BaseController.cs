using System;
using System.Web.Http;
using Hospital.Web.Common.ActionResults;
using Repository.Pattern.UnitOfWork;

namespace Hospital.WebAPI.Controllers
{
    public class BaseController : ApiController
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        protected IUnitOfWorkAsync UnitOfWork { get { return _unitOfWork; } }

        public BaseController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWork = unitOfWorkAsync;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        protected virtual UpdatedActionResult<TEntity> Updated<TEntity>(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return new UpdatedActionResult<TEntity>(entity, this);
        }

    }
}