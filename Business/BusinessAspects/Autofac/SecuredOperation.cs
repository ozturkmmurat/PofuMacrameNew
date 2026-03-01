using Business.Constans;
using Castle.DynamicProxy;
using Core.CrossCuttingConcers.Caching;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;
        private ICacheManager _cacheManager;
        public SecuredOperation(string roles)
        {
            _roles = roles.Split(','); // Claimleri böl ve _roles dizisne at 
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
            // Autofac ile oluşturduğumuz servis mimarisine ulaş 
        }

        protected override void OnBefore(IInvocation invocation)
        {
            try
            {
                var checkNameIdentifier = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var checkNameIdentifierNull = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value == null;
            }
            catch (Exception)
            {

                throw new SecuredOperationException(UserMessages.TokenExpired);
            }

            //UserId bazen 0 geliyor bu sebepten dolayı bu kontrol oluşturuldu Frontend tarafında interceptor da buna göre bir kod yazdım durum devam ederse  bu kod aktif edilmeli.
            // Hatanın oluştuğunu şuradan anlayabilirsiniz diyelim ki giriş yaptınız. 40 45 dakika işlem yapmadınız sonra işlem yapmaya çalıştınız ama  işlem yapamıyorsunuz örneğin kayıt işlemi ama sizi hesaptan da atmadı yani token süresi dolmadı
            // O zaman bu kod aktif edilmeli ilgili sorun devam ediyor demektir.

            var userId = ClaimHelper.GetUserId(_httpContextAccessor.HttpContext);
            if (_cacheManager.Get<IEnumerable<string>>($"{CacheKeys.UserIdForClaim}={userId}") == null)
            {
                throw new SecuredOperationException(UserMessages.TokenExpired);
            }

            var roleClaims = _cacheManager.Get<IEnumerable<string>>($"{CacheKeys.UserIdForClaim}={userId}").ToList();// O an ki kullanıcını Claimroles bul diyoruz 

            var isAdmin = roleClaims.Contains("admin");
            var rolesIncludeUser = _roles.Any(r => string.Equals(r, "user", StringComparison.OrdinalIgnoreCase));

            var hasRequiredRole = false;
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role)) // Claimlerin içinde ilgili rol var ise 
                {
                    hasRequiredRole = true;
                    break;
                }
            }

            if (!hasRequiredRole)
            {
                throw new SecuredOperationException(UserMessages.AuthorizationDenied); // Eğer ki claimi yok ise hata ver 
            }

            // Eğer method [SecuredOperation("user,...")] ile işaretlenmişse ve çağıran admin değilse,
            // dışarıdan gelen userId parametresini ve varsa DTO içindeki UserId propertysini token'daki UserId ile eşitle.
            if (rolesIncludeUser && !isAdmin)
            {
                var parameters = invocation.Method.GetParameters();

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (string.Equals(parameters[i].Name, "userId", StringComparison.OrdinalIgnoreCase) &&
                        (parameters[i].ParameterType == typeof(int) || parameters[i].ParameterType == typeof(int?)))
                    {
                        invocation.Arguments[i] = userId;
                    }
                }

                for (int i = 0; i < invocation.Arguments.Length; i++)
                {
                    var argument = invocation.Arguments[i];
                    if (argument == null) continue;

                    var argumentType = argument.GetType();
                    var userIdProperty = argumentType.GetProperty("UserId");
                    if (userIdProperty != null &&
                        (userIdProperty.PropertyType == typeof(int) || userIdProperty.PropertyType == typeof(int?)) &&
                        userIdProperty.CanWrite)
                    {
                        userIdProperty.SetValue(argument, userId);
                    }
                }
            }
        }
    }
}