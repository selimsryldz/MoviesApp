using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesProject.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExist = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153,
        UserAlreadyActive = 154,
        ActivateIdDoesNotExists = 155,
        UserNotFound = 156,
        UpdateFailed = 157,
        UserCreateFailed = 158
    }
}