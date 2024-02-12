using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using DevExpress.Web.Internal;
using DevExpress.Web;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Diagnostics;

/// <summary>
/// Summary description for Utils
/// </summary>
    public static class Utils
    {
        static HttpContext Context { get { return HttpContext.Current; } }
        static List<NavigationItem> _navigationItems;
        // update : 5/April/2018
        static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static bool fBrowserIsMobile()
        {
            Debug.Assert(HttpContext.Current != null);
            if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                if (u.Length < 4)
                    return false;

                if (MobileCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0, 4)))
                    return true;
            }

            return false;
        }
        public static string CurrentPageName
        {
            get
            {
                var key = "CE1167E3-A068-4E7C-8BFD-4A7D308BEF43";
                if (Context.Items[key] == null)
                    Context.Items[key] = GetCurrentPageName();
                return Context.Items[key].ToString();
            }
        }
        public static string UserName
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.User == null)
                    return null;
                else
                    return HttpContext.Current.User.Identity.Name;
            }
        }
        static string GetCurrentPageName()
        {
            var fileName = Path.GetFileName(Context.Request.Path);
            var result = fileName.Substring(0, fileName.Length - 5);
            if (result.ToLower() == "default")
                result = "mail";
            if (result.ToLower().Contains("print"))
                result = "print";
            return result.ToLower();
        }
    public static List<NavigationItem> NavigationItems
    {
        get
        {
            if (_navigationItems == null)
            {
                _navigationItems = new List<NavigationItem>();
                PopuplateNavigationItems(_navigationItems);
            }
            return _navigationItems;
        }
    }
    static void PopuplateNavigationItems(List<NavigationItem> list)
    {
        var path = Utils.Context.Server.MapPath("~/App_Data/Navigation.xml");
        list.AddRange(XDocument.Load(path).Descendants("Item").Select(n => new NavigationItem()
        {
            Text = n.Attribute("Text").Value,
            NavigationUrl = n.Attribute("NavigateUrl").Value,
            SpriteClassName = n.Attribute("SpriteClassName").Value
        }));
    }
    public static void EnsureRequestValidationMode()
        {
            try
            {
                if (Environment.Version.Major >= 4)
                {
                    Type type = typeof(WebControl).Assembly.GetType("System.Web.Configuration.RuntimeConfig");
                    MethodInfo getConfig = type.GetMethod("GetConfig", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { }, null);
                    object runtimeConfig = getConfig.Invoke(null, null);
                    MethodInfo getSection = type.GetMethod("GetSection", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(Type) }, null);
                    HttpRuntimeSection httpRuntimeSection = (HttpRuntimeSection)getSection.Invoke(runtimeConfig, new object[] { "system.web/httpRuntime", typeof(HttpRuntimeSection) });
                    FieldInfo bReadOnly = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                    bReadOnly.SetValue(httpRuntimeSection, false);

                    PropertyInfo pi = typeof(HttpRuntimeSection).GetProperty("RequestValidationMode");
                    if (pi != null)
                    {
                        Version version = (Version)pi.GetValue(httpRuntimeSection, null);
                        Version RequiredRequestValidationMode = new Version(2, 0);
                        if (version != null && !Version.Equals(version, RequiredRequestValidationMode))
                        {
                            pi.SetValue(httpRuntimeSection, RequiredRequestValidationMode, null);
                        }
                    }
                    bReadOnly.SetValue(httpRuntimeSection, true);
                }
            }
            catch
            {
            }
        }
    public static string CurrentTheme
    {
        get
        {
            var themeCookie = Context.Request.Cookies["MailDemoCurrentTheme"];
            var res = Utils.fBrowserIsMobile() ? "iOS" : "DevEx";
            return themeCookie == null ? res : HttpUtility.UrlDecode(themeCookie.Value);
        }
    }
    public static void ApplyTheme(Page page)
    {
        var themeName = CurrentTheme;
        if (string.IsNullOrEmpty(themeName))
            themeName = Utils.fBrowserIsMobile() ? "iOS" : "DevEx";
        page.Theme = themeName;
    }
}
    public class NavigationItem
    {
        public string Text { get; set; }
        public string NavigationUrl { get; set; }
        public string SpriteClassName { get; set; }
    }
    public class DemoException : Exception {
        public DemoException(string message) : base(message) { }
    }