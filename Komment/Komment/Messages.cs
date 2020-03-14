using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komment
{

    public enum RegistrationResponse
    {
        Success,
        Error,
        UserExists
    }

    public enum LoginResponse
    {
        Success,
        Unauthanticated,
        Error
    }

    public enum UpdateNoteResponse
    {
        Success,
        Unauthanticated,
        Error
    }
    
    public enum DeleteNoteRespnse
    {
        Success,
        Unauthanticated,
        Error
    }
}
