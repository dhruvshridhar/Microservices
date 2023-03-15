using System;
namespace Mango.Services.Identity.Initializer
{
    //To create seed users
    public interface IDbInitializer
    {
        public void Initialize();
    }
}

