using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Webkit;
using Java.Lang;
using Android.Util;
using Java.Security;
using System.Text;
using Android.Net.Http;
using Java.Interop;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;

namespace PayUApplication
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static string txnid;
        private static string TAG = "MainActivity";
        private static WebView webviewPayment;
        private WebViewClient webViewClient = new MyWebViewClient();
        private static Context context;
        private static string SUCCESS_URL = "https://www.payumoney.com/mobileapp/payumoney/success.php";
        private static string FAILED_URL = "https://www.payumoney.com/mobileapp/payumoney/failure.php";
        private static string firstname = "kiran";
        private static string email = "kiran@gmail.com";
        private static string productInfo = "test";
        private static string mobile = "7090751801";
        public static string totalAmount = "100.00";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //  webviewPayment = (WebView)FindViewById(Resource.Layout.activity_main);
            SetContentView(Resource.Layout.activity_main);
            webviewPayment = FindViewById<WebView>(Resource.Id.webView1);
            webviewPayment.SetBackgroundColor(Android.Graphics.Color.White);
            webviewPayment.Settings.JavaScriptEnabled = true;
            webviewPayment.Settings.SetSupportZoom(true);
            webviewPayment.Settings.DomStorageEnabled = true;
            webviewPayment.Settings.LoadWithOverviewMode = true;
            webviewPayment.Settings.UseWideViewPort = true;
            webviewPayment.Settings.CacheMode = CacheModes.NoCache;
            webviewPayment.Settings.SetSupportMultipleWindows(true);
            webviewPayment.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            webviewPayment.AddJavascriptInterface(new PayUJavaScriptInterface(this), "PayUMoney"); //JavaInterface  
            Java.Lang.StringBuilder url_s = new Java.Lang.StringBuilder();
            //url_s.Append("https://test.payu.in/_payment"); //PauMoney Test URL  //https://sandboxsecure.payu.in/_payment
            url_s.Append("https://sandboxsecure.payu.in/_payment");
            Log.Info(TAG, "call url " + url_s);
            webviewPayment.PostUrl(url_s.ToString(), Encoding.UTF8.GetBytes(getPostString()));
            webviewPayment.SetWebViewClient(webViewClient);
            // Set our view from the "main" layout resource
            // SetContentView(Resource.Layout.activity_main);
        }

        private string getPostString()
        {
            string TxtStr = Generate();
            string txnid = hashCal("SHA-256", TxtStr).Substring(0, 20);
            //txnid = "TXN" + txnid;
            //string key = "rjQUPktU"; //Key  //merchent id:zTYLJAQ5
            //string salt = "e5iIg1jwi8"; //salt:s6nGdvEf79
            string key = "gtKFFx"; //Key  //merchent id:zTYLJAQ5
            string salt = "eCwWELxi"; //salt:s6nGdvEf79
                                      //string key = "vpJ7Bom9"; //Key  //merchent id:zTYLJAQ5
                                      //string salt = "SFOJ0eRkiO"; //salt:s6nGdvEf79
            Java.Lang.StringBuilder post = new Java.Lang.StringBuilder();
            post.Append("key=");
            post.Append(key);
            post.Append("&");
            post.Append("txnid=");
            post.Append(txnid);
            post.Append("&");
            post.Append("amount=");
            post.Append(totalAmount);
            post.Append("&");
            post.Append("productinfo=");
            post.Append(productInfo);
            post.Append("&");
            post.Append("firstname=");
            post.Append(firstname);
            post.Append("&");
            post.Append("email=");
            post.Append(email);
            post.Append("&");
            post.Append("phone=");
            post.Append(mobile);
            post.Append("&");
            post.Append("surl=");
            post.Append(SUCCESS_URL);
            post.Append("&");
            post.Append("furl=");
            post.Append(FAILED_URL);
            post.Append("&");
            Java.Lang.StringBuilder checkSumStr = new Java.Lang.StringBuilder();
            MessageDigest digest = null;
            byte[] hash;
            try
            {
                digest = MessageDigest.GetInstance("SHA-512"); // MessageDigest.getInstance("SHA-256");  
                checkSumStr.Append(key);
                checkSumStr.Append("|");
                checkSumStr.Append(txnid);
                checkSumStr.Append("|");
                checkSumStr.Append(totalAmount);
                checkSumStr.Append("|");
                checkSumStr.Append(productInfo);
                checkSumStr.Append("|");
                checkSumStr.Append(firstname);
                checkSumStr.Append("|");
                checkSumStr.Append(email);
                //checkSumStr.Append("|||||");
                //checkSumStr.Append("BOLT_KIT_ASP.NET");
                checkSumStr.Append("|||||||||||");
                checkSumStr.Append(salt);
                // digest.Update(Encoding.ASCII.GetBytes(checkSumStr.ToString()));
                var datab = Encoding.UTF8.GetBytes(checkSumStr.ToString());

                //hash = bytesToHexString(digest.Digest());
                //    hash=  GenerateSHA512String(datab);
                using (SHA512 shaM = new SHA512Managed())
                {
                    hash = shaM.ComputeHash(datab);
                }

                post.Append("hash=");
                post.Append(hash);
                post.Append("&");
                Log.Info(TAG, "SHA result is " + hash);
            }
            catch (NoSuchAlgorithmException e1)
            {
                // TODO Auto-generated catch block  
                e1.PrintStackTrace();
            }
            //post.Append("service_provider=");
            //post.Append("payu_paisa");
            return post.ToString();

        }
        // Convert Byte to Hash key Values: -  
        //string Byte to Hash key Conversation  
        private static string bytesToHexString(byte[] bytes)
        {
            StringBuffer sb = new StringBuffer();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = Integer.ToHexString(0xFF & bytes[i]);
                if (hex.Length == 1)
                {
                    sb.Append('0');
                }
                sb.Append(hex);
            }
            return sb.ToString();
        }


        public string hashCal(string type, string str)
        {
            StringBuffer hexString = new StringBuffer();
            try
            {
                MessageDigest digestTxID = null;
                digestTxID = MessageDigest.GetInstance(type);
                digestTxID.Reset();
                digestTxID.Update(Encoding.ASCII.GetBytes(str.ToString()));
                byte[] messageDigest = digestTxID.Digest();
                for (int i = 0; i < messageDigest.Length; i++)
                {
                    string hex = Integer.ToHexString(0xFF & messageDigest[i]);
                    if (hex.Length == 1) hexString.Append("0");
                    hexString.Append(hex);
                }
            }
            catch (NoSuchAlgorithmException ex) { }
            return hexString.ToString();
        }




        //  Generate TxnID Random Number: -  
        //Txnid Generate  
        public string Generate()
        {
            long ticks = System.DateTime.Now.Ticks;
            System.Threading.Thread.Sleep(200);
            Java.Util.Random rnd = new Java.Util.Random();
            string rndm = Integer.ToString(rnd.NextInt()) + (System.DateTime.Now.Ticks - ticks / 1000);
            // int myRandomNo = rnd.Next(10000, 99999);  
            string txnid = hashCal("SHA-256", rndm).Substring(0, 20);
            return txnid;
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        private static string GetStringFromHash(byte[] hash)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}


