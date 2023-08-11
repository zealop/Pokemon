using System.Text.RegularExpressions;

namespace Game.Utils
{
    public class Utils
    {
        public static string ToID(object text)
        {
            if (text is null)
            {
                return "";
            }

            if (text.GetAnonymousProp("id") is string id)
            {
                return id;
            }

            if (text.GetAnonymousProp("userid") is string userid)
            {
                return userid;
            }

            if (text.GetAnonymousProp("roomid") is string roomid)
            {
                return roomid;
            }

            if (text is not string && text is not int)
            {
                return "";
            }

            return Regex.Replace(text.ToString().ToLower(), "/[^a-z0-9]+/g", "");
        }
    }
}