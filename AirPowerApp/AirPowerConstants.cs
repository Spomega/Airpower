using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPowerApp
{
    class AirPowerConstants
    {

        public static string server_url = "http://getonairpower.com/api/v1/";
        public static readonly string  WELCOME = "welcome";
        public static readonly string LOGIN = "account/login";
        public static readonly string REGISTER = "account/register";
        public static readonly string ACTIVATE = "account/activate";
        public static readonly string GET_ITEMS = "items/getitems";
        public static readonly string GET_PAYOPTIONS = "items/getPaymentOptions";
        public static readonly string MPOWER_PAY = "items/mpowerPay";
        public static readonly string VISA_PAY ="items/visaPay";
        public static readonly string MOBILE_PAY = "items/mobilePay";
        public static readonly string MPOWER_CONFIRM = "items/mpowerConfirm";
        public static readonly string GET_LIBRARY = "items/getItemLibrary";
        public static readonly string CATEGORIES = "items/getcategories";
        public static readonly string FACEBOOK_LOGIN = "account/facebookSignup";
        public static readonly string SEARCH_ITEMS = "items/searchitems";
        public static readonly string PASSWORD_RESET = "account/passwordReset";
        public static readonly string FORGET_PASSWORD = "account/forgetPassword";
        public static readonly string FORGET_PASSWORD_CONFIRM = "account/forgetPasswordConfirm";
        public static readonly string ERROR_MESSAGE = "Sorry Could Not Access Server,Please Check Your Internet Connection";
       

    }
}
