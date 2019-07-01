using MoviesProject.Database;
using MoviesProject.Messages;
using MoviesProject.Repository;
using MoviesProject.Results;
using MoviesProject.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesProject.Managers
{
    public class UserManager
    {
        private Repository<User> repo_user = new Repository<User>();
        public Result<User> RegisterUser(RegisterViewModel data)
        {
            //MovieDBEntities db = new MovieDBEntities();
            User user = repo_user.Find(x => x.Username == data.Username || x.Email == data.EMail);
            Result<User> res = new Result<User>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                }
                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı");
                }
            }
            else
            {
                int dbResult = repo_user.Insert(new User()
                {

                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),

                    IsActive = true,
                    IsAdmin = false


                });

                if (dbResult > 0)
                {
                    res.Resultt = repo_user.Find(x => x.Email == data.EMail && x.Username == data.Username);

                }


            }
            return res;
        }
        public Result<User> LoginUser(LoginViewModel data)
        {

            try
            {
                Result<User> res = new Result<User>();
                var a = repo_user.List();
                res.Resultt = repo_user.Find(x => x.Username == data.Username && x.Password == data.Password);

                if (res.Resultt != null)
                {
                    if (!res.Resultt.IsActive)
                    {
                        res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir. Lütfen e-posta adresinizi kontrol ediniz.");
                    }

                }
                else
                {
                    res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
                }
                return res;
            }
            catch (Exception e)
            {

                throw;
            }

        }
    
}
}